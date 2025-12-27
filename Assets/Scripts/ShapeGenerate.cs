using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerate 
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;

    public ShapeGenerate(ShapeSettings settings){
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for(int i=0; i< settings.noiseLayers.Length; i++){
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere){
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0){
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(settings.noiseLayers[0].enable){
                elevation = firstLayerValue;
            }
        }

        for (int i=1; i<noiseFilters.Length;i++){
            if(settings.noiseLayers[i].enable){
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask)?firstLayerValue:1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        return pointOnUnitSphere * settings.planetRadius * (1+elevation);
    }
}
