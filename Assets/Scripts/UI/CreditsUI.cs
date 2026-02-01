using UnityEngine;
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
    }

}