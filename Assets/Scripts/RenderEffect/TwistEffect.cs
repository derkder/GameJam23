﻿// create by 长生但酒狂
// create time 2020.4.8
// ---------------------------【放大镜特效】---------------------------

using UnityEngine;
public class TwistEffect : PostEffectsBase
{
    // shader
    //public Shader myShader;
    //材质 
    //private Material mat = null;
    //public Material material
    //{
    //    get
    //    {
    //        // 检查着色器并创建材质
    //        mat = CheckShaderAndCreateMaterial(myShader, mat);
    //        return mat;
    //    }
    //}
    private bool _isTwisting;
    private float _startTime;
    private float _twistStrength;
    private float _clockwise;

    // 放大强度
    [Range(-2.0f, 2.0f), Tooltip("放大强度")]
    public float zoomFactor = 0.4f;

    // 放大镜大小
    [Range(0.0f, 0.2f), Tooltip("放大镜大小")]
    public float size = 0.15f;

    // 凸镜边缘强度
    [Range(0.0001f, 0.1f), Tooltip("凸镜边缘强度")]
    public float edgeFactor = 0.05f;

    // 遮罩中心位置
    private Vector2 pos = new Vector4(0.5f, 0.5f);

    void Start()
    {
        //找到对应的Shader文件  
        shader = Shader.Find("screenEffect/Zoom");
    }

    // 渲染屏幕
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material && _isTwisting)
        {
            // 把鼠标坐标传递给Shader
            material.SetVector("_Pos", pos);
            material.SetFloat("_ZoomFactor", zoomFactor);
            material.SetFloat("_EdgeFactor", edgeFactor);
            material.SetFloat("_Size", size);
            material.SetFloat("_TimeDiff", Time.time - _startTime);
            material.SetFloat("_Radius", _twistStrength);
            material.SetFloat("_Clockwise", _clockwise);
            // 渲染
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            //将mousePos转化为（0，1）区间
            //pos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            if (!_isTwisting)
            {
                TwistPlane(mousePos.x / Screen.width, mousePos.y / Screen.height);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isTwisting = false;
        }
    }

    /// <summary>
    /// 创造漩涡渲染效果
    /// </summary>
    /// <param 坐标x值name="x"></param>
    /// <param 坐标y值name="y"></param>
    /// <param 旋转强度name="twistStrength"></param>
    /// <param 是否顺时针name="clockwise"></param>
    public void TwistPlane(float x, float y, float twistStrength = 20, bool clockwise = false)
    {
        _twistStrength = twistStrength;
        _clockwise = clockwise == true ? -1 : 1;
        pos = new Vector2(x, y);
        _isTwisting = true;
        _startTime = Time.time;
    }


}