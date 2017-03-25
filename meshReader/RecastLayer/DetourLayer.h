#include "Detour/DetourCommon.h"
#include "Detour/DetourNavMesh.h"
#include "Detour/DetourNavMeshQuery.h"
#include "Detour/DetourNavMeshBuilder.h"
#include "Detour/DetourStatus.h"
#include "RecastLayer.h"
#include "NavMeshQueryCallback.h"
#include <vcclr.h>
#include <cstring>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace RecastLayer;

namespace DetourLayer
{
[Flags]
public enum struct DetourStatus : System::UInt32
{
Failure = DT_FAILURE,
Success = DT_SUCCESS,
InProgress = DT_IN_PROGRESS,

WrongMagic = DT_WRONG_MAGIC,
WrongVersion = DT_WRONG_VERSION,
OutOfMemory = DT_OUT_OF_MEMORY,
InvalidParam = DT_INVALID_PARAM,
BufferTooSmall = DT_BUFFER_TOO_SMALL,
OutOfNodes = DT_OUT_OF_NODES,
PartialResult = DT_PARTIAL_RESULT
};

public enum struct ConnectionType
{
OneWay = 0,
BiDirectional = DT_OFFMESH_CON_BIDIR
};

public enum struct PolyType
{
Ground = DT_POLYTYPE_GROUND,
OffMeshConnection = DT_POLYTYPE_OFFMESH_CONNECTION
};

[Flags]
public enum struct StraightPathFlag
{
Start = DT_STRAIGHTPATH_START,
End = DT_STRAIGHTPATH_END,
OffMeshConnection = DT_STRAIGHTPATH_OFFMESH_CONNECTION
};

public enum struct SupportedVersion : System::UInt32
{
value = DT_NAVMESH_VERSION
};

public ref class Poly
{
private:
const dtPoly* _poly;

public:

// see MESH_NULL_IDX
static const unsigned short NullIndex = 0xffff;

Poly(const dtPoly* poly)
{
_poly = poly;
}

unsigned short GetVertice(int index)
{
return _poly->verts[index];
}

unsigned short GetNeighbor(int index)
{
return _poly->neis[index];
}

property unsigned int FirstLink
{
unsigned int get()
{
return _poly->firstLink;
}
}

property array<unsigned short>^ Vertices
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(DT_VERTS_PER_POLYGON);
for (int i = 0; i < DT_VERTS_PER_POLYGON; i++)
ret[i] = _poly->verts[i];
return ret;
}
}

property array<unsigned short>^ Neis
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(DT_VERTS_PER_POLYGON);
for (int i = 0; i < DT_VERTS_PER_POLYGON; i++)
ret[i] = _poly->neis[i];
return ret;
}
}

void Disable()
{
const_cast<dtPoly*>(_poly)->flags = 0x0000;
}

property PolyFlag Flags
{
PolyFlag get()
{
return (PolyFlag)_poly->flags;
}

void set(PolyFlag value)
{
const_cast<dtPoly*>(_poly)->flags = (unsigned short)value;
}
}

property unsigned char VerticeCount
{
unsigned char get()
{
return _poly->vertCount;
}
}

property unsigned char AreaAndType
{
unsigned char get()
{
return _poly->areaAndtype;
}
}

property PolyArea Area
{
PolyArea get()
{
return (PolyArea)_poly->getArea();
}
}

property PolyType Type
{
PolyType get()
{
return (PolyType)_poly->getType();
}
}
};

public ref class PolyDetail
{
private:
dtPolyDetail* _polyDetail;

public:

PolyDetail(dtPolyDetail* polyDetail)
{
_polyDetail = polyDetail;
}

property unsigned int VerticeBase
{
unsigned int get()
{
return _polyDetail->vertBase;
}
}

property unsigned int TriangleBase
{
unsigned int get()
{
return _polyDetail->triBase;
}
}

property unsigned int VerticeCount
{
unsigned int get()
{
return _polyDetail->vertCount;
}
}

property unsigned int TriangleCount
{
unsigned int get()
{
return _polyDetail->triCount;
}
}
};

public ref class Link
{
private:
dtLink* _link;

public:
static const unsigned int NullLink = DT_NULL_LINK;

Link(dtLink* link)
{
_link = link;
}

property dtPolyRef Reference
{
dtPolyRef get()
{
return _link->ref;
}
}

property unsigned int Next
{
unsigned int get()
{
return _link->next;
}
}

property unsigned char Edge
{
unsigned char get()
{
return _link->edge;
}
}

property unsigned char Side
{
unsigned char get()
{
return _link->side;
}
}

property unsigned char BMin
{
unsigned char get()
{
return _link->bmin;
}
}

property unsigned char BMax
{
unsigned char get()
{
return _link->bmax;
}
}
};

[Flags]
public enum struct TileFlag
{
TileFreeData = DT_TILE_FREE_DATA,
};

public ref class MeshHeader
{
private:
dtMeshHeader* _header;

public:

MeshHeader(dtMeshHeader* header)
{
_header = header;
}

property int PolygonCount
{
int get()
{
return _header->polyCount;
}
}

property int MaxLinkCount
{
int get()
{
return _header->maxLinkCount;
}
}

property int DetailMeshCount
{
int get()
{
return _header->detailMeshCount;
}
}

property int DetailVerticeCount
{
int get()
{
return _header->detailVertCount;
}
}

property int DetailTriangleCount
{
int get()
{
return _header->detailTriCount;
}
}

property int BVNodeCount
{
int get()
{
return _header->bvNodeCount;
}
}

property int OffMeshConCount
{
int get()
{
return _header->offMeshConCount;
}
}

property int OffMeshBase
{
int get()
{
return _header->offMeshBase;
}
}

property float WalkableHeight
{
float get()
{
return _header->walkableHeight;
}
}

property float WalkableClimb
{
float get()
{
return _header->walkableClimb;
}
}

property float WalkableRadius
{
float get()
{
return _header->walkableRadius;
}
}

property float BVQuantFactor
{
float get()
{
return _header->bvQuantFactor;
}
}

property int X
{
int get()
{
return _header->x;
}
}

property int Y
{
int get()
{
return _header->y;
}
}

property int UserId
{
int get()
{
return _header->userId;
}
}

property int VerticeCount
{
int get()
{
return _header->vertCount;
}
}

property int Magic
{
int get()
{
return _header->magic;
}
}

property int Version
{
int get()
{
return _header->version;
}
}

property array<float>^ BMax
{
array<float>^ get()
{
array<float>^ ret = gcnew array<float>(3);
ret[0] = _header->bmax[0];
ret[1] = _header->bmax[1];
ret[2] = _header->bmax[2];
return ret;
}
}

property array<float>^ BMin
{
array<float>^ get()
{
array<float>^ ret = gcnew array<float>(3);
ret[0] = _header->bmin[0];
ret[1] = _header->bmin[1];
ret[2] = _header->bmin[2];
return ret;
}
}
};

