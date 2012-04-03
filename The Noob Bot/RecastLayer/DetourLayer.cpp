#include "DetourLayer.h"

#pragma managed

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace RecastLayer;

namespace DetourLayer
{

	bool Detour::CreateNavMeshData([Out] array<unsigned char>^% data, PolyMesh^ pm, PolyMeshDetail^ dm, int tileX, int tileY, array<float>^ bmin, array<float>^ bmax, float walkableHeight, float walkableRadius, float walkableClimb, float cs, float ch, int tileSize, array<OffMeshConnection^>^ offMeshCons)
	{
		dtNavMeshCreateParams params;
		// PolyMesh data
		params.verts = pm->GetNativeObject()->verts;
		params.vertCount = pm->GetNativeObject()->nverts;
		params.polys = pm->GetNativeObject()->polys;
		params.polyAreas = pm->GetNativeObject()->areas;
		params.polyFlags = pm->GetNativeObject()->flags;
		params.polyCount = pm->GetNativeObject()->npolys;
		params.nvp = pm->GetNativeObject()->nvp;
		// PolyMeshDetail data
		params.detailMeshes = dm->GetNativeObject()->meshes;
		params.detailVerts = dm->GetNativeObject()->verts;
		params.detailVertsCount = dm->GetNativeObject()->nverts;
		params.detailTris = dm->GetNativeObject()->tris;
		params.detailTriCount = dm->GetNativeObject()->ntris;
		// Copy bounding box
		params.bmin[0] = bmin[0];
		params.bmin[1] = bmin[1];
		params.bmin[2] = bmin[2];
		params.bmax[0] = bmax[0];
		params.bmax[1] = bmax[1];
		params.bmax[2] = bmax[2];
		// General settings
		params.ch = ch;
		params.cs = cs;
		params.walkableClimb = walkableClimb;
		params.walkableHeight = walkableHeight;
		params.walkableRadius = walkableRadius;
		params.tileX = tileX;
		params.tileY = tileY;
		params.tileSize = tileSize;
		
		// Generate off mesh connection data
		if (offMeshCons != nullptr)
		{
			params.offMeshConCount = offMeshCons->Length;
			auto offMeshConAreas = new unsigned char[offMeshCons->Length];
			for (int i = 0; i < offMeshCons->Length; i++)
				offMeshConAreas[i] = (unsigned char)offMeshCons[i]->AreaId;
			params.offMeshConAreas = offMeshConAreas;
			auto offMeshConDir = new unsigned char[offMeshCons->Length];
			for (int i = 0; i < offMeshCons->Length; i++)
				offMeshConDir[i] = (unsigned char)offMeshCons[i]->Type;
			params.offMeshConDir = offMeshConDir;
			auto offMeshConFlags = new unsigned short[offMeshCons->Length];
			for (int i = 0; i< offMeshCons->Length; i++)
				offMeshConFlags[i] = (unsigned short)offMeshCons[i]->Flags;
			params.offMeshConFlags = offMeshConFlags;
			auto offMeshConRad = new float[offMeshCons->Length];
			for (int i = 0; i < offMeshCons->Length; i++)
				offMeshConRad[i] = offMeshCons[i]->Radius;
			params.offMeshConRad = offMeshConRad;
			auto offMeshConUserID = new unsigned int[offMeshCons->Length];
			for (int i = 0; i < offMeshCons->Length; i++)
				offMeshConUserID[i] = offMeshCons[i]->UserID;
			params.offMeshConUserID = offMeshConUserID;
			auto offMeshConVerts = new float[3 * 2 * offMeshCons->Length];
			for (int i = 0; i < offMeshCons->Length; i++)
			{
					offMeshConVerts[(i*6) + 0] = offMeshCons[i]->From[0];
					offMeshConVerts[(i*6) + 1] = offMeshCons[i]->From[1];
					offMeshConVerts[(i*6) + 2] = offMeshCons[i]->From[2];
					offMeshConVerts[(i*6) + 3] = offMeshCons[i]->To[0];
					offMeshConVerts[(i*6) + 4] = offMeshCons[i]->To[1];
					offMeshConVerts[(i*6) + 5] = offMeshCons[i]->To[2];
			}
			params.offMeshConVerts = offMeshConVerts;
		}
		else
			params.offMeshConCount = 0;

		int navDataSize;
		unsigned char* navData;
		bool result = dtCreateNavMeshData(&params, &navData, &navDataSize);
		if (result)
		{
			data = gcnew array<unsigned char>(navDataSize);
			for (int i = 0; i < navDataSize; i++)
				data[i] = navData[i];
			dtFree(navData);
			return true;
		}
		data = nullptr;
		return false;
	}

}