using UnityEngine;

public class MatchCanvasToSprite : MonoBehaviour
{
    public SpriteRenderer squareSprite; // O SpriteRenderer do Square
    public Canvas worldSpaceCanvas;     // O Canvas em World Space

    void Start()
    {
        Match();
    }

    public void Match()
    {
        if (squareSprite == null || worldSpaceCanvas == null) return;

        // Tamanho do sprite em unidades de mundo
        Bounds bounds = squareSprite.bounds;
        Vector2 worldSize = bounds.size;

        // Ajusta o tamanho do RectTransform do Canvas
        RectTransform rt = worldSpaceCanvas.GetComponent<RectTransform>();
        if (rt == null) return;

        // Zera escala para evitar distorções
        rt.localScale = Vector3.one;

        // Considerar a escala atual do canvas no mundo
        Vector3 lossyScale = rt.lossyScale;

        // Corrige o tamanho do RectTransform dividindo pelo lossyScale
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSize.x / lossyScale.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSize.y / lossyScale.y);

        // Opcional: posiciona o canvas no centro do sprite
        worldSpaceCanvas.transform.position = bounds.center;
    }
}
