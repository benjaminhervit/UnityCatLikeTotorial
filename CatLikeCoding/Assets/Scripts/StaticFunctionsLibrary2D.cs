using UnityEngine;

public static class StaticFunctionsLibrary2D
{
    public delegate float Function(float x);

    public static Function GetFunction(int index)
    {
        if (index == 0)
        {
            return Linear;
        }
        else if (index == 1)
        {
            return Cubed;
        }
        else if(index == 2)
        {
            return Parabola;
        }
        else
        {
            return Linear;
        }
    }

    private static float Linear(float x)
    {
        return x;
    }

    private static float Cubed(float x)
    {
        return x * x * x;
    }

    private static float Parabola(float x)
    {
        return x * x;
    }
}
