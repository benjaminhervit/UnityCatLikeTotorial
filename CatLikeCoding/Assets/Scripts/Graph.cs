using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //SERIALIZED FIELDS
    [SerializeField]
    Transform pointPrefab;

    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    [SerializeField]
    bool addRandom;

    [SerializeField, Range(0,1)]
    float randomness;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary3D.FunctionName function;

    //INTERNAL VARS
    Transform[] points;

    float duration;

    bool transitioning;

    FunctionLibrary3D.FunctionName transitionFunction;


    //FUNCTIONS
    private void Awake()
    {
        points = new Transform[resolution * resolution];

        float step = 2f / resolution;
        var scale = Vector3.one * step;
        Vector3 position = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    void Update()
    {
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();

            if (transitioning)
            {
                UpdateFunctionTransition();
            }
            else
            {
                UpdateFunction();
            }
        }
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary3D.GetNextFunctionName(function) :
            FunctionLibrary3D.GetRandomFunctionNameOtherThan(function);
    }

    void UpdateFunction()
    {
        FunctionLibrary3D.Function f = FunctionLibrary3D.GetFunction(function);

        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time, randomness, addRandom);
        }
    }

    void UpdateFunctionTransition()
    {
        FunctionLibrary3D.Function
            from = FunctionLibrary3D.GetFunction(transitionFunction),
            to = FunctionLibrary3D.GetFunction(function);
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary3D.Morph(u, v, time, randomness, addRandom, from, to, progress);
        }
    }
}
