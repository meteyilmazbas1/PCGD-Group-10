using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class CreditsUI : MonoBehaviour
    {

        [SerializeField] Button backButton;

        void Awake()
        {
            backButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                SoundManager.Instance.PlayButtonClick();
            });
        }

        void OnEnable()
        {
             EventSystem.current.SetSelectedGameObject(backButton.gameObject);
        }
    }


}