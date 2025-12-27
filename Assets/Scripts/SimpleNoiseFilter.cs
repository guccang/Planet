using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    Noise noise;
    NoiseSettings.SimpleNoiseSettings settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
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
        for(int i=0;i<settings.numLayers;i++){
            float v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue +=  (v+ 1) * 0.5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        noiseValue = Mathf.Max(0,noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
