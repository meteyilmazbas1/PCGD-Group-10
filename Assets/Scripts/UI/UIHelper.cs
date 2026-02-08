using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UrbanNinja
{
    public class UIHelper : MonoBehaviour
    {
        [SerializeField] private AudioClip _selectSound;
        [SerializeField] private AudioClip _submitSound;
        [SerializeField] private Image _frame;

        /// <summary>
        /// Bindings from EventTrigger on the game object.
        /// </summary>
        public void PlaySelect()
        {
            AudioSource.PlayClipAtPoint(_selectSound, Camera.main.transform.position);
        }
        /// <summary>
        /// Bindings from EventTrigger on the game object.
        /// </summary>
        public void PlaySubmit()
        {
            AudioSource.PlayClipAtPoint(_submitSound, Camera.main.transform.position);
        }
        private void OnEnable()
        {
            _frame.gameObject.SetActive(false);
        }
        private void FixedUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject != null &&
                EventSystem.current.currentSelectedGameObject.activeInHierarchy) return;
            if (EventSystem.current.firstSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
        }
    }
}
