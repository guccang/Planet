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
    [HideInInspector]
    public string selectedPlanetName;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    ShapeGenerate shapeGenerate = new ShapeGenerate();
    ColourGenerate colourGenerate = new ColourGenerate();

    TerrainFace[] terrainFaces;
    Vector3[] directors = {Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back};

    public void OnValidate(){
        GeneratePlanet();
    }

    public void Initialize(){

        shapeGenerate.UpdateSettings(shapeSettings);
        colourGenerate.UpdateSettings(colourSettings);

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

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].gameObject.GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

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
        if(autoUpdate){
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated(){
        if(autoUpdate){
            Initialize();
            GenerateColour();
        }
    }

    public void GenerateMesh(){
            for(int i=0;i<6;i++){
                if(meshFilters[i].gameObject.activeSelf){
                    terrainFaces[i].ConstructMesh();
                }
            }
            colourGenerate.UpdateElevation(shapeGenerate.elevationMinMax);
    }

    public void GenerateColour(){
        colourGenerate.UpdateColors();
        for(int i=0;i<6;i++){
            if(meshFilters[i].gameObject.activeSelf){
                terrainFaces[i].UpdateUVs(colourGenerate);
            }
        }
    }

    // === 新增：加载功能 ===
    public void LoadConfig(string saveName)
    {
        PlanetData data = PlanetSaveSystem.LoadPlanetData(saveName);
        if (data == null) return;

        // 1. 克隆现有的 Settings，避免修改原始资源文件
        // 这一点非常重要！必须使用 Instantiate 创建运行时副本
        ShapeSettings newShapeSettings = Instantiate(shapeSettings);
        ColourSettings newColorSettings = Instantiate(colourSettings);

        // 2. 使用 JsonUtility 的 "Overwrite" 功能将存档数据覆盖到新副本上
        JsonUtility.FromJsonOverwrite(data.shapeSettingsJson, newShapeSettings);
        JsonUtility.FromJsonOverwrite(data.colorSettingsJson, newColorSettings);

        // 3. 重新赋值给当前星球
        this.shapeSettings = newShapeSettings;
        this.colourSettings = newColorSettings;

        // 4. 刷新星球生成
        GeneratePlanet(); 
    }

    // === 新增：保存功能 ===
    public void SaveConfig(string saveName)
    {
        PlanetSaveSystem.SavePlanet(this, saveName);
    }

}
