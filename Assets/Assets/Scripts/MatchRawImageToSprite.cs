using UnityEngine;
using UnityEngine.UI;

public class MatchRawImageToSprite : MonoBehaviour
{
    public SpriteRenderer squareSprite; // O SpriteRenderer do "Square"
    public RawImage rawImage;           // A RawImage dentro de um Canvas World Space

    void Start()
    {
        Match();
    }

    public void Match()
    {
        if (squareSprite == null || rawImage == null) return;

        // Pega o tamanho do sprite no mundo
        Bounds bounds = squareSprite.bounds;
        Vector2 size = bounds.size;

        // Aplica esse tamanho na RawImage
        RectTransform rt = rawImage.rectTransform;

        // Zera escala para evitar interferência
        rt.localScale = Vector3.one;

        // Ajusta o tamanho diretamente em unidades de mundo
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

        // Posiciona o RawImage exatamente no mesmo lugar que o sprite
        rawImage.transform.position = bounds.center;
    }
}
