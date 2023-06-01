#ifndef PUDDLE_UTIL_INCLUDED
#define PUDDLE_UTIL_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

struct Point {
    uint xy;
    float size;
    uint r1g1;
    uint b1r2;
    uint g2b2;

    float2 Pos() {
        return float2(
            f16tof32(xy),
            f16tof32(xy >> 16)
        );
    }

    float3 Color1() {
        return float3(
            f16tof32(r1g1),
            f16tof32(r1g1 >> 16),
            f16tof32(b1r2)
        );
    }

    float3 Color2() {
        return float3(
            f16tof32(b1r2 >> 16),
            f16tof32(g2b2),
            f16tof32(g2b2 >> 16)
        );
    }
};

StructuredBuffer<Point> Points;

uint PointCount;
float4 ScaleOffset;

void GetPointColor_float(in float2 UV, out float4 Out) {
    #ifndef SHADERGRAPH_PREVIEW
    const float2 pos = UV * ScaleOffset.xy + ScaleOffset.zw;
    Point closest = ZERO_INITIALIZE(Point, closest);
    float minDist2 = FLT_MAX;
    UNITY_LOOP
    for (uint i = 0; i < PointCount; i++) {
        Point p = Points.Load(i);
        const float2 relPos = p.Pos() - pos;
        const float dist2 = dot(relPos, relPos);
        if (dist2 < minDist2) {
            closest = p;
            minDist2 = dist2;
        }
    }
    const float normDist = sqrt(minDist2) / closest.size;
    Out = float4(lerp(closest.Color1(), closest.Color2(), normDist), normDist <= 1);
    #else
    Out = float4(UV, 0, 1);
    #endif
}

void GetPointColor_half(in half2 UV, out half4 Out) {
    #ifndef SHADERGRAPH_PREVIEW
    const half2 pos = UV * ScaleOffset.xy + ScaleOffset.zw;
    Point closest = ZERO_INITIALIZE(Point, closest);
    float minDist2 = FLT_MAX;
    UNITY_LOOP
    for (uint i = 0; i < PointCount; i++) {
        Point p = Points.Load(i);
        const half2 relPos = p.Pos() - pos;
        const half dist2 = dot(relPos, relPos);
        if (dist2 < minDist2) {
            closest = p;
            minDist2 = dist2;
        }
    }
    const float normDist = sqrt(minDist2) / closest.size;
    Out = half4(lerp(closest.Color1(), closest.Color2(), normDist), normDist <= 1);
    #else
    Out = half4(UV, 0, 1);
    #endif
}

#endif