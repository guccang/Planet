using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlanetData
{
    public string saveName;
    public float radius;
    
    // 我们保存 ShapeSettings 和 ColorSettings 的 JSON 字符串
    // 这样做是为了避免处理复杂的嵌套类序列化问题，利用 Unity 自身的 JsonUtility 处理每一层
    public string shapeSettingsJson;
    public string colorSettingsJson;

    // 构造函数：从当前的星球提取数据
    public PlanetData(Planet planet, string name)
    {
        this.saveName = name;
        this.radius = planet.shapeSettings.planetRadius;

        // 将 ScriptableObject 里的数据转为 JSON 存起来
        this.shapeSettingsJson = JsonUtility.ToJson(planet.shapeSettings);
        this.colorSettingsJson = JsonUtility.ToJson(planet.colourSettings);
    }
}