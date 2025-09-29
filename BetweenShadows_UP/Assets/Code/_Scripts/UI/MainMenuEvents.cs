using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _startButton;

    private List<Button> _menuButtons = new List<Button>();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnStartGameClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
    }

    private void OnStartGameClick(ClickEvent e)
    {
        Debug.Log("Start Game Button Press");
    }
}