public ref class OffMeshConnection
{
private:
bool _isNative;
dtOffMeshConnection* _connection;

ConnectionType _type;
unsigned int _userID;
PolyFlag _flags;
PolyArea _areaType;
float _radius;
array<float>^ _from;
array<float>^ _to;

public:

OffMeshConnection()
{
_isNative = false;
}

OffMeshConnection(dtOffMeshConnection* connection)
{
_isNative = true;
_connection = connection;
}

property ConnectionType Type
{
ConnectionType get()
{
if (_isNative)
return (ConnectionType)_connection->flags;
return _type;
}

void set(ConnectionType value)
{
if (_isNative)
_connection->side = (unsigned char)value;
else
_type = value;
}
}

property unsigned short PolyId
{
unsigned short get()
{
if (_isNative)
return _connection->poly;
throw gcnew System::Exception("Only available with native OffMeshConnection");
}
}

property PolyArea AreaId
{
PolyArea get()
{
if (_isNative)
throw gcnew System::Exception("Only accessible from polygon with PolyId");
return _areaType;
}

void set(PolyArea value)
{
_areaType = value;
}
}

property unsigned int UserID
{
unsigned int get()
{
if (_isNative)
return _connection->userId;
return _userID;
}

void set(unsigned int value)
{
_userID = value;
}
}

property PolyFlag Flags
{
PolyFlag get()
{
if (_isNative)
throw gcnew System::Exception("Only accessible from polygon with PolyId");
return _flags;
}

void set(PolyFlag value)
{
_flags = value;
}
}

property float Radius
{
float get()
{
if (_isNative)
return _connection->rad;
return _radius;
}

void set(float value)
{
_radius = value;
}
}

property array<float>^ To
{
array<float>^ get()
{
if (_isNative)
{
auto ret = gcnew array<float>(3);
for (int i = 0; i < 3; i++)
ret[i] = _connection->pos[i];
return ret;
}
return _to;
}

void set(array<float>^ value)
{
_to = value;
}
}

property array<float>^ From
{
array<float>^ get()
{
if (_isNative)
{
auto ret = gcnew array<float>(3);
for (int i = 3; i < 6; i++)
ret[i] = _connection->pos[i];
return ret;
}
return _from;
}

void set(array<float>^ value)
{
_from = value;
}
}
};

