using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    void Awake()
    {
    }

    private void ButtonAction()
    {
        print("Button click");
        SoundManager.Instance.PlayButtonClick();
    }
}
