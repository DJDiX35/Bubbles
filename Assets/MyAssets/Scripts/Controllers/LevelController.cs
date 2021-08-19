using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Settings _settings;


    public void Init(
    Settings settings
    )
    {
        _settings = settings;
    }

    public void LevelStart()
    {
        _settings.level.Start();
        _settings.game.State = GameState.GamePlay;
    }
    public void LevelStop()
    {
        _settings.level.End();
    }
}