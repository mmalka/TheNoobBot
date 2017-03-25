#ifndef NAVMESHQUERYCALLBACKH
#define NAVMESHQUERYCALLBACKH

using namespace System;

namespace DetourLayer
{

public interface class NavMeshQueryCallback
{
void PathfinderUpdate(array<float>^ best);
void Log(String^ text);
};

}

#endif
