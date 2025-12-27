using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    Noise noise;
    NoiseSettings.RidgidNoiseSettings settings;

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
    {
        noise = new Noise();
        this.settings = settings;
    }

    public float Evaluate(Vector3 point){
        // noise.Evaluate  [-1,1]
        // [0,1]
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;
        for(int i=0;i<settings.numLayers;i++){
            float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultipliter);

            noiseValue +=  v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        noiseValue = Mathf.Max(0,noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