public ref class MeshTile
{
private:
const dtMeshTile* _tile;
const dtNavMesh* _mesh;

public:

MeshTile(const dtMeshTile* tile, const dtNavMesh* mesh)
{
_tile = tile;
_mesh = mesh;
}

array<float>^ GetVertice(int index)
{
auto ret = gcnew array<float>(3);
ret[0] = _tile->verts[index*3 + 0];
ret[1] = _tile->verts[index*3 + 1];
ret[2] = _tile->verts[index*3 + 2];
return ret;
}

array<float>^ GetDetailVertice(int index)
{
auto ret = gcnew array<float>(3);
ret[0] = _tile->detailVerts[index*3 + 0];
ret[1] = _tile->detailVerts[index*3 + 1];
ret[2] = _tile->detailVerts[index*3 + 2];
return ret;
}

OffMeshConnection^ GetOffMeshConnection(int index)
{
return gcnew OffMeshConnection(&_tile->offMeshCons[index]);
}

Poly^ GetPolygon(unsigned short index)
{
return gcnew Poly(&_tile->polys[index]);
}

inline dtPolyRef GetPolygonRef(unsigned short index)
{
auto base = _mesh->getPolyRefBase(_tile);
return _mesh->encodePolyId(_mesh->decodePolyIdSalt(base), _mesh->decodePolyIdTile(base), index);
}

property TileFlag Flags
{
TileFlag get()
{
return (TileFlag)_tile->flags;
}
}

property MeshHeader^ Header
{
MeshHeader^ get()
{
if (!IsValid)
return nullptr;

return gcnew MeshHeader(_tile->header);
}
}

property unsigned int Salt
{
unsigned int get()
{
return _tile->salt;
}
}

property int DataSize
{
int get()
{
return _tile->dataSize;
}
}

property bool IsValid
{
bool get()
{
return _tile != 0 && _tile->header != 0;
}
}

Link^ GetLink(unsigned int index)
{
if (!_tile->links)
return nullptr;

return gcnew Link(&_tile->links[index]);
}

/* Disabled unused function
bool Rebuild(array<bool>^ visited, const int visitedMask, const float cellHeight, const int nvp, [Out] array<unsigned char>^% rData)
{
	const float cellSize = 1 / _tile->header->bvQuantFactor;
	auto baseRef = _mesh->getPolyRefBase(_tile);

	dtPolyRef* oldPolyIndices = new dtPolyRef[_tile->header->polyCount];
	dtPolyRef* newPolyIndices = new dtPolyRef[_tile->header->polyCount];
	if (!oldPolyIndices || !newPolyIndices)
		goto error;

	int polyCount = 0, portalCount = 0;
	int totalPolys = _tile->header->polyCount - _tile->header->offMeshConCount;
	for (int i = 0; i < totalPolys; i++)
	{
		if (!visited[GetPolygonRef(i) & visitedMask])
			continue;

		oldPolyIndices[polyCount] = i;
		newPolyIndices[i] = polyCount;
		polyCount++;

		for (int j = 0; j < _tile->polys[i].vertCount; j++)
		{
			// null index
			if (_tile->polys[i].neis[j] == 0xFFFF)
				continue;

			if (_tile->polys[i].neis[j] & DT_EXT_LINK)
				portalCount++;
		}
	}

	// if no polys are left, exit early
	if (!polyCount)
	{
		rData = nullptr;
		delete[] oldPolyIndices;
		delete[] newPolyIndices;
		return true;
	}

	const unsigned int maxLinkCount = _tile->header->maxLinkCount;
	const unsigned int headerSize = dtAlign4(sizeof(dtMeshHeader));
	const unsigned int vertsSize = dtAlign4(sizeof(float)*3*_tile->header->vertCount);
	const unsigned int polysSize = dtAlign4(sizeof(dtPoly) * polyCount);
	const unsigned int linksSize = dtAlign4(sizeof(dtLink)* maxLinkCount);
	const unsigned int detailMeshesSize = dtAlign4(sizeof(dtPolyDetail) * polyCount);
	const unsigned int detailVertsSize = dtAlign4(sizeof(float) * 3 * _tile->header->detailVertCount);
	const unsigned int detailTrisSize = dtAlign4(sizeof(unsigned char) * 4 * _tile->header->detailTriCount);
	const unsigned int bvTreeSize = dtAlign4(sizeof(dtBVNode) * 2 * polyCount);
	const unsigned int offMeshConsSize = dtAlign4(sizeof(dtOffMeshConnection) * _tile->header->offMeshConCount);
	const unsigned int dataSize = headerSize + vertsSize + polysSize + linksSize + detailMeshesSize + detailVertsSize + detailTrisSize + bvTreeSize + offMeshConsSize;

	unsigned char* data = new unsigned char[dataSize];
	if (!data)
		goto error;
	memset(data, 0, dataSize);
	unsigned char* d = data;
	dtMeshHeader* header = (dtMeshHeader*)d; d+= headerSize;
	float* navVerts = (float*)d; d+= vertsSize;
	dtPoly* navPolys = (dtPoly*)d; d+= polysSize;
	d += linksSize; // will be generated when inserting tile
	dtPolyDetail* navDMeshes = (dtPolyDetail*)d; d += detailMeshesSize;
	float* navDVerts = (float*)d; d += detailVertsSize;
	unsigned char* navDTris = d; d += detailTrisSize;
	dtBVNode* navBvTree = (dtBVNode*)d; d+= bvTreeSize;
	dtOffMeshConnection* offMeshCons = (dtOffMeshConnection*)d; d+= offMeshConsSize;

	// header
	header->magic = DT_NAVMESH_MAGIC;
	header->version = DT_NAVMESH_VERSION;
	header->polyCount = polyCount;
	header->vertCount = _tile->header->vertCount;
	header->maxLinkCount = maxLinkCount;
	memcpy((float*)header->bmin, _tile->header->bmin, sizeof(float) * 3);
	memcpy((float*)header->bmax, _tile->header->bmax, sizeof(float) * 3);
	header->detailMeshCount = polyCount;
	header->detailVertCount = _tile->header->detailVertCount;
	header->detailTriCount = _tile->header->detailTriCount;
	header->bvQuantFactor = _tile->header->bvQuantFactor;
	header->offMeshBase = newPolyIndices[_tile->header->offMeshBase];
	header->offMeshConCount = _tile->header->offMeshConCount;
	header->walkableClimb = _tile->header->walkableClimb;
	header->walkableHeight = _tile->header->walkableHeight;
	header->walkableRadius = _tile->header->walkableRadius;
	header->bvNodeCount = polyCount * 2;
	header->userId = _tile->header->userId;
	header->x = _tile->header->x;
	header->y = _tile->header->y;

	// vertices
	for (int i = 0; i < _tile->header->vertCount; i++)
	{
		const float* iv = &_tile->verts[i*3];
		float *v = &navVerts[i*3];
		memcpy(v, iv, sizeof(float)*3);
	}

	// polygons
	unsigned int currentLink = 0;
	for (int i = 0; i < polyCount; i++)
	{
		const dtPoly* ip = &_tile->polys[oldPolyIndices[i]];
		dtPoly* p = &navPolys[i];

		p->vertCount = ip->vertCount;
		p->flags = ip->flags;
		p->areaAndtype = ip->areaAndtype;

		for (unsigned int v = 0; v < p->vertCount; v++)
			p->verts[v] = ip->verts[v];

		for (unsigned int n = 0; n < DT_VERTS_PER_POLYGON; n++)
		{
			// we don't touch portals and non-existant neighbours
			if (ip->neis[n] & DT_EXT_LINK || !ip->neis[n])
				p->neis[n] = ip->neis[n];
			else
				p->neis[n] = newPolyIndices[ip->neis[n]-1]+1;
		}
	}

	delete[] newPolyIndices;

	for (int i = 0; i < polyCount; i++)
	{
		dtPolyDetail* dtl = &navDMeshes[i];
		dtPolyDetail* tileDtl = &_tile->detailMeshes[oldPolyIndices[i]];

		dtl->triBase = tileDtl->triBase;
		dtl->vertBase = tileDtl->vertBase;
		dtl->triCount = tileDtl->triCount;
		dtl->vertCount = tileDtl->vertCount;

		// triangles
		int base = dtl->triBase;
		for (int j = 0; j < tileDtl->triCount; j++)
		{
			memcpy(&navDTris[base++*4], &_tile->detailTris[tileDtl->triBase + j], sizeof(unsigned char) * 4);
		}

		//vertices
		int vbase = dtl->vertBase;
		for (int j = 0; j < tileDtl->vertCount; j++)
		{
			memcpy(&navDVerts[vbase++*3], &_tile->detailVerts[tileDtl->vertBase + j], sizeof(float) * 3);
		}
	}

	delete[] oldPolyIndices;

	// voxelize vertices for bvtree
	unsigned short* verts = new unsigned short[header->vertCount * 3];
	if (!verts)
		goto error;
	for (int i = 0; i < header->vertCount; i++)
	{
		verts[i*3 + 0] = (unsigned short)((navVerts[i*3 + 0] - header->bmin[0]) / cellSize);
		verts[i*3 + 1] = (unsigned short)((navVerts[i*3 + 1] - header->bmin[1]) / cellHeight);
		verts[i*3 + 2] = (unsigned short)((navVerts[i*3 + 2] - header->bmin[2]) / cellSize);
	}

	// voxelize polys for bvtree
	unsigned short* polys = new unsigned short[header->polyCount * nvp * 2];
	if (!polys)
		goto error;
	for (int i = 0; i < header->polyCount; i++)
	{
		for (int j = 0; j < nvp; j++)
		{
			polys[i*nvp*2 + j] = navPolys[i].verts[j];
			polys[i*nvp*2 + nvp +j] = navPolys[i].neis[j];
		}
	}

	// create and store bvtree
	createBVTree(verts, header->vertCount, polys, header->polyCount, nvp, cellSize, cellHeight, header->polyCount*2, navBvTree);
	delete[] verts;
	delete[] polys;

	// all done
	rData = gcnew array<unsigned char>(dataSize);
	Marshal::Copy((IntPtr)data, rData, 0, dataSize);
	return true;
	
error:
	if (oldPolyIndices)
		delete[] oldPolyIndices;
	if (newPolyIndices)
		delete[] newPolyIndices;
	if (data)
		delete[] data;
	if (verts)
		delete[] verts;
	if (polys)
		delete[] polys;

	rData = nullptr;
	return false;
}*/

};

