using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary3D
{
    /*
     All of functions only affects one axis (the axis input) but is ideally meant to affect the Y axis. 
     */

    //CONTROL ALL FUNCTIONS, ETC
    public delegate Vector3 Function(float u, float v, float t, float rand, bool addRandom);

    public enum FunctionName      { Wave, MultiWave, MultiWaveInverse, MorphingWave, Ripple, RippleIn, Onion, Sphere, TwistedSphere, Torus, None }
    static Function[] functions = { Wave, MultiWave, MultiWaveInverse, MorphingWave, Ripple, RippleIn, Onion, Sphere, TwistedSphere, Torus, None };
   
    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }

    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        if ((int)name < functions.Length - 2) // -2 because the last is None
        {
            return name + 1;
        }
        else
        {
            return 0;
        }
    }

    //TRANSITIONS
    public static Vector3 Morph(float u, float v, float t, float rand, bool addRandom, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t, rand, addRandom), to(u, v, t, rand, addRandom), SmoothStep(0f, 1f, progress));
    }

    //FUNCTIONS

    public static Vector3 Wave(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 1;
        }

        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + (v * +rand) + t ));
        p.z = v;
        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 0.5f;
        }

        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + t));
        p.y += rand * Sin(2f * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= (1f / 2.5f);
        p.z = v;
        return p;
    }

    public static Vector3 MultiWaveInverse(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 0.5f;
        }

        Vector3 p;   
        p.x = u;
        p.y = Sin(PI * (v + t));
        p.y *= rand * Sin(2f * PI * (u + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= (1f / 2.5f);
        p.z = v;
        return p;
    }

    public static Vector3 MorphingWave(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 0.5f;
        }

        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += rand * Sin(2f * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f / 2.5f;
        p.z = v;
        return p;
    }

    public static Vector3 Ripple(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 1f;
        }

        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= (rand + 10f * d);
        p.z = v;
        return p;
    }

    public static Vector3 RippleIn(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 1f;
        }

        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d + t));
        p.y /= (rand + 10f * d);
        p.z = v;
        return p;
    }

    public static Vector3 Onion(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 0.5f;
        }

        float r = 0.5f + rand * Sin(PI * t);
        float s = r * Cos(rand * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * v;
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t, float rand, bool addRandom)
    {
        if (!addRandom)
        {
            rand = 0.5f;
        }

        float r = 0.5f + rand * Sin(PI * t);
        float s = r * Cos(rand * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(rand * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 TwistedSphere(float u, float v, float t, float rand, bool addRandom)
    {
        float r1 = 0.9f;
        float r2 = 0.1f;

        if(addRandom)
        {
            r1 = rand;
            r2 = 1f - rand;
        }

        float r = r1 + r2 * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Torus(float u, float v, float t, float rand, bool addRandom)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));

        if (addRandom)
        {
            r1 = rand + 0.1f * Sin(PI * (6f * u + 0.5f * t));
            r2 = (1f - rand) + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t)); ;
        }

        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 None(float u, float v, float t, float rand, bool addRandom)
    {
        Vector3 p;
        p.x = 0f;
        p.y = 0f;
        p.z = 0f;
        return p;
    }

}
