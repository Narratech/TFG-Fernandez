using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool crouch;
        public bool shoot;
        public bool interact;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpInput(context.action.IsPressed());
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // el botón de crouching es algo que sólo interesa como "primera pulsación"
            // (crouching es algo que se togglea, no hay que estar constantemente pulsándolo)
            if (!context.started) return;

            // siempre se cambia a hacer lo que no se estuviera haciendo
            CrouchInput(!crouch);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            InteractInput(context.started);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            SprintInput(context.action.IsPressed());
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void CrouchInput(bool newCrouchState)
        {
            crouch = newCrouchState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}