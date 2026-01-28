using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UrbanNinja;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _loadGameSceneButton;
    [SerializeField] private Button _loadMainMenuSceneButton;
    [SerializeField] private Button _loadNameSelectSceneButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _nameField;

    private void OnEnable()
    {
        if (_loadGameSceneButton != null)
        {
            _loadGameSceneButton.onClick.AddListener(OnLoadGameScene);
        }
        if (_loadMainMenuSceneButton != null)
        {
            _loadMainMenuSceneButton.onClick.AddListener(SceneLoader.LoadMainMenuScene);
        }
        if (_loadNameSelectSceneButton != null)
        {
            _loadNameSelectSceneButton.onClick.AddListener(SceneLoader.LoadNameSelectScene);
        }
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(OnRestartGame);
        }
    }
    private void OnDisable()
    {
        if (_loadGameSceneButton != null)
        {
            _loadGameSceneButton.onClick.RemoveListener(OnLoadGameScene);
        }
        if (_loadMainMenuSceneButton != null)
        {
            _loadMainMenuSceneButton.onClick.RemoveListener(SceneLoader.LoadMainMenuScene);
        }
        if (_loadNameSelectSceneButton != null)
        {
            _loadNameSelectSceneButton.onClick.RemoveListener(SceneLoader.LoadNameSelectScene);
        }
        if (_restartButton != null)
        {
            _restartButton.onClick.RemoveListener(OnRestartGame);
        }
    }
    private void OnLoadGameScene()
    {
        string name = _nameField.text;
        GameManager.StartNewGame(name);
        SceneLoader.LoadGameScene();
    }
    private void OnRestartGame()
    {
        string name = GameManager.GetCurrentPlayerName();
        GameManager.StartNewGame(name);
        SceneLoader.LoadGameScene();
    }
}
