using UnityEngine;
using UnityEngine.UI;

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
