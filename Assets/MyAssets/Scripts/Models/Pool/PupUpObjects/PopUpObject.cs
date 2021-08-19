using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpObject : MonoBehaviour
{
    [SerializeField]
    private MinMaxTypes.Float _popUpSpeedDelta = new MinMaxTypes.Float() { min = 1f, max = 2f };
    private float _popUpSpeed = 1f;

    [SerializeField]
    private MinMaxTypes.Float _fluctuationDelta = new MinMaxTypes.Float() { min = 1f, max = 2f };
    private float _fluctuation = 1f;

    [SerializeField]
    private bool _randomizeDirection = true;

    public virtual void Init()
    {
        GenerateBaseValues();
    }

    private void GenerateBaseValues()
    {
        _popUpSpeed = Random.Range(_popUpSpeedDelta.min, _popUpSpeedDelta.max);
        _fluctuation = Random.Range(_fluctuationDelta.min, _fluctuationDelta.max);
        if (_randomizeDirection && Random.Range(0, 2) != 0) _fluctuation *= -1;
    }

    public void MoveVertical(float timeScale)
    {
        Vector3 position = transform.position;
        position.y += _popUpSpeed * timeScale;
        transform.position = position;
    }

    public void MoveHorisontal(float timeScale)
    {
        Vector3 position = transform.position;
        position.x += Mathf.Sin(Time.time) * _fluctuation * timeScale;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter! (" + name + ")");

        /*
         * в данной функции необходимо прописать код на взаимодействие с шариком игрока
         */
    }
}
