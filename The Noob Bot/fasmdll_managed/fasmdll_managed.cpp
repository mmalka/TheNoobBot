#include "fasmdll_managed.h"

#pragma region UNMANAGED
#pragma unmanaged

typedef struct _c_FasmLineHeader
{
	char * file_path;
	DWORD line_number;
	union
	{
		DWORD file_offset;
		DWORD macro_offset_line;
	};
	_c_FasmLineHeader * macro_line;
} _C_FASM_LINE_HEADER;

typedef struct _c_FasmState
{
	int condition;
	union
	{
		int error_code;
		DWORD output_length;
	};
	union
	{
		BYTE * output_data;
		_c_FasmLineHeader * error_data;
	};
} _C_FASM_STATE;

extern "C" DWORD fasm_Assemble(char * szSource, BYTE * lpMemory, int nSize, int nPassesLimit);

BYTE * _c_fasm_memorybuf;

DWORD _c_FasmAssemble(char * szSource, DWORD nMemorySize, DWORD nPassesLimit)
{
	DWORD dwFasmReturn;
	//_C_FASM_STATE *fasm_state;

	if (strlen(szSource) == 0)
		return NULL;

	if (nPassesLimit == 0)
		nPassesLimit = DEFAULT_PASS_LIMIT;

	if (nMemorySize == 0)
		nMemorySize = DEFAULT_MEMORY_SIZE;


	if (_c_fasm_memorybuf)
		delete[] _c_fasm_memorybuf;

	_c_fasm_memorybuf = new BYTE[nMemorySize];

	dwFasmReturn = fasm_Assemble(szSource, _c_fasm_memorybuf, nMemorySize, nPassesLimit);

	//fasm_state = reinterpret_cast<_C_FASM_STATE *>(_c_fasm_memorybuf);

	return dwFasmReturn;
}
#pragma endregion

#pragma managed

namespace Fasm
{
	ManagedFasm::ManagedFasm()
	{
		m_AssemblyString = gcnew StringBuilder("use32\n");
		m_ThreadHandles = gcnew List<IntPtr>();

		m_MemorySize = DEFAULT_MEMORY_SIZE;
		m_PassLimit = DEFAULT_PASS_LIMIT;
	}

	ManagedFasm::ManagedFasm(IntPtr hProcess)
	{
		m_hProcess = hProcess;

		m_AssemblyString = gcnew StringBuilder("use32\n");
		m_ThreadHandles = gcnew List<IntPtr>();

		m_MemorySize = DEFAULT_MEMORY_SIZE;
		m_PassLimit = DEFAULT_PASS_LIMIT;
	}

	ManagedFasm::~ManagedFasm()
	{
		for (int i = 0; i < m_ThreadHandles->Count; i++)
			CloseHandle((HANDLE)(m_ThreadHandles[i].ToInt32()));
		m_ThreadHandles->Clear();
	}

	void ManagedFasm::AddLine(String ^ szLine)
	{
		m_AssemblyString->Append(szLine + "\n");
	}

	void ManagedFasm::AddLine(String ^ szFormatString, ... array<Object ^> ^ args)
	{
		m_AssemblyString->AppendFormat(szFormatString + "\n", args);
	}

	void ManagedFasm::Add(String ^ szLine)
	{
		m_AssemblyString->Append(szLine);
	}

	void ManagedFasm::Add(String ^ szFormatString, ... array<Object ^> ^ args)
	{
		m_AssemblyString->AppendFormat(szFormatString, args);
	}

	void ManagedFasm::InsertLine(String ^ szLine, int nIndex)
	{
		m_AssemblyString->Insert(nIndex, szLine + "\n");
	}

	void ManagedFasm::Insert(String ^ szLine, int nIndex)
	{
		m_AssemblyString->Insert(nIndex, szLine);
	}

	array<Byte> ^ ManagedFasm::Assemble()
	{
		return ManagedFasm::Assemble(m_AssemblyString->ToString(), m_MemorySize, m_PassLimit);
	}

	void ManagedFasm::Clear()
	{
		m_AssemblyString = gcnew StringBuilder("use32\n");
	}

	bool ManagedFasm::Inject(IntPtr hProcess, DWORD dwAddress)
	{
		if (hProcess == IntPtr::Zero)
			return false;

		if (m_AssemblyString->ToString()->Contains("use64") || m_AssemblyString->ToString()->Contains("use16"))
			m_AssemblyString->Replace("use32\n", "");

		if (!m_AssemblyString->ToString()->Contains("org "))
			m_AssemblyString->Insert(0, String::Format("org 0x{0:X08}\n", dwAddress));

		IntPtr lpSource = IntPtr::Zero;

		try
		{
			lpSource = Marshal::StringToHGlobalAnsi(m_AssemblyString->ToString());
			_c_FasmAssemble((char *)lpSource.ToPointer(), m_MemorySize, m_PassLimit);
		}
		catch (Exception ^ ex)
		{
			Console::WriteLine(ex->Message);
			return false;
		}
		finally
		{
			if (lpSource != IntPtr::Zero)
				Marshal::FreeHGlobal(lpSource);
		}

		_C_FASM_STATE * fasm_state = reinterpret_cast<_C_FASM_STATE *>(_c_fasm_memorybuf);
		if (fasm_state->condition != FASM_OK)
			throw gcnew Exception(String::Format("Assembly failed!  Error code: {0};  Error Line: {1}", fasm_state->error_code, fasm_state->error_data->line_number));
		
		return WriteProcessMemory((HANDLE)hProcess, (void *)dwAddress, fasm_state->output_data, fasm_state->output_length, NULL);
	}

