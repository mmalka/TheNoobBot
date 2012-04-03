#include "RecastLayer.h"

#pragma managed

using namespace System;
using namespace System::Runtime::InteropServices;

namespace RecastLayer
{

		void Recast::CalcBounds(array<float>^ verts, [Out] array<float>^% bmin, [Out] array<float>^% bmax)
		{
			bmin = gcnew array<float>(3);
			bmax = gcnew array<float>(3);
			pin_ptr<float> bminPointer = &bmin[0];
			pin_ptr<float> bmaxPointer = &bmax[0];
			pin_ptr<float> verticesPointer = &verts[0];

			rcCalcBounds(verticesPointer, verts->Length / 3, bminPointer, bmaxPointer);
		}

		void Recast::CalcGridSize(array<float>^ bmin, array<float>^ bmax, float cs, [Out] int% w, [Out] int% h)
		{
			int width = 0, height = 0;
			pin_ptr<float> bminPointer = &bmin[0];
			pin_ptr<float> bmaxPointer = &bmax[0];
			rcCalcGridSize(bminPointer, bmaxPointer, cs, &width, &height);
			w = width;
			h = height;
		}

		bool RecastContext::CreateHeightfield([Out] Heightfield^% hf, int width, int height, array<float>^ bmin, array<float>^ bmax, float cs, float ch)
		{
			pin_ptr<float> bminPointer = &bmin[0];
			pin_ptr<float> bmaxPointer = &bmax[0];
			rcHeightfield* heightField = rcAllocHeightfield();
			bool result = rcCreateHeightfield(_context, *heightField, width, height, bminPointer, bmaxPointer, cs, ch);
			hf = gcnew Heightfield(heightField);
			return result;
		}

		void RecastContext::MarkWalkableTriangles(float walkableSlopeAngle, array<float>^% vertices, array<int>^% tris, [Out] array<unsigned char>^% areas)
		{
			pin_ptr<float> verticesPointer = &vertices[0];
			pin_ptr<int> trisPointer = &tris[0];
			areas = gcnew array<unsigned char>(tris->Length / 3);
			pin_ptr<unsigned char> areasPointer = &areas[0];
			rcMarkWalkableTriangles(_context, walkableSlopeAngle, verticesPointer, vertices->Length / 3, trisPointer, tris->Length / 3, areasPointer);
		}

		void RecastContext::ClearUnwalkableTriangles(float walkableSlopeAngle, array<float>^% vertices, array<int>^% tris, array<unsigned char>^ areas)
		{
			pin_ptr<float> verticesPointer = &vertices[0];
			pin_ptr<int> trisPointer = &tris[0];
			pin_ptr<unsigned char> areasPointer = &areas[0];
			rcClearUnwalkableTriangles(_context, walkableSlopeAngle, verticesPointer, vertices->Length / 3, trisPointer, tris->Length / 3, areasPointer);
		}

		void RecastContext::RasterizeTriangles(array<float>^% vertices, array<int>^% tris, array<unsigned char>^% areas, Heightfield^ hf, int walkableClimb)
		{
			pin_ptr<float> verticesPointer = &vertices[0];
			pin_ptr<int> trisPointer = &tris[0];
			pin_ptr<unsigned char> areasPointer = &areas[0];
			rcRasterizeTriangles(_context, verticesPointer, vertices->Length / 3, trisPointer, areasPointer, tris->Length / 3, *(hf->GetNativeObject()), walkableClimb);
		}

		void RecastContext::FilterLowHangingWalkableObstacles(const int walkableClimb, Heightfield^ solid)
		{
			rcFilterLowHangingWalkableObstacles(_context, walkableClimb, *(solid->GetNativeObject()));
		}

		void RecastContext::FilterLedgeSpans(const int walkableHeight, const int walkableClimb, Heightfield^ solid)
		{
			rcFilterLedgeSpans(_context, walkableHeight, walkableClimb, *(solid->GetNativeObject()));
		}

		void RecastContext::FilterWalkableLowHeightSpans(int walkableHeight, Heightfield^ solid)
		{
			rcFilterWalkableLowHeightSpans(_context, walkableHeight, *(solid->GetNativeObject()));
		}

		bool RecastContext::BuildCompactHeightfield(const int walkableHeight, const int walkableClimb, Heightfield^ hf, [Out] CompactHeightfield^% chf)
		{
			rcCompactHeightfield* nativeChf = rcAllocCompactHeightfield();
			bool result = rcBuildCompactHeightfield(_context, walkableHeight, walkableClimb, *(hf->GetNativeObject()), *nativeChf);
			chf = gcnew CompactHeightfield(nativeChf);
			return result;
		}

		bool RecastContext::ErodeWalkableArea(int radius, CompactHeightfield^ chf)
		{
			return rcErodeWalkableArea(_context, radius, *(chf->GetNativeObject()));
		}
		
		bool RecastContext::MedianFilterWalkableArea(CompactHeightfield^ chf)
		{
			return rcMedianFilterWalkableArea(_context, *(chf->GetNativeObject()));
		}

		bool RecastContext::BuildDistanceField(CompactHeightfield^ chf)
		{
			return rcBuildDistanceField(_context, *(chf->GetNativeObject()));
		}

		bool RecastContext::BuildRegions(CompactHeightfield^ chf, const int borderSize, const int minRegionArea, const int mergeRegionArea)
		{
			return rcBuildRegions(_context, *(chf->GetNativeObject()), borderSize, minRegionArea, mergeRegionArea);
		}

		bool RecastContext::BuildRegionsMonotone(CompactHeightfield^ chf, const int borderSize, const int minRegionArea, const int mergeRegionArea)
		{
			return rcBuildRegionsMonotone(_context, *(chf->GetNativeObject()), borderSize, minRegionArea, mergeRegionArea);
		}

		bool RecastContext::BuildContours(CompactHeightfield^ chf, const float maxError, const int maxEdgeLen, [Out] ContourSet^% cset)
		{
			rcContourSet* nativeCset = rcAllocContourSet();
			bool result = rcBuildContours(_context, *(chf->GetNativeObject()), maxError, maxEdgeLen, *nativeCset);
			cset = gcnew ContourSet(nativeCset);
			return result;
		}

		bool RecastContext::BuildPolyMesh(ContourSet^ cset, int nvp, [Out] PolyMesh^% mesh)
		{
			rcPolyMesh* nativePm = rcAllocPolyMesh();
			bool result = rcBuildPolyMesh(_context, *(cset->GetNativeObject()), nvp, *nativePm);
			mesh = gcnew PolyMesh(nativePm);
			return result;
		}

		bool RecastContext::BuildPolyMeshDetail(PolyMesh^ mesh, CompactHeightfield^ chf, const float sampleDist, const float sampleMaxError, [Out] PolyMeshDetail^% dmesh)
		{
			rcPolyMeshDetail* nativePmd = rcAllocPolyMeshDetail();
			bool result = rcBuildPolyMeshDetail(_context, *(mesh->GetNativeObject()), *(chf->GetNativeObject()), sampleDist, sampleMaxError, *nativePmd);
			dmesh = gcnew PolyMeshDetail(nativePmd);
			return result;
		}

		void RecastContext::SetContextHandler(BuildContext^ context)
		{
			_context->SetHandler(context);
		}
}
