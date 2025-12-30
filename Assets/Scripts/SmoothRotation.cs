using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    [Header("目标旋转角度")]
    [SerializeField] private Vector3 targetRotation = new Vector3(0, 360, 0);
    
    [Header("旋转速度")]
    [SerializeField] private float rotationDuration = 5f; // 完成一次旋转的时间（秒）
    
    [Header("缓动类型")]
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private Vector3 startRotation;
    private float elapsedTime;
    
    void Start()
    {
        startRotation = transform.eulerAngles;
        elapsedTime = 0f;
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        // 计算进度（0到1之间循环）
        float progress = (elapsedTime % rotationDuration) / rotationDuration;
        
        // 应用缓动曲线
        float easedProgress = easeCurve.Evaluate(progress);
        
        // 插值旋转角度
        Vector3 newRotation = Vector3.Lerp(startRotation, startRotation + targetRotation, easedProgress);
        transform.eulerAngles = newRotation;
    }
}