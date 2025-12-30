using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class PlanetSaveSystem
{
    // 存档文件夹路径
    public static string SaveFolder => Path.Combine(Application.persistentDataPath, "PlanetSaves");

    public static void SavePlanet(Planet planet, string saveName)
    {
        // 1. 确保文件夹存在
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }

        // 2. 打包数据
        PlanetData data = new PlanetData(planet, saveName);
        string json = JsonUtility.ToJson(data, true); // true = 格式化输出，方便阅读

        // 3. 写入文件
        string filePath = Path.Combine(SaveFolder, saveName + ".json");
        File.WriteAllText(filePath, json);
        
        Debug.Log($"Planet saved to: {filePath}");
    }

    public static PlanetData LoadPlanetData(string saveName)
    {
        string filePath = Path.Combine(SaveFolder, saveName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlanetData>(json);
        }
        else
        {
            Debug.LogError("Save file not found: " + filePath);
            return null;
        }
    }

    // 获取所有已保存的星球名称列表
    public static List<string> GetSavedPlanetNames()
    {
        if (!Directory.Exists(SaveFolder)) return new List<string>();

        // 获取文件夹下所有 .json 文件
        DirectoryInfo info = new DirectoryInfo(SaveFolder);
        FileInfo[] files = info.GetFiles("*.json");

        // 只返回文件名（不带后缀），方便 UI 显示
        return files.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList();
    }
}