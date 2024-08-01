using System;
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
        [Tooltip("Pressing cancel will be treated as pressing this button.")]
        [SerializeField] private Button _cancelKeyButton;

        //[SerializeField] private bool _hideOnCancel = true;

        private Selectable _previousSelected = null;

        private InputAction _cancelKey = null;

        public bool IsOpened { get; private set; } = false;

        public event Action OnShow;
        public event Action OnHide;

        private void OnEnable()
        {
            if (_cancelKeyButton != null)
            {
                _cancelKey ??= EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.FindAction("Cancel");
                _cancelKey.performed += HideOnCancelAction;
            }
        }

        private void OnDisable()
        {
            if (_cancelKeyButton != null)
                _cancelKey.performed -= HideOnCancelAction;
        }

        private void HideOnCancelAction(InputAction.CallbackContext ctx)
        {
            _cancelKeyButton.onClick?.Invoke();
            Hide();
        }

        public void Show()
        {
            IsOpened = true;

            this.gameObject.SetActive(true);

            _previousSelected = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();

            _firstSelected.Select();

            OnShow?.Invoke();
        }

        public void Hide()
        {
            IsOpened = false;

            this.gameObject.SetActive(false);

            if (_previousSelected != null)
            {
                _previousSelected.Select();
            }

            OnHide?.Invoke();
        }
    }
}