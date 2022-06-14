using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{

	[SerializeField]
	TextMeshProUGUI display;

	public enum DisplayMode { FPS, MS }
	[SerializeField]
	DisplayMode displayMode = DisplayMode.FPS;

	[SerializeField, Range(0.1f, 2f)]
	float sampleDuration = 1f;

	int frames;
	float duration, bestDuration = float.MaxValue, worstDuration;

	void Update()
	{
		float frameDuration = Time.unscaledDeltaTime;

		frames += 1;
		duration += frameDuration;

		if (frameDuration < bestDuration)
		{
			bestDuration = frameDuration;
		}
		if (frameDuration > worstDuration)
		{
			worstDuration = frameDuration;
		}

		if (duration >= sampleDuration)
		{
			if (displayMode == DisplayMode.FPS)
			{
				display.SetText(
					"FPS\nB: {0:0}\n A: {1:0}\nW: {2:0}",
					1f / bestDuration,
					frames / duration,
					1f / worstDuration
				);
			}
			else
			{
				display.SetText(
					"MS\nB: {0:1}\nA: {1:1}\nW: {2:1}",
					1000f * bestDuration,
					1000f * duration / frames,
					1000f * worstDuration
				);
			}
			frames = 0;
			duration = 0f;
			bestDuration = float.MaxValue;
			worstDuration = 0f;
		}

	}
}