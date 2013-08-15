#include "Recast/Recast.h"
#include <vcclr.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace RecastLayer
{

	public enum class LogCategory
	{
		Progress = 1,
		Warning,
		Error
	};

	public enum class TimerLabel
	{
		Total,
		Temp,
		RasterizeTriangles,
		BuildCompactHeightfield,
		BuildContours,
		BuildContoursTrace,
		BuildContoursSimplify,
		FilterBorder,
		FilterWalkable,
		MedianArea,
		FilterLowObstacles,
		BuildPolymesh,
		MergePolymesh,
		ErodeArea,
		MarkBoxArea,
		MarkConvexPolyArea,
		BuildDistanceField,
		BuildDistanceFieldDist,
		BuildDistanceFieldBlur,
		BuildRegions,
		BuildRegionsWatershed,
		BuildRegionsExpand,
		BuildRegionsFlood,
		BuildRegionsFilter,
		BuildPolymeshDetail,
		MergePolymeshDetail
	};

	public enum class PolyArea
	{
		Terrain = 1,
		Water = 2,
		Road = 3,
		Danger = 4,
	};

	[Flags]
	public enum class PolyFlag
	{
		Walk = 1,
		Swim = 2,
		FlightMaster = 4,
	};

	public interface class BuildContext
	{
		void ResetLog();
		void Log(LogCategory category, String^ message);
		void ResetTimers();
		void StartTimer(TimerLabel label);
		void StopTimer(TimerLabel label);
	};

	class NativeBuildContext : public rcContext
	{
	private:
		gcroot<BuildContext^> _handler;
	public:

		NativeBuildContext()
		{
			_handler = nullptr;
		}

		void SetHandler(BuildContext^ handler)
		{
			_handler = handler;
			_handler->Log(LogCategory::Warning, "Handler registered");
		}

		void doResetLog()
		{
			if (_handler)
				_handler->ResetLog();
		}

		void doLog(const rcLogCategory category, const char* msg, const int len)
		{
			// heres hope msg is null terminated - String only takes wchar_t* w/ length
			if (_handler)
				_handler->Log((LogCategory)((int)category), gcnew String(msg));
		}

		void doResetTimers()
		{
			if (_handler)
				_handler->ResetLog();
		}

		void doStartTimer(const rcTimerLabel label)
		{
			if (_handler)
				_handler->StartTimer((TimerLabel)((int)label));
		}

		void doStopTimer(const rcTimerLabel label)
		{
			if (_handler)
				_handler->StopTimer((TimerLabel)((int)label));
		}

		virtual int doGetAccumulatedTime(const rcTimerLabel label)
		{
			return -1;
		}
	};

	public ref class Heightfield
	{
	private:
		rcHeightfield* _heightfield;

	public:

		Heightfield(rcHeightfield* hf)
		{
			_heightfield = hf;
		}

		rcHeightfield* GetNativeObject()
		{
			return _heightfield;
		}

		void Delete()
		{
			rcFreeHeightField(_heightfield);
		}
	};

	public ref class CompactHeightfield
	{
	private:
		rcCompactHeightfield* _chf;

	public:

		CompactHeightfield(rcCompactHeightfield* chf)
		{
			_chf = chf;
		}

		rcCompactHeightfield* GetNativeObject()
		{
			return _chf;
		}

		void Delete()
		{
			rcFreeCompactHeightfield(_chf);
		}
	};

	public ref class ContourSet
	{
	private:
		rcContourSet* _cset;

	public:

		ContourSet(rcContourSet* cset)
		{
			_cset = cset;
		}

		rcContourSet* GetNativeObject()
		{
			return _cset;
		}

		void Delete()
		{
			rcFreeContourSet(_cset);
		}
	};

	public ref class PolyMesh
	{
	private:
		rcPolyMesh* _pm;
		
	public:

		PolyMesh(rcPolyMesh* pm)
		{
			_pm = pm;
		}

		rcPolyMesh* GetNativeObject()
		{
			return _pm;
		}

		void MarkAll()
		{
			for (int i = 0; i < _pm->npolys; i++)
			{
				if (_pm->areas[i] == (int)PolyArea::Road || _pm->areas[i] == (int)PolyArea::Terrain)
					_pm->flags[i] = (int)PolyFlag::Walk;
				else if (_pm->areas[i] == (int)PolyArea::Water)
					_pm->flags[i] = (int)PolyFlag::Swim;
			}
		}

		void Delete()
		{
			rcFreePolyMesh(_pm);
		}
	};

	public ref class PolyMeshDetail
	{
	private:
		rcPolyMeshDetail* _pmd;
		
	public:

		PolyMeshDetail(rcPolyMeshDetail* pmd)
		{
			_pmd = pmd;
		}

		rcPolyMeshDetail* GetNativeObject()
		{
			return _pmd;
		}

		void Delete()
		{
			rcFreePolyMeshDetail(_pmd);
		}
	};

	public ref class RecastContext
	{
	private:
		NativeBuildContext* _context;

	public:

		RecastContext()
		{
			_context = new NativeBuildContext();
		}

		~RecastContext()
		{
		}

		!RecastContext()
		{
			delete _context;
		}

		void SetContextHandler(BuildContext^ context);

		bool CreateHeightfield([Out] Heightfield^% hf, int width, int height, array<float>^ bmin, array<float>^ bmax, float cs, float ch);
		void MarkWalkableTriangles(float walkableSlopeAngle, array<float>^% vertices, array<int>^% tris, [Out] array<unsigned char>^% areas);
		void ClearUnwalkableTriangles(float walkableSlopeAngle, array<float>^% vertices, array<int>^% tris, array<unsigned char>^ areas);
		void RasterizeTriangles(array<float>^% vertices, array<int>^% tris, array<unsigned char>^% areas, Heightfield^ hf, int walkableClimb);
		void FilterLowHangingWalkableObstacles(const int walkableClimb, Heightfield^ solid);
		void FilterLedgeSpans(const int walkableHeight, const int walkableClimb, Heightfield^ solid);
		void FilterWalkableLowHeightSpans(int walkableHeight, Heightfield^ solid);
		bool BuildCompactHeightfield(const int walkableHeight, const int walkableClimb, Heightfield^ hf, [Out] CompactHeightfield^% chf);
		bool ErodeWalkableArea(int radius, CompactHeightfield^ chf);
		bool MedianFilterWalkableArea(CompactHeightfield^ chf);
		bool BuildDistanceField(CompactHeightfield^ chf);
		bool BuildRegions(CompactHeightfield^ chf, const int borderSize, const int minRegionArea, const int mergeRegionArea);
		bool BuildRegionsMonotone(CompactHeightfield^ chf, const int borderSize, const int minRegionArea, const int mergeRegionArea);
		bool BuildContours(CompactHeightfield^ chf, const float maxError, const int maxEdgeLen, [Out] ContourSet^% cset);
		bool BuildPolyMesh(ContourSet^ cset, int nvp, [Out] PolyMesh^% mesh);
		bool BuildPolyMeshDetail(PolyMesh^ mesh, CompactHeightfield^ chf, const float sampleDist, const float sampleMaxError, [Out] PolyMeshDetail^% dmesh);
	};

	public ref class Recast
	{
	public:
		static void CalcBounds(array<float>^ verts, [Out] array<float>^% bmin, [Out] array<float>^% bmax);
		static void CalcGridSize(array<float>^ bmin, array<float>^ bmax, float cs, [Out] int% w, [Out] int% h);
	};

}