	bool ManagedFasm::Inject(DWORD dwAddress)
	{
		return this->Inject(m_hProcess, dwAddress);
	}

	DWORD ManagedFasm::InjectAndExecute(IntPtr hProcess, DWORD dwAddress, DWORD dwParameter)
	{
		if (hProcess == IntPtr::Zero)
			throw gcnew ArgumentNullException("hProcess");

		if (dwAddress == NULL)
			throw gcnew ArgumentNullException("dwAddress");

		HANDLE hThread;
		DWORD dwExitCode = 0;

		try {
			if (!this->Inject(hProcess, dwAddress))
				throw gcnew Exception("Injection failed for some reason.");
		} 
		catch (Exception ^ ex)
		{
			throw ex;
		}

		hThread = CreateRemoteThread((HANDLE)(hProcess.ToInt32()), NULL, 0, (LPTHREAD_START_ROUTINE)dwAddress, (void *)dwParameter, 0, NULL);
		if (hThread == NULL)
			throw gcnew Exception("Remote thread failed.");

		try
		{
			if (WaitForSingleObject(hThread, 10000) == WAIT_OBJECT_0)
				if (!GetExitCodeThread(hThread, &dwExitCode))
					throw gcnew Exception("Could not get thread exit code.");
		}
		finally
		{
			CloseHandle(hThread);
		}
		
		return dwExitCode;
	}

	DWORD ManagedFasm::InjectAndExecute(IntPtr hProcess, DWORD dwAddress)
	{
		return this->InjectAndExecute(hProcess, dwAddress, NULL);
	}

	DWORD ManagedFasm::InjectAndExecute(DWORD dwAddress)
	{
		return this->InjectAndExecute(m_hProcess, dwAddress, NULL);
	}

	IntPtr ManagedFasm::InjectAndExecuteEx(IntPtr hProcess, DWORD dwAddress, DWORD dwParameter)
	{
		HANDLE hThread;

		this->Inject(hProcess, dwAddress);

		hThread = CreateRemoteThread((HANDLE)(hProcess.ToInt32()), NULL, 0, (LPTHREAD_START_ROUTINE)dwAddress, (void *)dwParameter, 0, NULL);
		m_ThreadHandles->Add((IntPtr)hThread);
		return (IntPtr)hThread;
	}

	IntPtr ManagedFasm::InjectAndExecuteEx(IntPtr hProcess, DWORD dwAddress)
	{
		return this->InjectAndExecuteEx(hProcess, dwAddress, NULL);
	}

	IntPtr ManagedFasm::InjectAndExecuteEx(DWORD dwAddress)
	{
		return this->InjectAndExecuteEx(m_hProcess, dwAddress, NULL);
	}

#pragma region Static Methods
	array<Byte> ^ ManagedFasm::Assemble(String ^ szSource)
	{
		return ManagedFasm::Assemble(szSource, DEFAULT_MEMORY_SIZE, DEFAULT_PASS_LIMIT);
	}

	array<Byte> ^ ManagedFasm::Assemble(String ^ szSource, int nMemorySize)
	{
		return ManagedFasm::Assemble(szSource, nMemorySize, DEFAULT_PASS_LIMIT);
	}

	array<Byte> ^ ManagedFasm::Assemble(String ^ szSource, int nMemorySize, int nPassLimit)
	{
		array<Byte> ^ bBytecode;
		DWORD dwAssembleRet;
		_C_FASM_STATE *fasm_state;
		IntPtr lpSource;

		lpSource = Marshal::StringToHGlobalAnsi(szSource);

		dwAssembleRet = _c_FasmAssemble((char *)lpSource.ToPointer(), nMemorySize, nPassLimit);
		fasm_state = reinterpret_cast<_C_FASM_STATE *>(_c_fasm_memorybuf);
		
		Marshal::FreeHGlobal(lpSource);

		if (fasm_state->condition == FASM_OK)
		{
			bBytecode = gcnew array<Byte>(fasm_state->output_length);
			Marshal::Copy((IntPtr)(fasm_state->output_data), bBytecode, 0, fasm_state->output_length);
		}
		else
		{
			throw gcnew Exception(String::Format("Assembly failed!  Error code: {0};  Error Line: {1}", fasm_state->error_code, fasm_state->error_data->line_number));
		}
		
		return bBytecode;
	}
#pragma endregion
}