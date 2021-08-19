//
// Контроллер по сути ответственный за вьюшку UI и ее взаимодействие с миром.
// С названием нужно подумать, так как по сути это и как бы контроллер вьюшек,
// и, блин, КОНТРОЛЛЕР, который висеть должен на самом маинканвасе (читай-вьюшке).
// По идее зона ответственности - принимать нажатия кнопок и обновлять UI инфу на экране по эвентам.
// Но есть вопросы по правильному разделению обязанностей. Требует детального продумывания.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasController : MonoBehaviour
{
    private LevelController _levelController;

    [Header("Panels")]
    [SerializeField]
    private GameObject _mainMenuPanel;



    public void Init(LevelController levelController)
    {
        _levelController = levelController;

        ShowMainMenu();
    }

    /// <summary>
    /// Проброска кнопки старта с вьюшки маинканваса на контроллер уровня.
    /// Есть вариант замены на евенты. Нужно оценить адекватность замены.
    /// </summary>
    public void LevelStart()
    {
        if (_mainMenuPanel.activeSelf) _mainMenuPanel.SetActive(false);

        if (_levelController != null) _levelController.LevelStart();    // проверка на параноика :D
    }

    private void ShowMainMenu()
    {
        if (!_mainMenuPanel.activeSelf) _mainMenuPanel.SetActive(true);
    }
}