public ref class QueryFilter
{
private:
dtQueryFilter* _filter;

public:

QueryFilter()
{
_filter = new dtQueryFilter();
}

~QueryFilter()
{

}

!QueryFilter()
{
delete _filter;
}

property unsigned short ExcludeFlags
{
unsigned short get()
{
return _filter->getExcludeFlags();
}

void set(unsigned short value)
{
_filter->setExcludeFlags(value);
}
}

property unsigned short IncludeFlags
{
unsigned short get()
{
return _filter->getIncludeFlags();
}

void set(unsigned short value)
{
_filter->setIncludeFlags(value);
}
}

float GetAreaCost(const int area)
{
return _filter->getAreaCost(area);
}

void SetAreaCost(const int area, const float cost)
{
_filter->setAreaCost(area, cost);
}

dtQueryFilter* GetNativeObject()
{
return _filter;
}
};

public ref class NavMeshParams
{
private:
bool _ownReference;
dtNavMeshParams* _params;

public:

NavMeshParams(dtNavMeshParams* params)
{
_params = params;
}

NavMeshParams()
{
_params = new dtNavMeshParams;
_ownReference = true;
}

!NavMeshParams()
{

}

~NavMeshParams()
{
if (_ownReference)
delete _params;
}

property array<float>^ Origin
{
array<float>^ get()
{
auto ret = gcnew array<float>(3);
ret[0] = _params->orig[0];
ret[1] = _params->orig[1];
ret[2] = _params->orig[2];
return ret;
}

void set(array<float>^ value)
{
_params->orig[0] = value[0];
_params->orig[1] = value[1];
_params->orig[2] = value[2];
}
}

property float TileWidth
{
float get()
{
return _params->tileWidth;
}

void set(float value)
{
_params->tileWidth = value;
}
}

property float TileHeight
{
float get()
{
return _params->tileHeight;
}

void set(float value)
{
_params->tileHeight = value;
}
}

property int MaxTiles
{
int get()
{
return _params->maxTiles;
}

void set(int value)
{
_params->maxTiles = value;
}
}

property int MaxPolygons
{
int get()
{
return _params->maxPolys;
}

void set(int value)
{
_params->maxPolys = value;
}
}

};

