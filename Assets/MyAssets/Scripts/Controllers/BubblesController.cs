using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesController : MonoBehaviour
{
    private Settings _settings;
    private BubblesPool _pool;
    private Timer _timer;
    private float _spawnDelay;

    private int _spawnCount = 1; // каунтер на спавн после первых заспавленных пузырей

    private Vector2 _spawnBorders = new Vector2(1, 1);

    private List<PopUpObject> _objects = new List<PopUpObject>();
    private List<PopUpObject> _toRemove = new List<PopUpObject>();

    private bool active = false;    // Временное решение чтобы избежать выполнения Update ДО инициализации. Придумаю вариант лучше - заменю. выключать скрипт в редакторе заранее считаю некрасивым.

    public void Init(
        Settings settings,
        BubblesPool pool,
        Timer timer
        )
    {
        _settings = settings;
        _pool = pool;
        pool.Init();
        _timer = timer;

        _spawnBorders = CameraExtensions.OrthographicBoundsVector2(Camera.main);
        _spawnBorders.x -= 0.5f;
        _spawnBorders.y += 1f;

        ClearPopUpList(_objects);

        Subscrive();

        active = true;
    }

    private void Subscrive()
    {
        _settings.level.StartEv -= StartSpawn;
        _settings.level.StartEv -= StartSpawn;
        _settings.level.StartEv += StartSpawn;

        _settings.level.EndEv -= StopSpawn;
        _settings.level.EndEv -= StopSpawn;
        _settings.level.EndEv += StopSpawn;
    }


    /// <summary>
    /// Update. Не терем, он тут.
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


    private void CheckBubbleLife(PopUpObject bubble)
    {
        if (bubble.transform.position.y < _spawnBorders.y) return;

        _toRemove.Add(bubble);
        _pool.Return(bubble);
    }

    /// <summary>
    /// Удаляем пузырьки находящиеся в списке "на удаление".
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

        //SpawnBubbles(_settings.bubbles.StartCount);
    }

    private void StopSpawn()
    {
        ClearPopUpList(_objects);
    }


    private void SpawnBubbles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnOneBubble();
        }
    }

    private void SpawnOneBubble(PopUpType type = PopUpType.Bubble)
    {
        _objects.Add(
            _pool.Spawn(type, new Vector3(Random.Range(-_spawnBorders.x, _spawnBorders.x), -_spawnBorders.y, 0f))
        );

        _spawnDelay = _settings.bubbles.SpawnDelay;
    }



    private void ClearPopUpList(List<PopUpObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
    }

}
