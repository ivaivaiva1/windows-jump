using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class FitCameraAndRenderTexture : MonoBehaviour
{
    public SpriteRenderer squareSprite;     // O sprite que define o tamanho da janela
    public RawImage targetRawImage;         // Onde a imagem vai aparecer
    public int pixelsPerUnit = 100;         // Escala da textura (100 = 100px = 1 unidade)
    public int pixelSnap = 8;              // Múltiplo ideal pro RenderTexture

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        FitToSquare();
    }

    public void FitToSquare()
    {
        if (squareSprite == null || targetRawImage == null) return;

        // --- Parte 1: Ajusta a câmera para centralizar no Square ---
        Bounds bounds = squareSprite.bounds;
        Vector3 center = bounds.center;
        Vector2 size = bounds.size;

        transform.position = new Vector3(center.x, center.y, transform.position.z);

        float aspect = size.x / size.y;
        cam.aspect = aspect;

        if (aspect > 1)
            cam.orthographicSize = (size.x / aspect) * 0.5f;
        else
            cam.orthographicSize = size.y * 0.5f;

        // --- Parte 2: Cria um RenderTexture com proporção e nitidez boas ---

        int texWidth = SnapToPixel(Mathf.RoundToInt(size.x * pixelsPerUnit), pixelSnap);
        int texHeight = SnapToPixel(Mathf.RoundToInt(size.y * pixelsPerUnit), pixelSnap);

        RenderTexture rt = new RenderTexture(texWidth, texHeight, 16)
        {
            name = "RT_" + gameObject.name,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        cam.targetTexture = rt;
        targetRawImage.texture = rt;
    }

    private int SnapToPixel(int value, int multiple)
    {
        return Mathf.CeilToInt(value / (float)multiple) * multiple;
    }
}