public ref class NavMesh
{
private:
dtNavMesh* _mesh;

public:

NavMesh()
{
_mesh = dtAllocNavMesh();
}

~NavMesh()
{
}

!NavMesh()
{
dtFreeNavMesh(_mesh);
}

dtNavMesh* GetNativeObject()
{
return _mesh;
}

property int MaxTiles
{
int get()
{
return _mesh->getMaxTiles();
}
}

int DecodePolyIndex(dtPolyRef ref)
{
return _mesh->decodePolyIdPoly(ref);
}

int DecodePolySalt(dtPolyRef ref)
{
return _mesh->decodePolyIdSalt(ref);
}

int DecodePolyTile(dtPolyRef ref)
{
return _mesh->decodePolyIdTile(ref);
}

MeshTile^ GetTile(int x, int y)
{
auto t = _mesh->getTileAt(x, y, 0);
if (t)
return gcnew MeshTile(t, _mesh);
return nullptr;
}

DetourStatus GetTileAndPolyByRef(dtPolyRef polyRef, [Out] MeshTile^% tile, [Out] Poly^% poly)
{
const dtMeshTile* nativeTile;
const dtPoly* nativePoly;
auto status = _mesh->getTileAndPolyByRef(polyRef, &nativeTile, &nativePoly);
if (dtStatusSucceed(status))
{
tile = gcnew MeshTile(nativeTile, _mesh);
poly = gcnew Poly(nativePoly);
}
else
{
tile = nullptr;
poly = nullptr;
}
return (DetourStatus)status;
}

dtTileRef GetTileReference(int x, int y)
{
return _mesh->getTileRefAt(x, y, 0);
}

bool HasTileAt(int x, int y)
{
const dtMeshTile* tile = _mesh->getTileAt(x, y, 0);
return tile && tile->header;
}

void GetTileByLocation(array<float>^ loc, [Out] int% x, [Out] int% y)
{
pin_ptr<float> locPointer = &loc[0];
pin_ptr<int> xPointer = &x;
pin_ptr<int> yPointer = &y;
_mesh->calcTileLoc(locPointer, xPointer, yPointer);
}

void GetTileBBox(int tX, int tY, [Out] array<float>^% bmin, [Out] array<float>^% bmax)
{
bmin = gcnew array<float>(3);
bmax = gcnew array<float>(3);
bmin[0] = _mesh->getParams()->orig[0] + (_mesh->getParams()->tileWidth * tX);
bmin[1] = _mesh->getParams()->orig[1];
bmin[2] = _mesh->getParams()->orig[2] + (_mesh->getParams()->tileHeight * tY);
bmax[0] = _mesh->getParams()->orig[0] + (_mesh->getParams()->tileWidth * (tX+1));
bmax[1] = _mesh->getParams()->orig[1];
bmax[2] = _mesh->getParams()->orig[2] + (_mesh->getParams()->tileHeight * (tY+1));
}

bool BuildRenderGeometry(int tileX, int tileY, [Out] array<float>^% verts, [Out] array<int>^% tris)
{
const dtMeshTile* tile = _mesh->getTileAt(tileX, tileY, 0);
if (!tile || !tile->header)
return false;

const dtMeshHeader* header = tile->header;
unsigned int nverts = 0, ntris = 0;

for (int i = 0; i < header->polyCount; i++)
{
const dtPoly* poly = &tile->polys[i];
if (poly->getType() == DT_POLYTYPE_OFFMESH_CONNECTION)
continue;

ntris += tile->detailMeshes[i].triCount;
}

nverts = 3 * ntris;

verts = gcnew array<float>(nverts * 3);
tris = gcnew array<int>(ntris * 3);

unsigned int cv = 0, ct = 0;
for (int i = 0; i < header->polyCount; i++)
{
const dtPoly* poly = &tile->polys[i];
if (poly->getType() == DT_POLYTYPE_OFFMESH_CONNECTION)
continue;

const dtPolyDetail* pd = &tile->detailMeshes[i];
for (int j = 0; j < pd->triCount; j++)
{
const unsigned char* t = &tile->detailTris[(pd->triBase+j)*4];
for (int k = 0; k < 3; k++)
{
pin_ptr<float> vertPtr = t[k] < poly->vertCount ? &tile->verts[poly->verts[t[k]]*3] : &tile->detailVerts[(pd->vertBase + t[k] - poly->vertCount) * 3];
pin_ptr<float> newVertPtr = &verts[cv * 3];
memcpy((void*)newVertPtr, vertPtr, sizeof(float) * 3);
cv++;
}

tris[ct * 3 + 0] = cv - 3;
tris[ct * 3 + 1] = cv - 2;
tris[ct * 3 + 2] = cv - 1;
ct++;
}
}
return true;
}

DetourStatus Initialize(int maxPolys, int maxTiles, array<float>^ orig, float tileHeight, float tileWidth)
{
dtNavMeshParams params;
params.maxPolys = maxPolys;
params.maxTiles = maxTiles;
params.orig[0] = orig[0];
params.orig[1] = orig[1];
params.orig[2] = orig[2];
params.tileHeight = tileHeight;
params.tileWidth = tileWidth;
return (DetourStatus)_mesh->init(&params);
}

DetourStatus Initialize(array<unsigned char>^ data)
{
unsigned char* copy = new unsigned char[data->Length];
Marshal::Copy(data, 0, (IntPtr)copy, data->Length);
return (DetourStatus)_mesh->init(copy, data->Length, DT_TILE_FREE_DATA);
}

DetourStatus AddTile(array<unsigned char>^ data, [Out] MeshTile^% tile)
{
return AddTile(data, 0, tile);
}

DetourStatus AddTile(array<unsigned char>^ data, unsigned int lastRef, [Out] MeshTile^% tile)
{
unsigned char* copy = new unsigned char[data->Length];
Marshal::Copy(data, 0, (IntPtr)copy, data->Length);
dtTileRef tileRef;
dtStatus status = _mesh->addTile(copy, data->Length, DT_TILE_FREE_DATA, lastRef, &tileRef);
if (dtStatusSucceed(status))
tile = gcnew MeshTile(_mesh->getTileByRef(tileRef), _mesh);
return (DetourStatus)status;
}

DetourStatus RemoveTileAt(int x, int y)
{
auto tileRef = _mesh->getTileRefAt(x, y, 0);
if (!tileRef)
return DetourStatus::Failure;
unsigned char* retData;
int retDataLength;
dtStatus status = _mesh->removeTile(tileRef, &retData, &retDataLength);

if (dtStatusSucceed(status))
{
delete[] retData;
return (DetourStatus)status;
}

return (DetourStatus)status;
}

DetourStatus RemoveTileAt(int x, int y, [Out] array<unsigned char>^% data)
{
auto ret = _mesh->getTileRefAt(x, y, 0);
if (!ret)
return DetourStatus::Failure;
unsigned char* retData;
int retDataLength;
auto status = _mesh->removeTile(ret, &retData, &retDataLength);
if (dtStatusSucceed(status) && retDataLength > 0)
{
data = gcnew array<unsigned char>(retDataLength);
for (int i = 0; i < retDataLength; i++)
data[i] = retData[i];
delete[] retData;
}
return (DetourStatus)status;
}

property NavMeshParams^ Parameter
{
NavMeshParams^ get()
{
return gcnew NavMeshParams(const_cast<dtNavMeshParams*>(_mesh->getParams()));
}
}
};

