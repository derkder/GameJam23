using UnityEngine;
public class BlurEffect : PostEffectsBase
{


    [Header("Radial Blur")]
    [Range(0, 1)]
    public float intensity = 0.0f;                         // Ч��ǿ��
    [Range(0f, 1f)]
    public float fadeRadius = 0.38f;                    // �뾶��Χ�ڵĶ�����
    [Range(0f, 1f)]
    public float sampleDistance = 0.25f;                // ����ģ��ÿ�β����ľ���
    [Range(1, 40)]
    public int blurDownSample = 40;                     // ģ���������������

    void Start()
    {
        //�ҵ���Ӧ��Shader�ļ�  
        shader = Shader.Find("screenEffect/RadialBlur");
        GameManager.Instance.MainCamera.depthTextureMode |= DepthTextureMode.Depth;
    }

    // ��Ⱦ��Ļ
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!material)
        {
            Graphics.Blit(source, destination);
            return;
        }
        var rw = Screen.width / blurDownSample;
        var rh = Screen.height / blurDownSample;
        RenderTexture downSampleRT = RenderTexture.GetTemporary(rw, rh, 0);
        downSampleRT.filterMode = FilterMode.Bilinear;
        downSampleRT.name = "downSampleRT";
        Graphics.Blit(source, downSampleRT);
        material.SetFloat("_Intensity", intensity);
        material.SetFloat("_FadeRadius", fadeRadius);
        material.SetFloat("_SampleDistance", sampleDistance);
        material.SetTexture("_DownSampleRT", downSampleRT);
        material.SetTexture("_SrcTex", source);
        Graphics.Blit(null, destination, material, 0);
        RenderTexture.ReleaseTemporary(downSampleRT);

    }
}