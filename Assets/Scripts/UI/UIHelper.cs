using System;
using UnityEngine;
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
        private void LateUpdate()
        {
            var current = EventSystem.current;
            var currentSelected = current.currentSelectedGameObject;
            var firstSelected = current.firstSelectedGameObject;

           // Console.Clear();
            //Debug.ClearDeveloperConsole();

            //print("currentSelected: " + currentSelected);
            //print("firstSelected: " + firstSelected);
    
            _frame.gameObject.SetActive(currentSelected == gameObject);

            if (currentSelected != null &&
                currentSelected.activeInHierarchy) return;
            if (firstSelected == null)
            {
                current.SetSelectedGameObject(gameObject);
            }
            else
            {
                current.SetSelectedGameObject(firstSelected);
            }
        }
    }
}