public ref class NavMeshQuery
{
private:
dtNavMeshQuery* _query;
dtNavMesh* _mesh;
NavMesh^ _managedMesh;
NavMeshQueryCallback^ _callback;

public:

NavMeshQuery(NavMeshQueryCallback^ callback)
{
_query = dtAllocNavMeshQuery();
_callback = callback;
}

NavMeshQuery()
{
_query = dtAllocNavMeshQuery();
_callback = nullptr;
}

~NavMeshQuery()
{

}

!NavMeshQuery()
{
dtFreeNavMeshQuery(_query);
}

float GetPolyHeight(dtPolyRef polyRef, array<float>^ pos)
{
float ret;
pin_ptr<float> posPointer = &pos[0];
if (_query->getPolyHeight(polyRef, posPointer, &ret) != DT_SUCCESS)
return 0;
return ret;
}

DetourStatus closestPointOnPolyBoundary(dtPolyRef polyRef, array<float>^ pos, [Out] array<float>^% closestPoint)
{
pin_ptr<float> posPointer = &pos[0];
float* pt = new float[3];

dtStatus status = _query->closestPointOnPolyBoundary(polyRef, posPointer, pt);
closestPoint = gcnew array<float>(3);
for (int i = 0; i < 3; i++)
closestPoint[i] = pt[i];

return (DetourStatus)status;
}

DetourStatus QueryPolygons(array<float>^ center, array<float>^ extents, QueryFilter^ filter, [Out] array<dtPolyRef>^% foundPolys)
{
pin_ptr<float> centerPointer = &center[0];
pin_ptr<float> extentsPointer = &extents[0];
dtPolyRef* polys = new dtPolyRef[8192];
int polyCount;

dtStatus status = _query->queryPolygons(centerPointer, extentsPointer, filter->GetNativeObject(), polys, &polyCount, 8192);
if (!dtStatusSucceed(status))
goto exit;

foundPolys = gcnew array<dtPolyRef>(polyCount);
for (int i = 0; i < polyCount; i++)
foundPolys[i] = polys[i];
goto exit;

exit:
delete[] polys;
return (DetourStatus)status;
}

DetourStatus Initialize(NavMesh^ mesh, const int maxNodes)
{
_managedMesh = mesh;
_mesh = mesh->GetNativeObject();
return (DetourStatus)_query->init(mesh->GetNativeObject(), maxNodes);
}

DetourStatus FindNearestPolygon(array<float>^ center, array<float>^ extents, QueryFilter^ filter, [Out] array<float>^% nearestPoint)
{
pin_ptr<float> centerPointer = &center[0];
pin_ptr<float> extentsPointer = &extents[0];

nearestPoint = gcnew array<float>(3);
pin_ptr<float> nearestPointer = &nearestPoint[0];

dtPolyRef discard;
return (DetourStatus)_query->findNearestPoly(centerPointer, extentsPointer, filter->GetNativeObject(), &discard, nearestPointer);
}

dtPolyRef FindNearestPolygon(array<float>^ center, array<float>^ extents, QueryFilter^ filter)
{
pin_ptr<float> centerPointer = &center[0];
pin_ptr<float> extentsPointer = &extents[0];
dtPolyRef ret;
dtStatus result = _query->findNearestPoly(centerPointer, extentsPointer, filter->GetNativeObject(), &ret, 0);
if (result != DT_SUCCESS)
{
return 0;
}
return ret;
}

DetourStatus FindStraightPath(array<float>^ start, array<float>^ end, array<dtPolyRef>^ pathCorridor, [Out] array<float>^% straightPath, [Out] array<StraightPathFlag>^% straightPathFlags, [Out] array<dtPolyRef>^% straightPathRefs)
{
pin_ptr<float> startPointer = &start[0];
pin_ptr<float> endPointer = &end[0];
pin_ptr<dtPolyRef> pathPointer = &pathCorridor[0];

int resultHopCount;
float* nativeStraightPath = new float[1024*3];
unsigned char* pathFlags = new unsigned char[1024];
dtPolyRef* pathRefs = new dtPolyRef[1024];

dtStatus status = _query->findStraightPath(startPointer, endPointer, pathPointer, pathCorridor->Length, nativeStraightPath, pathFlags, pathRefs, &resultHopCount, 1024);
if (!dtStatusSucceed(status))
{
delete[] nativeStraightPath;
delete[] pathFlags;
delete[] pathRefs;

straightPath = nullptr;
straightPathFlags = nullptr;
straightPathRefs = nullptr;
return (DetourStatus)status;
}

straightPath = gcnew array<float>(resultHopCount * 3);
for (int i = 0; i < resultHopCount; i++)
{
straightPath[(i*3)+0] = nativeStraightPath[(i*3)+0];
straightPath[(i*3)+1] = nativeStraightPath[(i*3)+1];
straightPath[(i*3)+2] = nativeStraightPath[(i*3)+2];
}
delete[] nativeStraightPath;
straightPathFlags = gcnew array<StraightPathFlag>(resultHopCount);
for (int i = 0; i < resultHopCount; i++)
straightPathFlags[i] = (StraightPathFlag)pathFlags[i];
delete[] pathFlags;
straightPathRefs = gcnew array<dtPolyRef>(resultHopCount);
for (int i = 0; i < resultHopCount; i++)
straightPathRefs[i] = pathRefs[i];
delete[] pathRefs;
return (DetourStatus)status;
}

DetourStatus FindLocalPath(dtPolyRef startRef, dtPolyRef endRef, array<float>^ startPos, array<float>^ endPos, QueryFilter^ filter, [Out] array<dtPolyRef>^% polyRefHops)
{
pin_ptr<float> startPosPointer = &startPos[0];
pin_ptr<float> endPosPointer = &endPos[0];
int hops;
dtPolyRef* hopBuffer = new dtPolyRef[44];
dtStatus status = _query->findPath(_callback, startRef, endRef, startPosPointer, endPosPointer, filter->GetNativeObject(), hopBuffer, &hops, 44);
if (dtStatusSucceed(status))
{
polyRefHops = gcnew array<dtPolyRef>(hops);
for (int i = 0; i < hops; i++)
polyRefHops[i] = hopBuffer[i];
delete[] hopBuffer;
return (DetourStatus)status;
}

delete[] hopBuffer;
return (DetourStatus)status;
}

DetourStatus FindPath(dtPolyRef startRef, dtPolyRef endRef, array<float>^ startPos, array<float>^ endPos, QueryFilter^ filter, [Out] array<dtPolyRef>^% polyRefHops)
{
pin_ptr<float> startPosPointer = &startPos[0];
pin_ptr<float> endPosPointer = &endPos[0];
int hops;
dtPolyRef* hopBuffer = new dtPolyRef[4096];
dtStatus status = _query->findPath(_callback, startRef, endRef, startPosPointer, endPosPointer, filter->GetNativeObject(), hopBuffer, &hops, 4096);
if (dtStatusSucceed(status))
{
polyRefHops = gcnew array<dtPolyRef>(hops);
for (int i = 0; i < hops; i++)
polyRefHops[i] = hopBuffer[i];
delete[] hopBuffer;
return (DetourStatus)status;
}

delete[] hopBuffer;
return (DetourStatus)status;
}

int MarkAreaInCircle(dtPolyRef startRef, array<float>^ center, float radius, QueryFilter^ filter, PolyArea areaType)
{
pin_ptr<float> centerPointer = &center[0];
dtPolyRef* results = new dtPolyRef[8192];
int resultCount;

if (!dtStatusSucceed(_query->findPolysAroundCircle(startRef, centerPointer, radius, filter->GetNativeObject(), results, 0, 0, &resultCount, 8192)))
{
delete[] results;
return 0;
}

for (int i = 0; i < resultCount; i++)
_mesh->setPolyArea(results[i], (unsigned char)areaType);
delete[] results;
return resultCount;
}

int MarkAreaInShape(dtPolyRef startRef, array<float>^ verts, QueryFilter^ filter, PolyArea areaType)
{
dtPolyRef* refs = new dtPolyRef[8192];
int resultCount;
pin_ptr<float> vertsPointer = &verts[0];

if (!dtStatusSucceed(_query->findPolysAroundShape(startRef, vertsPointer, verts->Length / 3, filter->GetNativeObject(), refs, 0, 0, &resultCount, 8192)))
{
delete[] refs;
return 0;
}

for (int i = 0; i < resultCount; i++)
_mesh->setPolyArea(refs[i], (unsigned char)areaType);
delete[] refs;
return resultCount;
}
};

