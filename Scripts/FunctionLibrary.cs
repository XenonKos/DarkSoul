using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionLibrary
{
    public delegate float Function(float x, float z, float t);
    public enum FunctionName { Wave, MultiWave, Ripple }
    static Function[] functions = { Wave, MultiWave, Ripple };
    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
    public static float Wave(float x, float z, float t)
    {
        return Mathf.Sin(Mathf.PI * (x + t));
    }
    public static float MultiWave(float x, float z, float t)
    {
        float y = Mathf.Sin(Mathf.PI * (x + 0.5f * t));
        y += 0.5f * Mathf.Sin(2f * Mathf.PI * (x + t));
        return y;
    }
    public static float Ripple(float x,  float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(Mathf.PI * (4f * d - t));
        return y / (1f + 10f * d);
    }
}
