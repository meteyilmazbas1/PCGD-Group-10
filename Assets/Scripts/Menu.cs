using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _loadGameSceneButton;
        [SerializeField] private Button _loadMainMenuSceneButton;
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
                _loadMainMenuSceneButton.onClick.AddListener(LoadMainMenuScene);
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
                _loadMainMenuSceneButton.onClick.RemoveListener(LoadMainMenuScene);
            }
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(OnRestartGame);
            }
        }
        private void LoadMainMenuScene()
        {
            SceneNavigationManager.Instance.LoadScene(Scenes.MainMenu);
        }
        private void OnLoadGameScene()
        {
            string name = _nameField.text;
            GameManager.StartNewGame(name);
            SceneNavigationManager.Instance.LoadScene(Scenes.GameScene);
        }
        private void OnRestartGame()
        {
            string name = GameManager.GetCurrentPlayerName();
            GameManager.StartNewGame(name);
            SceneNavigationManager.Instance.LoadScene(Scenes.GameScene);
        }
    }

}