public ref class NavMeshCreateParams
{
private:
dtNavMeshCreateParams* _params;

public:

NavMeshCreateParams()
{
_params = new dtNavMeshCreateParams;
memset(_params, 0, sizeof(dtNavMeshCreateParams));
}

dtNavMeshCreateParams* GetNativeObject()
{
return _params;
}

void Delete()
{
delete _params;
}

property int VerticeCount
{
int get()
{
return _params->vertCount;
}

void set(int value)
{
_params->vertCount = value;
}
}

property int PolygonCount
{
int get()
{
return _params->polyCount;
}

void set(int value)
{
_params->polyCount = value;
}
}

property int VerticesPerPolygon
{
int get()
{
return _params->nvp;
}

void set(int value)
{
_params->nvp = value;
}
}

property int DetailTriangleCount
{
int get()
{
return _params->detailTriCount;
}

void set(int value)
{
_params->detailTriCount = value;
}
}

property int DetailVerticeCount
{
int get()
{
return _params->detailVertsCount;
}

void set(int value)
{
_params->detailVertsCount = value;
}
}

property int OffMeshConCount
{
int get()
{
return _params->offMeshConCount;
}

void set(int value)
{
_params->offMeshConCount = value;
}
}

property int UserId
{
int get()
{
return _params->userId;
}

void set(int value)
{
_params->userId = value;
}
}

property int TileX
{
int get()
{
return _params->tileX;
}

void set(int value)
{
_params->tileX = value;
}
}

property int TileY
{
int get()
{
return _params->tileY;
}

void set(int value)
{
_params->tileY = value;
}
}

property float WalkableHeight
{
float get()
{
return _params->walkableHeight;
}

void set(float value)
{
_params->walkableHeight = value;
}
}

property float WalkableRadius
{
float get()
{
return _params->walkableRadius;
}

void set(float value)
{
_params->walkableRadius = value;
}
}

property float WalkableClimb
{
float get()
{
return _params->walkableClimb;
}

void set(float value)
{
_params->walkableClimb = value;
}
}

property float CellSize
{
float get()
{
return _params->cs;
}

void set(float value)
{
_params->cs = value;
}
}

property float CellHeight
{
float get()
{
return _params->ch;
}

void set(float value)
{
_params->ch = value;
}
}

property array<float>^ BMax
{
array<float>^ get()
{
array<float>^ ret = gcnew array<float>(3);
ret[0] = _params->bmax[0];
ret[1] = _params->bmax[1];
ret[2] = _params->bmax[2];
return ret;
}

void set(array<float>^ value)
{
_params->bmax[0] = value[0];
_params->bmax[1] = value[1];
_params->bmax[2] = value[2];
}
}

property array<float>^ BMin
{
array<float>^ get()
{
array<float>^ ret = gcnew array<float>(3);
ret[0] = _params->bmin[0];
ret[1] = _params->bmin[1];
ret[2] = _params->bmin[2];
return ret;
}

void set(array<float>^ value)
{
_params->bmin[0] = value[0];
_params->bmin[1] = value[1];
_params->bmin[2] = value[2];
}
}

property array<unsigned short>^ Vertices
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(_params->vertCount);
for (int i = 0; i < _params->vertCount; i++)
ret[i] = _params->verts[i];
return ret;
}

void set(array<unsigned short>^ value)
{
VerticeCount = value->Length;
if (_params->verts)
delete[] _params->verts;
auto verts = new unsigned short[value->Length];
for (int i = 0; i < value->Length; i++)
verts[i] = value[i];
_params->verts = verts;
}
}

property array<unsigned short>^ Polygons
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(_params->polyCount);
for (int i = 0; i < _params->polyCount; i++)
ret[i] = _params->polys[i];
return ret;
}

void set(array<unsigned short>^ value)
{
PolygonCount = value->Length;
if (_params->polys)
delete[] _params->polys;
auto polys = new unsigned short[value->Length];
for (int i = 0; i < value->Length; i++)
polys[i] = value[i];
_params->polys = polys;
}
}

property array<unsigned short>^ PolygonFlags
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(_params->polyCount);
for (int i = 0; i < _params->polyCount; i++)
ret[i] = _params->polyFlags[i];
return ret;
}

void set(array<unsigned short>^ value)
{
PolygonCount = value->Length;
if (_params->polyFlags)
delete[] _params->polyFlags;
auto polyFlags = new unsigned short[value->Length];
for (int i = 0; i < value->Length; i++)
polyFlags[i] = value[i];
_params->polyFlags = polyFlags;
}
}

property array<unsigned char>^ PolygonAreas
{
array<unsigned char>^ get()
{
auto ret = gcnew array<unsigned char>(_params->polyCount);
for (int i = 0; i < _params->polyCount; i++)
ret[i] = _params->polyAreas[i];
return ret;
}

void set(array<unsigned char>^ value)
{
PolygonCount = value->Length;
if (_params->polyAreas)
delete[] _params->polyAreas;
auto polyAreas = new unsigned char[value->Length];
for (int i = 0; i < value->Length; i++)
polyAreas[i] = value[i];
_params->polyAreas = polyAreas;
}
}

