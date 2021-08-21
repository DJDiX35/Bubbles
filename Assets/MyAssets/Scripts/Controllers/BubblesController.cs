using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control all bubble (except player) movements during lifetime.
/// </summary>
public class BubblesController : MonoBehaviour
{
    private Settings _settings;
    private BubblesPool _pool;
    private Timer _timer;
    private float _spawnDelay;

    private int _spawnCount = 1;

    private Vector2 _spawnBorders = new Vector2(1, 1);

    private List<PopUpObject> _objects = new List<PopUpObject>();
    private List<PopUpObject> _toRemove = new List<PopUpObject>();

    private bool active = false;    // Stub to avoid doing Update BEFORE initializing. I'll come up with a better option - I'll replace it. I think it's ugly to turn off the script in the editor in advance.

    public void Init(
        Settings settings,
        BubblesPool pool,
        Timer timer
        )
    {
        _settings = settings;
        _pool = pool;
        _timer = timer;

        _spawnBorders = CameraExtensions.OrthographicBoundsVector2(Camera.main);
        _spawnBorders.x -= 0.5f;
        _spawnBorders.y += 1f;

        ClearPopUpList(_objects);

        Subscribe();

        active = true;
    }

    private void Subscribe()
    {
        _settings.level.StartEv -= StartSpawn;
        _settings.level.StartEv -= StartSpawn;
        _settings.level.StartEv += StartSpawn;

        _settings.level.EndEv -= StopSpawn;
        _settings.level.EndEv -= StopSpawn;
        _settings.level.EndEv += StopSpawn;
    }


    /// <summary>
    /// Update. Dont lost it. It's here.
    /// </summary>
    void Update()
    {
        if (!active) return;
        if (_settings.game.State != GameState.GamePlay) return;

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].MoveVertical(Time.deltaTime);
            _objects[i].MoveHorisontal(Time.deltaTime);

            CheckBubbleLife(_objects[i]);
        }

        ClearTrashCan();

        if (_spawnDelay > 0)
        {
            _spawnDelay -= Time.deltaTime;
        }
        else
        {
            SpawnBubbles((int)(_spawnCount));
        }
    }

    /// <summary>
    /// Checking bubble for "allow to die"
    /// </summary>
    /// <param name="bubble">Bubble to check</param>
    private void CheckBubbleLife(PopUpObject bubble)
    {
        if (bubble.transform.position.y < _spawnBorders.y) return;

        _toRemove.Add(bubble);
        _pool.Return(bubble);
    }

    /// <summary>
    /// Removing the bubbles that are in the "toRemove" list.
    /// </summary>
    private void ClearTrashCan()
    {
        if (_toRemove.Count == 0) return;

        for (int i = 0; i < _toRemove.Count; i++)
        {
            _objects.Remove(_toRemove[i]);
        }
        _toRemove.Clear();
    }


    private void StartSpawn()
    {
        _spawnCount = 1;

        ClearPopUpList(_objects);
    }

    private void StopSpawn()
    {
        ClearPopUpList(_objects);
    }

    /// <summary>
    /// Spawn many bubbles
    /// </summary>
    /// <param name="count">Spawn quantity</param>
    private void SpawnBubbles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOneBubble();
        }
    }

    /// <summary>
    /// Spawn one bubble of selected Type
    /// </summary>
    /// <param name="type">Type to spawn</param>
    private void SpawnOneBubble(PopUpType type = PopUpType.Bubble)
    {
        _objects.Add(
            _pool.Spawn(type, new Vector3(Random.Range(-_spawnBorders.x, _spawnBorders.x), -_spawnBorders.y, 0f))
        );

        _spawnDelay = _settings.bubbles.SpawnDelay;
    }


    /// <summary>
    /// Clear list of active bubbles and remove all objects back to pool
    /// </summary>
    /// <param name="list">List to Clear</param>
    private void ClearPopUpList(List<PopUpObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            _objects.Remove(list[i]);
        }
        list.Clear();
    }

}
