using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UrbanNinja
{
    public class UIHelper : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioClip _selectSound;
        [SerializeField] private AudioClip _submitSound;
        [SerializeField] private Image _frame;

        /// <summary>
        /// Bindings from EventTrigger on the game object.
        /// </summary>
        public void PlaySelect()
        {
            SoundManager.Instance.PlaySelect();
        }

        /// <summary>
        /// Bindings from EventTrigger on the game object.
        /// </summary>
        public void PlaySubmit()
        {
            SoundManager.Instance.PlaySubmit();
        }

        private void OnEnable()
        {
            _frame.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            var current = EventSystem.current;
            var currentSelected = current.currentSelectedGameObject;
            var firstSelected = current.firstSelectedGameObject;

            _frame.gameObject.SetActive(currentSelected == gameObject);

            if (currentSelected != null && currentSelected.activeInHierarchy) return;

            if (firstSelected == null)
            {
                current.SetSelectedGameObject(gameObject);
            }
            else
            {
                current.SetSelectedGameObject(firstSelected);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var current = EventSystem.current;
            var currentSelected = current.currentSelectedGameObject;
            if (currentSelected != gameObject)
            {
                current.SetSelectedGameObject(gameObject);
            }
        }
    }
}
