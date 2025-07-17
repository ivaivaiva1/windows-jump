using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class SetPlayerCamera : MonoBehaviour
{
    public RawImage targetRawImage;

    private Camera cam;
    private RenderTexture rt;

    private int baseWidth = 1920;   // Resolução base em pixels (largura)
    private int baseHeight = 1080;  // Resolução base em pixels (altura)

    private int pixelsPerUnit = 512;  // Valor padrão

    void Start()
    {
        cam = GetComponent<Camera>();
        CreateRenderTexture(pixelsPerUnit);
    }

    // Função para criar ou recriar a RenderTexture com base no pixelsPerUnit
    public void CreateRenderTexture(int newPixelsPerUnit)
    {
        pixelsPerUnit = newPixelsPerUnit;

        if (rt != null)
        {
            cam.targetTexture = null;
            rt.Release();
            Destroy(rt);
        }

        // Ajusta resolução da RenderTexture proporcional à base e pixelsPerUnit
        // Aqui assumimos que a resolução da RT é fixa 1920x1080, 
        // mas você pode ajustar com base no pixelsPerUnit se quiser.
        int texWidth = baseWidth;
        int texHeight = baseHeight;

        rt = new RenderTexture(texWidth, texHeight, 16)
        {
            name = "RT_PlayerCamera",
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
            useMipMap = false,
            autoGenerateMips = false,
            antiAliasing = 1
        };

        cam.targetTexture = rt;

        if (targetRawImage != null)
        {
            targetRawImage.texture = rt;

            // Faz a RawImage preencher a tela
            targetRawImage.rectTransform.anchorMin = Vector2.zero;
            targetRawImage.rectTransform.anchorMax = Vector2.one;
            targetRawImage.rectTransform.offsetMin = Vector2.zero;
            targetRawImage.rectTransform.offsetMax = Vector2.zero;
        }
    }

    // Função pública para setar pixelsPerUnit externamente
    public void SetPixelsPerUnit(int newPixelsPerUnit)
    {
        CreateRenderTexture(newPixelsPerUnit);
    }
}
