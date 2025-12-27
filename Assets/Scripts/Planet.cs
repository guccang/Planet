using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask {ALL,Top,Bottom,Left,Right,Front,Back};
    public FaceRenderMask faceRenderMask;

    [SerializeField,HideInInspector]
    public MeshFilter[] meshFilters;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    ShapeGenerate shapeGenerate;

    TerrainFace[] terrainFaces;
    Vector3[] directors = {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};

    public void OnValidate(){
        GeneratePlanet();
    }

    public void Initialize(){

        shapeGenerate = new ShapeGenerate(shapeSettings);

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

            terrainFaces[i] = new TerrainFace(shapeGenerate,meshFilters[i].sharedMesh,resolution,directors[i]);
            bool faceRender = faceRenderMask == FaceRenderMask.ALL || (int)(faceRenderMask-1) == i;
            meshFilters[i].gameObject.SetActive(faceRender);
        }
    }

    public void GeneratePlanet(){
        Initialize();
        GenerateMesh();
        GenerateColour();
    }

    public void OnShapeSettingsUpdated(){
        Initialize();
        GenerateMesh();
    }

    public void OnColourSettingsUpdated(){
        Initialize();
        GenerateColour();
    }

    public void GenerateMesh(){
        if(autoUpdate){
        for(int i=0;i<6;i++){
            if(meshFilters[i].gameObject.activeSelf){
                terrainFaces[i].ConstructMesh();
            }
        }
        }
    }

    public void GenerateColour(){
        if(autoUpdate){
        foreach(MeshFilter m in meshFilters){
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
        }
    }

}
