using UnityEngine;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform image1;
        public Transform image2;
        public float speed = 1f;

        [HideInInspector] public float imageWidth;
    }

    [SerializeField] private List<ParallaxLayer> layers;
    [SerializeField] private Vector2 direction = Vector2.left;

    private float leftLimit;
    private float rightLimit;

    private void Start()
    {
        // Limites baseados no pai
        SpriteRenderer parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        if (parentSprite == null)
        {
            Debug.LogError("O pai do ParallaxController precisa ter um SpriteRenderer.");
            return;
        }

        float parentWidth = parentSprite.bounds.size.x;
        float centerX = parentSprite.bounds.center.x;
        leftLimit = centerX - parentWidth / 2f;
        rightLimit = centerX + parentWidth / 2f;

        foreach (var layer in layers)
        {
            SpriteRenderer sr = layer.image1.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("SpriteRenderer não encontrado em " + layer.image1.name);
                continue;
            }

            layer.imageWidth = sr.bounds.size.x;

            // Alinha a segunda imagem
            layer.image2.position = layer.image1.position + new Vector3(layer.imageWidth, 0, 0);
        }
    }

    private void Update()
    {
        foreach (var layer in layers)
        {
            Vector3 move = (Vector3)(direction.normalized * layer.speed * Time.deltaTime);

            layer.image1.position += move;
            layer.image2.position += move;

            // Verifica se precisa rebobinar
            WrapIfNeeded(layer.image1, layer.image2, layer.imageWidth);
            WrapIfNeeded(layer.image2, layer.image1, layer.imageWidth);
        }
    }

    private void WrapIfNeeded(Transform moving, Transform other, float width)
    {
        float offset = 0.01f; // para evitar erros de precisão acumulados

        if (direction.x < 0)
        {
            // Indo para a esquerda
            if (moving.position.x + width < leftLimit - offset)
                moving.position = other.position + Vector3.right * width;
        }
        else if (direction.x > 0)
        {
            // Indo para a direita
            if (moving.position.x > rightLimit + offset)
                moving.position = other.position - Vector3.right * width;
        }
    }
}
