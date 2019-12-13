using UnityEngine;

public class PerlinNoiseScroller
{
    private float Frequency;
    private float Amplitude;

    private Vector3 noiseOffset;
    private Vector3 noise;

    public Vector3 Noise => noise;

    public PerlinNoiseScroller (float frequency, float amplitude)
    {
        Frequency = frequency;
        Amplitude = amplitude;

        float rand = 32f;

        noiseOffset.x = Random.Range(0f,rand);
        noiseOffset.y = Random.Range(0f,rand);
        noiseOffset.z = Random.Range(0f,rand);
    }

    public void UpdateNoise()
    {
        float scrollOffset = Frequency * Time.deltaTime;

        noiseOffset.x += scrollOffset;
        noiseOffset.y += scrollOffset;
        noiseOffset.z += scrollOffset;

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 1);
        noise.y = Mathf.PerlinNoise(noiseOffset.x, 2);
        noise.z = Mathf.PerlinNoise(noiseOffset.x, 3);

        noise -= Vector3.one * 0.5f;
        noise *= Amplitude;
    }
}
