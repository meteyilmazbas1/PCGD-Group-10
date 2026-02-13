using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UrbanNinja
{
    public class NameSelectionUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button playButton;
        [SerializeField] Button menuButton;
        [Header("Initials Slots")]
        [SerializeField] Button[] slots = new Button[3];
        [Header("Input actions")]
        [SerializeField] private InputActionReference _scrollLetterUp;
        [SerializeField] private InputActionReference _scrollLetterDown;

        int[] letterIndices = new int[3];
        int selectedSlot = -1;
        const int LETTER_COUNT = 26;
        string playerInitials = "AAA";

        void Awake()
        {
            playButton.onClick.AddListener(PlayButtonAction);
            menuButton.onClick.AddListener(MenuButtonAction);
            _scrollLetterUp.action.performed += ctx => OnScrollUp();
            _scrollLetterDown.action.performed += ctx => OnScrollDown();
        }

        void Start()
        {
            UpdateAllSlots();
        }

        void Update()
        {
            HandleSlotSwitch();
            //HandleLetterChange();
            /*
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                print("Current Initials: " + GetInitials());
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            }*/
        }

        void HandleSlotSwitch()
        {
            EventSystem current = EventSystem.current;
            if (current.currentSelectedGameObject == slots[0].gameObject)
            {
                selectedSlot = 0;
            }
            else if (current.currentSelectedGameObject == slots[1].gameObject)
            {
                selectedSlot = 1;
            }
            else if (current.currentSelectedGameObject == slots[2].gameObject)
            {
                selectedSlot = 2;
            }
            else
            {
                selectedSlot = -1;
            }
        }
        /*
        void HandleLetterChange()
        {
            

            if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnScrollDown();
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnScrollUp();
            }
        }*/

        private void OnScrollUp()
        {
            if (selectedSlot == -1) { return; }
            letterIndices[selectedSlot]--;
            if (letterIndices[selectedSlot] < 0)
                letterIndices[selectedSlot] = LETTER_COUNT - 1;

            UpdateSlot(selectedSlot);
        }

        private void OnScrollDown()
        {
            if (selectedSlot == -1) { return; }
            letterIndices[selectedSlot] =
                (letterIndices[selectedSlot] + 1) % LETTER_COUNT;

            UpdateSlot(selectedSlot);
        }

        void UpdateSlot(int index)
        {
            char letter = (char)('A' + letterIndices[index]);
            slots[index].GetComponentInChildren<TMP_Text>().text = letter.ToString();
        }

        void UpdateAllSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                UpdateSlot(i);
            }
        }

        string GetInitials()
        {
            for (int i = 0; i < letterIndices.Length; i++)
            {
                playerInitials = playerInitials[..i]
                    + (char)('A' + letterIndices[i])
                    + playerInitials[(i + 1)..];
            }
            return playerInitials;
        }

        void PlayButtonAction()
        {
            print("Initials Submitted: " + GetInitials());
            GameManager.StartNewGame(GetInitials());
            SoundManager.Instance.PlayButtonClick();
            SceneNavigationManager.Instance.LoadScene(Scenes.GameScene);
        }

        void MenuButtonAction()
        {
            SoundManager.Instance.PlayButtonClick();
            SceneNavigationManager.Instance.LoadScene(Scenes.MainMenu);
        }
    }
}