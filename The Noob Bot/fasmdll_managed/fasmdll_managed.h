#pragma once

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <stdio.h>

#include "defines.h"

#include "AssemblyInfo.cpp"

#using <mscorlib.dll>

using namespace System;
using namespace System::Text;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

namespace Fasm
{
	public ref class ManagedFasm
	{
	public:
		ManagedFasm();
		ManagedFasm(IntPtr hProcess);

		~ManagedFasm();

		void AddLine(String ^ szLine);
		void AddLine(String ^ szFormatString, ... array<Object ^> ^ args);
		void Add(String ^ szLine);
		void Add(String ^ szFormatString, ... array<Object ^> ^ args);
		void InsertLine(String ^ szLine, int nIndex);
		void Insert(String ^ szLine, int nIndex);
		void Clear();

		array<Byte> ^ Assemble();

		bool Inject(IntPtr hProcess, DWORD dwAddress);
		bool Inject(DWORD dwAddress);

		DWORD InjectAndExecute(IntPtr hProcess, DWORD dwAddress, DWORD dwParameter);
		DWORD InjectAndExecute(IntPtr hProcess, DWORD dwAddress);
		DWORD InjectAndExecute(DWORD dwAddress);

		IntPtr InjectAndExecuteEx(IntPtr hProcess, DWORD dwAddress, DWORD dwParameter);
		IntPtr InjectAndExecuteEx(IntPtr hProcess, DWORD dwAddress);
		IntPtr InjectAndExecuteEx(DWORD dwAddress);

		IntPtr GetProcessHandle() { return m_hProcess; }
		void SetProcessHandle(IntPtr Value) { m_hProcess = Value; }

		int GetMemorySize() { return m_MemorySize; }
		void SetMemorySize(int Value) { m_MemorySize = Value; }

		int GetPassLimit() { return m_PassLimit; }
		void SetPassLimit(int Value) { m_PassLimit = Value; }

		static array<Byte> ^ Assemble(String ^ szSource);
		static array<Byte> ^ Assemble(String ^ szSource, int nMemorySize);
		static array<Byte> ^ Assemble(String ^ szSource, int nMemorySize, int nPassLimit);

	private:
		StringBuilder ^ m_AssemblyString;
		List<IntPtr> ^ m_ThreadHandles;

		IntPtr m_hProcess;
		int m_MemorySize;
		int m_PassLimit;
	};
}
