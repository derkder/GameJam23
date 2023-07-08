using Assets.Scripts;
using UnityEngine;
public class TwistEffect : PostEffectsBase
{
    private bool _isTwisting;
    private float _startTime;
    private float _twistStrength;
    private float _clockwise;
    public bool _clickWell;
    private float _twistPosx;
    private float _twistPosy;

    // 放大强度
    [Range(-2.0f, 2.0f), Tooltip("放大强度")]
    public float zoomFactor = 0.4f;

    // 放大镜大小
    [Range(-2.0f, 2.0f), Tooltip("放大镜大小")]
    public float size = 0.08f;

    // 凸镜边缘强度
    [Range(0.0001f, 0.1f), Tooltip("凸镜边缘强度")]
    public float edgeFactor = 0.07f;

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
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Input.mousePosition;
            //将mousePos转化为（0，1）区间
            if (!_isTwisting)
            {
                if(_clickWell == true)
                {
                    TwistPlane(_twistPosx / Screen.width, _twistPosy / Screen.height);
                }
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            _isTwisting = false;
            _clickWell = false;
        }
    }


    /// <summary>
    /// 创造漩涡渲染效果
    /// </summary>
    private void TwistPlane(float x, float y, float twistStrength = 20, bool clockwise = false)
    {
        _twistStrength = twistStrength;
        _clockwise = clockwise == true ? -1 : 1;
        pos = new Vector2(x, y);
        _isTwisting = true;
        _startTime = Time.time;
    }

    public void ChangeTwistVal(Transform trans, GravityWellModifierStatus curStatus, float strength)
    {
        _twistPosx = GameManager.Instance.MainCamera.WorldToScreenPoint(trans.position).x;
        _twistPosy = GameManager.Instance.MainCamera.WorldToScreenPoint(trans.position).y;
        _clickWell = true;
    }
}