using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


namespace GameCells.UI
{
    public class GCUI_Panel : MonoBehaviour
    {
        [SerializeField] private Selectable _firstSelected;

        private Selectable _previousSelected = null;

        private InputAction _cancelKey = null;

        private void OnEnable()
        {
            _cancelKey ??= EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.FindAction("Cancel");
            _cancelKey.performed += HideOnCancelAction;
        }

        private void OnDisable()
        {
            _cancelKey.performed -= HideOnCancelAction;
        }

        private void HideOnCancelAction(InputAction.CallbackContext ctx)
        {
            Hide();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            _previousSelected = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();

            _firstSelected.Select();
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);

            if (_previousSelected != null)
            {
                _previousSelected.Select();
            }
        }
    }
}