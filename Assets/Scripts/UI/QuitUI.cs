using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UrbanNinja
{
    public class QuitUI : MonoBehaviour
    {
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;

        void Awake()
        {
            yesButton.onClick.AddListener(YesButtonAction);
            noButton.onClick.AddListener(NoButtonAction);
        }

        void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(noButton.gameObject);
        }

        void YesButtonAction()
        {
            SoundManager.Instance.PlayButtonClick();
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
#else
        Application.Quit();
#endif
        }

        void NoButtonAction()
        {
            SoundManager.Instance.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }

}