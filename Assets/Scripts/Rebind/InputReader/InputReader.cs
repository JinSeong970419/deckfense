using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Deckfense
{
    [CreateAssetMenu(fileName ="InputReader", menuName = "Input System/InputReader")]
    public class InputReader : ScriptableObject, GameControls.IGameActionActions
    {
        public event UnityAction enterAction = delegate { };
        public event UnityAction<Action> backQute = delegate { };
        private bool backQuteActive = false;

        private GameControls controller;
        public GameControls Controller { get { return controller; } }

        private Vector2[] touchPositions = new Vector2[2];
        private float minDistanceToCheck = 50f;

        private void OnEnable()
        {
            if(controller == null)
            {
                controller = new GameControls();

                controller.GameAction.SetCallbacks(this);
                controller.Enable();
            }

            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            controller.GameAction.Disable();
            EnhancedTouchSupport.Disable();
        }

        public void OnBackQute(InputAction.CallbackContext context)
        {
            Action contextAction;
            backQuteActive = !backQuteActive;
            if (backQuteActive)
            {
                contextAction = () => { PopupManager.Instance.OpenPopup(PopupKind.PopupDebugConsole); };
            }
            else
            {
                contextAction = () => { PopupManager.Instance.ClosePopup(PopupKind.PopupDebugConsole); };
            }

            if (context.started)
            {
                backQute.Invoke(contextAction);
            }
        }

        public void OnEnterAction(InputAction.CallbackContext context)
        {
            if (context.started)
                enterAction.Invoke();
        }

        public void OnTouch(InputAction.CallbackContext context)
        {
            if (Touch.activeTouches.Count > 0)
            {
                int touchIndex = 0;

                foreach (var touch in Touch.activeTouches)
                {
                    if (touchIndex < 2)
                    {
                        touchPositions[touchIndex] = touch.screenPosition;
                        touchIndex++;
                    }
                }

                if (Vector2.Distance(touchPositions[0], touchPositions[1]) >= minDistanceToCheck)
                {
                    PopupManager.Instance.ClosePopup(PopupKind.PopupDebugConsole);
                }
            }
        }
    }
}