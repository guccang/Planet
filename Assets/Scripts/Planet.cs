using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 10;

    [SerializeField,HideInInspector]
    public MeshFilter[] meshFilters;

    TerrainFace[] terrainFaces;
    Vector3[] directors = {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};

    public void OnValidate(){

        Initialize();
        GenerateMesh();

    }

    public void Initialize(){

        if(meshFilters == null || meshFilters.Length==0)
        {
            meshFilters = new MeshFilter[6];
        }
        if(terrainFaces == null){
            terrainFaces = new TerrainFace[6];
        }

        for(int i=0;i<6;i++){

            if(meshFilters[i] == null){

                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh,resolution,directors[i]);
        }
    }

    public void GenerateMesh(){
        foreach(TerrainFace face in terrainFaces){
            face.ConstructMesh();
        }
    }

}
