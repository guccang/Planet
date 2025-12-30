using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;
    string saveName;
    int selectedIndex;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope()){
            base.OnInspectorGUI();
            if(check.changed){
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet")){
            planet.GeneratePlanet();
        }

        DrawSettingEditor(planet.shapeSettings,planet.OnShapeSettingsUpdated,ref planet.shapeSettingsFoldout,ref shapeEditor);
        DrawSettingEditor(planet.colourSettings,planet.OnColourSettingsUpdated,ref planet.colourSettingsFoldout,ref colourEditor);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Save / Load System", EditorStyles.boldLabel);

        saveName = EditorGUILayout.TextField("Config Name", saveName);


        if (GUILayout.Button("Save Planet"))
        {
            planet.SaveConfig(saveName);
        }

        EditorGUILayout.BeginHorizontal();
        // 2. 获取存档列表
        EditorGUILayout.Space();
        List<string> options = PlanetSaveSystem.GetSavedPlanetNames();

        if (options.Count > 0)
        {
            // 3. 绘制下拉选择框
            selectedIndex = EditorGUILayout.Popup("Select Planet", selectedIndex, options.ToArray());
            planet.selectedPlanetName = options[selectedIndex];

            if (GUILayout.Button("Load Selected Planet"))
            {
                planet.LoadConfig(planet.selectedPlanetName);
                saveName = planet.selectedPlanetName;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No saved planets found.", MessageType.Info);
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.EndHorizontal();
    }

    void DrawSettingEditor(Object settings,System.Action onSettingsUpdated,ref bool foldout,ref Editor editor){
        if(settings != null){
        foldout = EditorGUILayout.InspectorTitlebar(foldout,settings);
        using (var check = new EditorGUI.ChangeCheckScope()){
            if(foldout){
                CreateCachedEditor(settings,null,ref editor);
                editor.OnInspectorGUI();
                if(check.changed){
                    if(onSettingsUpdated != null){
                        onSettingsUpdated();
                    }
                }
            }
        }
        }
    }

    void OnEnable(){
        planet = (Planet)target;
    }
}
