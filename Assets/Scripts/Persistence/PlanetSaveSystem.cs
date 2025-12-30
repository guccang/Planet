using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class PlanetSaveSystem
{
    // 获取 Assets/Resources/PlanetSaves 的物理路径
    // 注意：Path.Combine 会处理不同系统的斜杠问题
    public static string SaveFolder = Path.Combine(Application.dataPath, "Resources", "PlanetSaves");

    public static void SavePlanet(Planet planet, string saveName)
    {
        // 1. 确保目录存在
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }

        // 2. 序列化数据
        PlanetData data = new PlanetData(planet, saveName);
        string json = JsonUtility.ToJson(data, true);

        // 3. 写入文件
        string filePath = Path.Combine(SaveFolder, saveName + ".json");
        File.WriteAllText(filePath, json);

        // 4. 重要：在编辑器中保存后刷新，否则 Resources.Load 可能找不到新文件
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log($"Planet saved to: {filePath}");
    }

    public static PlanetData LoadPlanetData(string saveName)
    {
        // 尝试从 Resources 加载（这样在打包后也能读取）
        // 注意：Resources.Load 不需要文件后缀名
        TextAsset targetJson = Resources.Load<TextAsset>("PlanetSaves/" + saveName);

        if (targetJson != null)
        {
            return JsonUtility.FromJson<PlanetData>(targetJson.text);
        }
        else
        {
            Debug.LogError("Save file not found in Resources: " + saveName);
            return null;
        }
    }

    public static List<string> GetSavedPlanetNames()
    {
        // 如果目录不存在，返回空列表
        if (!Directory.Exists(SaveFolder)) return new List<string>();

        DirectoryInfo info = new DirectoryInfo(SaveFolder);
        // 获取所有 .json 文件
        FileInfo[] files = info.GetFiles("*.json");

        return files.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList();
    }
}