property array<unsigned int>^ DetailMeshes
{
array<unsigned int>^ get()
{
auto ret = gcnew array<unsigned int>(_params->polyCount * 4);
for (int i = 0; i < (_params->polyCount*4); i++)
ret[i] = _params->detailMeshes[i];
return ret;
}

void set(array<unsigned int>^ value)
{
PolygonCount = value->Length / 4;
if (_params->detailMeshes)
delete[] _params->detailMeshes;
auto detailMeshes = new unsigned int[value->Length];
for (int i = 0; i < value->Length; i++)
detailMeshes[i] = value[i];
_params->detailMeshes = detailMeshes;
}
}

property array<float>^ DetailVertices
{
array<float>^ get()
{
auto ret = gcnew array<float>(_params->detailVertsCount * 3);
for (int i = 0; i < _params->detailVertsCount*3; i++)
ret[i] = _params->detailVerts[i];
return ret;
}

void set(array<float>^ value)
{
DetailVerticeCount = value->Length / 3;
if (_params->detailVerts)
delete[] _params->detailVerts;
auto detailVerts = new float[value->Length];
for (int i = 0; i < value->Length; i++)
detailVerts[i] = value[i];
_params->detailVerts = detailVerts;
}
}

property array<unsigned char>^ DetailTriangles
{
array<unsigned char>^ get()
{
auto ret = gcnew array<unsigned char>(_params->detailTriCount * 3);
for (int i = 0; i < _params->detailTriCount*3; i++)
ret[i] = _params->detailTris[i];
return ret;
}

void set(array<unsigned char>^ value)
{
DetailTriangleCount = value->Length / 3;
if (_params->detailTris)
delete[] _params->detailTris;
auto detailTris = new unsigned char[value->Length];
for (int i = 0; i < value->Length; i++)
detailTris[i] = value[i];
_params->detailTris = detailTris;
}
}

property array<float>^ OffMeshConVertices
{
array<float>^ get()
{
auto ret = gcnew array<float>(_params->offMeshConCount * 3 * 2);
for (int i = 0; i < _params->offMeshConCount*6; i++)
ret[i] = _params->offMeshConVerts[i];
return ret;
}

void set(array<float>^ value)
{
OffMeshConCount = value->Length / 6;
if (_params->offMeshConVerts)
delete[] _params->offMeshConVerts;
auto offMeshConVerts = new float[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConVerts[i] = value[i];
_params->offMeshConVerts = offMeshConVerts;
}
}

property array<float>^ OffMeshConRadii
{
array<float>^ get()
{
auto ret = gcnew array<float>(_params->offMeshConCount);
for (int i = 0; i < _params->offMeshConCount; i++)
ret[i] = _params->offMeshConRad[i];
return ret;
}

void set(array<float>^ value)
{
OffMeshConCount = value->Length;
if (_params->offMeshConRad)
delete[] _params->offMeshConRad;
auto offMeshConRad = new float[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConRad[i] = value[i];
_params->offMeshConRad = offMeshConRad;
}
}

property array<unsigned short>^ OffMeshConFlags
{
array<unsigned short>^ get()
{
auto ret = gcnew array<unsigned short>(_params->offMeshConCount);
for (int i = 0; i < _params->offMeshConCount; i++)
ret[i] = _params->offMeshConFlags[i];
return ret;
}

void set(array<unsigned short>^ value)
{
OffMeshConCount = value->Length;
if (_params->offMeshConFlags)
delete[] _params->offMeshConFlags;
auto offMeshConFlags = new unsigned short[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConFlags[i] = value[i];
_params->offMeshConFlags = offMeshConFlags;
}
}

property array<unsigned char>^ OffMeshConAreas
{
array<unsigned char>^ get()
{
auto ret = gcnew array<unsigned char>(_params->offMeshConCount);
for (int i = 0; i < _params->offMeshConCount; i++)
ret[i] = _params->offMeshConAreas[i];
return ret;
}

void set(array<unsigned char>^ value)
{
OffMeshConCount = value->Length;
if (_params->offMeshConAreas)
delete[] _params->offMeshConAreas;
auto offMeshConAreas = new unsigned char[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConAreas[i] = value[i];
_params->offMeshConAreas = offMeshConAreas;
}
}

property array<unsigned char>^ OffMeshConDirection
{
array<unsigned char>^ get()
{
auto ret = gcnew array<unsigned char>(_params->offMeshConCount);
for (int i = 0; i < _params->offMeshConCount; i++)
ret[i] = _params->offMeshConDir[i];
return ret;
}

void set(array<unsigned char>^ value)
{
OffMeshConCount = value->Length;
if (_params->offMeshConDir)
delete[] _params->offMeshConDir;
auto offMeshConDir = new unsigned char[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConDir[i] = value[i];
_params->offMeshConDir = offMeshConDir;
}
}

property array<unsigned int>^ OffMeshConUserId
{
array<unsigned int>^ get()
{
auto ret = gcnew array<unsigned int>(_params->offMeshConCount);
for (int i = 0; i < _params->offMeshConCount; i++)
ret[i] = _params->offMeshConUserID[i];
return ret;
}

void set(array<unsigned int>^ value)
{
OffMeshConCount = value->Length;
if (_params->offMeshConUserID)
delete[] _params->offMeshConUserID;
auto offMeshConUserID = new unsigned int[value->Length];
for (int i = 0; i < value->Length; i++)
offMeshConUserID[i] = value[i];
_params->offMeshConUserID = offMeshConUserID;
}
}

};

public ref class Detour
{
public:
static bool CreateNavMeshData([Out] array<unsigned char>^% data, PolyMesh^ pm, PolyMeshDetail^ dm, int tileX, int tileY, array<float>^ bmin, array<float>^ bmax, float walkableHeight, float walkableRadius, float walkableClimb, float cs, float ch, bool buildBvTree, array<OffMeshConnection^>^ offMeshCons);
};

}
