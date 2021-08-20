using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Bubble class
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class Bubble : PopUpObject
{
    [SerializeField]
    private float _sizeDelta = 0.5f;

    public override void GenerateSize(float size)
    {
        float _size = Random.Range(size * (1 - _sizeDelta), size * (1 + _sizeDelta));
        transform.localScale = new Vector3(_size, _size, 1f);
    }
}
