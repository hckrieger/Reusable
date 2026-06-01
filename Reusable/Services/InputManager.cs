using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reusable.Services
{
	public class InputManager
	{

		private KeyboardState currentKeyState;
		private KeyboardState previousKeyState;
		private GamePadState currentGamePadState;
		private GamePadState previousGamePadState;
		private MouseState currentMouseState, previousMouseState;
		DisplayManager displayManager;

		public enum BindingType
		{
			None,
			Platformer
		}

		public enum InputAction
		{
			Jump,
			Attack,
			Dash,
			Interact,
			MoveLeft,
			MoveRight,
			LookUp,
			Crouch,
			Pause
			
		}

		private BindingType bindingType;

		private Dictionary<BindingType, Dictionary<InputAction, Func<bool>>>? inputBindings { get; set; }

		public Dictionary<InputAction, Func<bool>>? Binding { get; set; }

		public InputManager(DisplayManager displayManager, BindingType bindingType = BindingType.None)
		{
			this.displayManager = displayManager;
			this.bindingType = bindingType;

			inputBindings = new Dictionary<BindingType, Dictionary<InputAction, Func<bool>>>();

			Dictionary<InputAction, Func<bool>> platformerBindings = new Dictionary<InputAction, Func<bool>>
			{
				{ InputAction.MoveLeft,  () => IsKeyDown(Keys.A) || IsButtonDown(Buttons.DPadLeft) },
				{ InputAction.MoveRight, () => IsKeyDown(Keys.D) || IsButtonDown(Buttons.DPadRight) },
				{ InputAction.LookUp,    () => IsKeyDown(Keys.W) || IsButtonDown(Buttons.DPadUp) },
				{ InputAction.Crouch,  () => IsKeyDown(Keys.S) || IsButtonDown(Buttons.DPadDown) },

				{ InputAction.Jump,      () => IsKeyPressed(Keys.Space) || IsButtonPressed(Buttons.A) },
				{ InputAction.Attack,    () => IsKeyPressed(Keys.U) || IsButtonPressed(Buttons.X) },
				{ InputAction.Dash,      () => IsKeyPressed(Keys.O) || IsButtonPressed(Buttons.B) },
				{ InputAction.Interact,  () => IsKeyPressed(Keys.E) || IsButtonPressed(Buttons.Y) },

				{ InputAction.Pause,     () => IsKeyPressed(Keys.Escape) || IsButtonPressed(Buttons.Start) }
			};

			inputBindings?.Add(BindingType.Platformer, platformerBindings);

			Binding = inputBindings?[bindingType];
		}

		public void Update()
		{
			previousKeyState = currentKeyState;
			currentKeyState = Keyboard.GetState();
			previousGamePadState = currentGamePadState;
			currentGamePadState = GamePad.GetState(0);
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();
			

		}

		public bool IsKeyDown(Keys key)
		{
			return currentKeyState.IsKeyDown(key);
		}

		public bool IsKeyPressed(Keys key)
		{
			return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
		}

		public bool IsKeyReleased(Keys key)
		{
			return !currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key);
		}



		public float MouseX => MousePosition.X;
		public float MouseY => MousePosition.Y;

		public Vector2 MousePosition => displayManager.ScreenToViewport(new Vector2(currentMouseState.X, currentMouseState.Y));

		public void SetMousePosition(int x, int y)
		{
			Mouse.SetPosition(x, y);
		}

		public bool IsMouseButtonDown()
		{
			return currentMouseState.LeftButton == ButtonState.Pressed;
		}

		public bool IsMouseButtonPressed()
		{
			return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
		}

		public bool IsMouseButtonJustReleased()
		{
			return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
		}	

		public bool IsMouseOver(Rectangle rectangle)
		{
			return rectangle.Contains(MousePosition);
		}
		
		public bool IsMousePressedOver(Rectangle rectangle)
		{
			return rectangle.Contains(MousePosition) & IsMouseButtonPressed(); 
		}

		public bool IsButtonDown(Buttons button)
		{
			return currentGamePadState.IsButtonDown(button);
		}

		public bool IsButtonPressed(Buttons button)
		{
			return currentGamePadState.IsButtonDown(button) && !previousGamePadState.IsButtonDown(button);
		}

		public bool IsButtonReleased(Buttons button)
		{
			return !currentGamePadState.IsButtonDown(button) && previousGamePadState.IsButtonDown(button);
		}


		public float RightTriggerValue => currentGamePadState.Triggers.Right;
		public Vector2 LeftThumbStick => currentGamePadState.ThumbSticks.Left;
		public float LeftTriggerValue => currentGamePadState.Triggers.Left;
		public Vector2 RightThumbStick => currentGamePadState.ThumbSticks.Right;

	}
}
