using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float _timerScale = 0.01f;
    public float Time { get; private set; }

    private bool active = false;

    public void Init(Settings.Level level)
    {
        Subscribe(level);
    }

    private void Subscribe(Settings.Level level)
    {
        level.StartEv -= StartTimer;
        level.StartEv -= StartTimer;
        level.StartEv += StartTimer;

        level.EndEv -= StopTimer;
        level.EndEv -= StopTimer;
        level.EndEv += StopTimer;
    }


    private void StartTimer()
    {
        Time = 0;
        StartCoroutine(TimerCoroutine());
    }

    private void StopTimer()
    {
        if (!active) return;

        StopCoroutine(TimerCoroutine());
        active = false;
    }

    IEnumerator TimerCoroutine()
    {
        active = true;
        for (; ; )
        {
            Time += _timerScale;
            yield return new WaitForSeconds(_timerScale);
        }
    }
}
