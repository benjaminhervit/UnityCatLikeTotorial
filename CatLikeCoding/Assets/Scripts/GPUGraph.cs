using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    //SERIALIZED FIELDS
    [SerializeField, Range(10, 200)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary3D.FunctionName function;

    public enum TransitionMode { Cycle, Random }
    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 0f;

    [SerializeField]
    bool addRandom;

    [SerializeField, Range(0, 1)]
    float randomness;

    //INTERNAL VARS

    float duration;

    bool transitioning;

    FunctionLibrary3D.FunctionName transitionFunction;


    //FUNCTIONS
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
        }
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary3D.GetNextFunctionName(function) :
            FunctionLibrary3D.GetRandomFunctionNameOtherThan(function);
    }
}
