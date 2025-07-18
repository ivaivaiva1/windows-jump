using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class FitCameraAndRenderTexture : MonoBehaviour
{
    public SpriteRenderer squareSprite;      // O sprite que define o tamanho da janela (alvo da câmera)
    public SpriteRenderer targetSquare;      // O sprite que define o tamanho desejado para o squareSprite
    public RawImage targetRawImage;          // Onde a imagem vai aparecer
    private int pixelsPerUnit = 100;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        FitSquareToTargetSquare();
    }

    public void FitSquareToTargetSquare()
    {
        if (squareSprite == null || targetSquare == null) return;

        Vector2 targetSize = targetSquare.bounds.size;
        Vector2 currentSize = squareSprite.bounds.size;

        Vector3 newScale = squareSprite.transform.localScale;
        newScale.x *= targetSize.x / currentSize.x;
        newScale.y *= targetSize.y / currentSize.y;

        squareSprite.transform.localScale = newScale;

        FitToSquare();
    }

    public void FitToSquare()
    {
        if (squareSprite == null || targetRawImage == null) return;

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

        int texWidth = Mathf.RoundToInt(size.x * pixelsPerUnit);
        int texHeight = Mathf.RoundToInt(size.y * pixelsPerUnit);

        RenderTexture rt = new RenderTexture(texWidth, texHeight, 16)
        {
            name = "RT_" + gameObject.name,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
            useMipMap = false,
            autoGenerateMips = false,
            antiAliasing = 1
        };

        cam.targetTexture = rt;
        targetRawImage.texture = rt;
    }
}
