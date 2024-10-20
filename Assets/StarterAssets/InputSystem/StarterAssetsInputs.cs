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
		public bool aim;
		public bool shoot;
		public Vector2 scroll;
		public bool interact;
		public bool alpha0;
		public bool alpha1;
		public bool alpha2;
		public bool alpha3;
		public bool alpha4;
		public bool alpha5;
		public bool alpha6;
		public bool alpha7;
		public bool escape;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

		public void OnScroll(InputValue value)
		{
			ScrollInput(value.Get<Vector2>());
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void OnAlpha0(InputValue value)
		{
			Alpha0Input(value.isPressed);
		}

		public void OnAlpha1(InputValue value)
		{
			Alpha1Input(value.isPressed);
		}

		public void OnAlpha2(InputValue value)
		{
			Alpha2Input(value.isPressed);
		}

		public void OnAlpha3(InputValue value)
		{
			Alpha3Input(value.isPressed);
		}

		public void OnAlpha4(InputValue value)
		{
			Alpha4Input(value.isPressed);
		}

		public void OnAlpha5(InputValue value)
		{
			Alpha5Input(value.isPressed);
		}

		public void OnAlpha6(InputValue value)
		{
			Alpha6Input(value.isPressed);
		}

		public void OnAlpha7(InputValue value)
		{
			Alpha7Input(value.isPressed);
		}

		public void OnEscape(InputValue value)
		{
			EscapeInput(value.isPressed);
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

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void ScrollInput(Vector2 newScrollState)
		{
			scroll = newScrollState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void Alpha0Input(bool newAlpha0State)
		{
			alpha0 = newAlpha0State;
		}

		public void Alpha1Input(bool newAlpha1State)
		{
			alpha1 = newAlpha1State;
		}

		public void Alpha2Input(bool newAlpha2State)
		{
			alpha2 = newAlpha2State;
		}

		public void Alpha3Input(bool newAlpha3State)
		{
			alpha3 = newAlpha3State;
		}

		public void Alpha4Input(bool newAlpha4State)
		{
			alpha4 = newAlpha4State;
		}

		public void Alpha5Input(bool newAlpha5State)
		{
			alpha5 = newAlpha5State;
		}

		public void Alpha6Input(bool newAlpha6State)
		{
			alpha6 = newAlpha6State;
		}

		public void Alpha7Input(bool newAlpha7State)
		{
			alpha7 = newAlpha7State;
		}

		public void EscapeInput(bool newEscapeState)
		{
			escape = newEscapeState;
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