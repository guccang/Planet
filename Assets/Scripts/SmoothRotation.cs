using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    [Header("旋转轴与速度")]
    [Tooltip("设置每个轴每秒旋转的角度")]
    public Vector3 rotationSpeed = new Vector3(0, 10f, 0); // 默认每秒沿 Y 轴转 10 度

    [Header("旋转空间")]
    public Space coordinateSpace = Space.Self;

    void Update()
    {
        // 使用 Time.deltaTime 确保旋转速度不受帧率影响
        // transform.Rotate 是 Unity 官方推荐的持续旋转方式，内部处理了四元数转换
        transform.Rotate(rotationSpeed * Time.deltaTime, coordinateSpace);
    }
}