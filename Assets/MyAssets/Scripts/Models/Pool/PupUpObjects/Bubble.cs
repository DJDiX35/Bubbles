using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bubble : PopUpObject
{
    [SerializeField]
    private MinMaxTypes.Float _sizeDelta = new MinMaxTypes.Float() { min = 1f, max = 5f };

    public float Size { get; private set; }

    [SerializeField]
    private CircleCollider2D _collider;

    public override void Init()
    {
        base.Init();

        GenerateAndApplySize();
        SetColliderData();
    }

    private void GenerateAndApplySize()
    {
        Size = Random.Range(_sizeDelta.min, _sizeDelta.max);
        transform.localScale = new Vector3(Size, Size, 1f);
    }
    private void SetColliderData()
    {
        if (_collider == null) _collider = GetComponent<CircleCollider2D>();

        if (!_collider.isTrigger) _collider.isTrigger = true;
    }

}
