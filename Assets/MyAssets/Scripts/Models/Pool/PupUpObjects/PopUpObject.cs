using UnityEngine;

/// <summary>
/// Universal mother-type class of pop-up objects.
/// </summary>
public abstract class PopUpObject : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _collider;

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
        SetColliderData();
    }

    private void GenerateBaseValues()
    {
        _popUpSpeed = Random.Range(_popUpSpeedDelta.min, _popUpSpeedDelta.max);
        _fluctuation = Random.Range(_fluctuationDelta.min, _fluctuationDelta.max);
        if (_randomizeDirection && Random.Range(0, 2) != 0) _fluctuation *= -1;
    }
    private void SetColliderData()
    {
        if (_collider == null) _collider = GetComponent<CircleCollider2D>();

        if (!_collider.isTrigger) _collider.isTrigger = true;
    }

    /// <summary>
    /// Link to change size function.
    /// Using by objects with variable sizes (like different bubbles), dependent by player bubble size.
    /// </summary>
    /// <param name="size">Size of Player Bubble</param>
    public virtual void GenerateSize(float size) { }


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
         * in this function, we need to write a code for interacting with the player's bubble
         */
    }
}
