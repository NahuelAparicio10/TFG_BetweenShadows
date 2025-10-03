using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;
    private List<Button> _menuButtons = new List<Button>();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q<Button>("StartButton");
        _settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
        _exitButton = _document.rootVisualElement.Q<Button>("ExitButton");

        _startButton.clicked += HandleStartGame;
        _settingsButton.clicked += HandleSettings;
        _exitButton.clicked += HandleExit;

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        foreach (var btn in _menuButtons)
        {
            btn.focusable = true;

            btn.RegisterCallback<PointerEnterEvent>(evt => {
                OnButtonPointerEnter(btn);
            });
            btn.RegisterCallback<PointerLeaveEvent>(evt => {
                OnButtonPointerLeave(btn);
            });
        }

        var root = _document.rootVisualElement;
        root.RegisterCallback<NavigationMoveEvent>(OnNavMove);

    }

    private void OnButtonPointerEnter(Button btn)
    {
        btn.Focus();
    }

    private void OnButtonPointerLeave(Button btn)
    {
        if (_document.rootVisualElement.focusController.focusedElement == btn)
        {
            var focusedElement = _document.rootVisualElement.focusController.focusedElement;
            _document.rootVisualElement.focusController.focusedElement.Blur();
        }
    }

    private void HandleStartGame() => Debug.Log("Start Game Button Pressed");
    private void HandleSettings() => Debug.Log("Settings Button Pressed");
    private void HandleExit() => Debug.Log("Exit Button Pressed");

    private void OnNavMove(NavigationMoveEvent evt)
    {
        switch (evt.direction)
        {
            case NavigationMoveEvent.Direction.Up:
                MoveFocusVertical(-1);
                break;
            case NavigationMoveEvent.Direction.Down:
                MoveFocusVertical(+1);
                break;
            default:
                return;
        }
    }

    private void MoveFocusVertical(int delta)
    {
        var focusedElement = _document.rootVisualElement.focusController.focusedElement;
        Button focusedBtn = focusedElement as Button;

        int nextIdx = 0;

        if (focusedBtn == null)
        {
            _menuButtons[0].Focus();
            return;
        }

        int idx = _menuButtons.IndexOf(focusedBtn);
        if (idx < 0)
        {
            _menuButtons[0].Focus();
            return;
        }

        int next = idx + delta;
        if (next < 0)
            next = _menuButtons.Count - 1;
        if (next >= _menuButtons.Count)
            next = 0;

        _menuButtons[next].Focus();
    }
}
