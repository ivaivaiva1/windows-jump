using System.Collections.Generic;
using TarodevController;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class CollisionHandler : MonoBehaviour
{
    private string oneWayLayerName = "one way platform";
    private PlayerController playerController;

    private Collider2D _playerCollider;
    private Rigidbody2D _rb;
    private int _oneWayLayer;
    private bool needCheckCollisions = false;

    private List<Collider2D> ignoredColliders = new List<Collider2D>();
    private ContactFilter2D _filter;
    private Collider2D[] _overlapResults = new Collider2D[10];
    public bool goingUP;

    //private void Awake()
    //{
    //    _rb = GetComponent<Rigidbody2D>();
    //    playerController = GetComponent<PlayerController>();
    //    _playerCollider = GetComponent<Collider2D>();
    //    _oneWayLayer = LayerMask.NameToLayer(oneWayLayerName);

    //    // Ignorar colisão entre o layer do player e o layer oneWayLayer desde o início (para teste)
    //    Physics2D.IgnoreLayerCollision(gameObject.layer, _oneWayLayer, true);

    //    /*
    //    _filter = new ContactFilter2D
    //    {
    //        useLayerMask = true,
    //        layerMask = 1 << _oneWayLayer,
    //        useTriggers = false
    //    };

    //    if (playerController == null)
    //        Debug.LogError("PlayerController não foi atribuído no OneWayCollisionHandler.");
    //    */
    //}

    private void Awake()
    {
        _oneWayLayer = LayerMask.NameToLayer("one way platform");

        if (_oneWayLayer == -1)
        {
            Debug.LogError("Layer 'one way platform' não foi encontrada. Verifique se foi criada corretamente.");
            return;
        }

        Debug.Log($"Ignorando colisão entre {gameObject.layer} e {_oneWayLayer}");
        Physics2D.IgnoreLayerCollision(gameObject.layer, _oneWayLayer, true);
    }


    //private void FixedUpdate()
    //{
        
    //    goingUP = Player.Instance.playerController._frameVelocity.y > 0.01f;

    //    //// 1. Enquanto estiver subindo, ignorar colisão global com OneWayPlatform
    //    //Physics2D.IgnoreLayerCollision(gameObject.layer, _oneWayLayer, goingUp);

    //    //// 2. Detectar a transição: estava subindo e parou de subir
    //    //if (needCheckCollisions && !goingUp)
    //    //{
    //    //    CheckAndIgnoreOneWayOverlaps();
    //    //    needCheckCollisions = false;
    //    //}

    //    //if (goingUp)
    //    //{
    //    //    needCheckCollisions = true;
    //    //}

    //    //// 3. Restaurar colisões quando encostar no chão
    //    //if (playerController._grounded && ignoredColliders.Count > 0)
    //    //{
    //    //    foreach (var col in ignoredColliders)
    //    //    {
    //    //        if (col != null)
    //    //            Physics2D.IgnoreCollision(_playerCollider, col, false);
    //    //    }

    //    //    ignoredColliders.Clear();
    //    //}
    //    //*/
    //}

    private void CheckAndIgnoreOneWayOverlaps()
    {
        /*
        int count = _playerCollider.OverlapCollider(_filter, _overlapResults);

        for (int i = 0; i < count; i++)
        {
            var col = _overlapResults[i];
            if (col != null && !ignoredColliders.Contains(col))
            {
                Physics2D.IgnoreCollision(_playerCollider, col, true);
                ignoredColliders.Add(col);
            }
        }
        */
    }
}
