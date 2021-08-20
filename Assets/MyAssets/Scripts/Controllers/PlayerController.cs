using UnityEngine;

/// <summary>
/// Control all Player Bubble movements
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Settings.Game _game;
    private Settings.PlayerSettings _player;

    [SerializeField]
    private Transform _playerBubble;

    private float _startSize;
    private Vector2 _bounds;


    public void Init(
        Settings settings,
        InputBase input
        )
    {
        _game = settings.game;
        _player = settings.Player;

        _startSize = settings.Player.StartSize;

        _bounds = CameraExtensions.OrthographicBoundsVector2(Camera.main);

        Subscribe(settings, input);
    }

    #region Subscribe
    private void Subscribe(
        Settings settings, 
        InputBase input
        )
    {
        settings.level.StartEv -= ResetPlayerBubble;
        settings.level.StartEv -= ResetPlayerBubble;
        settings.level.StartEv += ResetPlayerBubble;

        settings.level.EndEv -= RemovePlayerBubble;
        settings.level.EndEv -= RemovePlayerBubble;
        settings.level.EndEv += RemovePlayerBubble;

        settings.game.StartEv -= RemovePlayerBubble;
        settings.game.StartEv -= RemovePlayerBubble;
        settings.game.StartEv += RemovePlayerBubble;

        input.MoveEv -= MovePlayerBubble;
        input.MoveEv -= MovePlayerBubble;
        input.MoveEv += MovePlayerBubble;
    }
    #endregion

    private void ResetPlayerBubble()
    {
        _playerBubble.position = Vector3.zero;
        _playerBubble.localScale = new Vector3(_startSize, _startSize, _startSize);
        _playerBubble.gameObject.SetActive(true);
    }

    private void RemovePlayerBubble()
    {
        _playerBubble.gameObject.SetActive(false);
    }

    private void MovePlayerBubble(Vector2 direction)
    {
        if (!gameObject.activeSelf) return;
        if (_game.State != GameState.GamePlay) return;

        Vector3 position = _playerBubble.position;
        position.x += direction.x;
        position.y += direction.y;
        position = CheckOutOfBounds(position, 0.001f);

        _playerBubble.position = position;

    }

    private Vector3 CheckOutOfBounds(Vector3 position, float accuracy)
    {
        // radius
        float r = _playerBubble.localScale.x * 0.5f;

        // on the X level is looped. Flew abroad - appeared from the other side.
        if (position.x < -_bounds.x) position.x = _bounds.x - accuracy;
        if (position.x > _bounds.x) position.x = -_bounds.x + accuracy;

        // on y - either looping or we stop and do not go further, to choose from. Uncomment what is needed.

        // loop
        /*if (position.y < -_bounds.y) position.y = _bounds.y - accuracy;
        if (position.y > _bounds.y) position.y = -_bounds.y + accuracy;*/

        // hard border
        if (position.y < -_bounds.y + r)
        {
            position.y = -_bounds.y + r + accuracy;
            _player.HitBorder(Settings.PlayerSettings.BorderType.Vertical);
        }
        if (position.y > _bounds.y - (r))
        {
            position.y = _bounds.y - (r + accuracy);
            _player.HitBorder(Settings.PlayerSettings.BorderType.Vertical);
        }

        return position;
    }

    public float GetBubbleSizeX()
    {
        return _playerBubble.transform.localScale.x;
    }
}
