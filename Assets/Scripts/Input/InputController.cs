using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InControl;

public class InputController : MonoBehaviour {

	public class PlayerActions : PlayerActionSet {
//		public PlayerAction Fire;
//		public PlayerAction Reload;
		public PlayerAction Melee;
//		public PlayerAction Interact;
//		public PlayerAction Aim;
//		public PlayerAction Sprint;
		public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;
		public PlayerTwoAxisAction Move;
//		public PlayerAction LookUp;
//		public PlayerAction LookDown;
//		public PlayerAction LookLeft;
//		public PlayerAction LookRight;
//		public PlayerTwoAxisAction Look;
		public PlayerAction Menu;

		public PlayerActions() {
//			Fire = CreatePlayerAction( "Fire" );
//			Reload = CreatePlayerAction( "Reload" );
			Melee = CreatePlayerAction( "Melee" );
//			Interact = CreatePlayerAction( "Interact" );
//			Aim = CreatePlayerAction( "Aim" );
//			Sprint = CreatePlayerAction( "Sprint" );
			Left = CreatePlayerAction( "Move Left" );
			Right = CreatePlayerAction( "Move Right" );
			Up = CreatePlayerAction( "Move Up" );
			Down = CreatePlayerAction( "Move Down" );
			Move = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
//			LookUp = CreatePlayerAction( "Look Up" );
//			LookDown = CreatePlayerAction( "Look Down" );
//			LookLeft = CreatePlayerAction( "Look Left" );
//			LookRight = CreatePlayerAction( "Look Right" );
//			Look = CreateTwoAxisPlayerAction( LookLeft, LookRight, LookDown, LookUp );
			Menu = CreatePlayerAction( "Menu" );
		}

		public static PlayerActions CreateWithDefaultBindings() {
			var playerActions = new PlayerActions();

//			playerActions.Fire.AddDefaultBinding( InputControlType.RightTrigger );
//			playerActions.Fire.AddDefaultBinding( Mouse.LeftButton );

//			playerActions.Reload.AddDefaultBinding( InputControlType.Action3 );
//			playerActions.Reload.AddDefaultBinding( Key.R );

			playerActions.Melee.AddDefaultBinding( Key.F );
			playerActions.Melee.AddDefaultBinding( InputControlType.Action2 );

//			playerActions.Interact.AddDefaultBinding( Key.E );
//			playerActions.Interact.AddDefaultBinding( InputControlType.Action1 );

//			playerActions.Aim.AddDefaultBinding( Mouse.RightButton );
//			playerActions.Aim.AddDefaultBinding( InputControlType.LeftTrigger );

//			playerActions.Sprint.AddDefaultBinding( Key.LeftShift );
//			playerActions.Sprint.AddDefaultBinding( InputControlType.LeftStickButton );

			playerActions.Up.AddDefaultBinding( Key.UpArrow );
			playerActions.Down.AddDefaultBinding( Key.DownArrow );
			playerActions.Left.AddDefaultBinding( Key.LeftArrow );
			playerActions.Right.AddDefaultBinding( Key.RightArrow );

			playerActions.Up.AddDefaultBinding( Key.W );
			playerActions.Down.AddDefaultBinding( Key.S );
			playerActions.Left.AddDefaultBinding( Key.A );
			playerActions.Right.AddDefaultBinding( Key.D );

			playerActions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
			playerActions.Right.AddDefaultBinding( InputControlType.LeftStickRight );
			playerActions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
			playerActions.Down.AddDefaultBinding( InputControlType.LeftStickDown );

//			playerActions.LookUp.AddDefaultBinding( InputControlType.RightStickUp );
//			playerActions.LookDown.AddDefaultBinding( InputControlType.RightStickDown );
//			playerActions.LookLeft.AddDefaultBinding( InputControlType.RightStickLeft );
//			playerActions.LookRight.AddDefaultBinding( InputControlType.RightStickRight );

//			playerActions.LookUp.AddDefaultBinding( Mouse.PositiveY );
//			playerActions.LookDown.AddDefaultBinding( Mouse.NegativeY );
//			playerActions.LookLeft.AddDefaultBinding( Mouse.NegativeX );
//			playerActions.LookRight.AddDefaultBinding( Mouse.PositiveX );

			playerActions.Menu.AddDefaultBinding( Key.Escape );
			playerActions.Menu.AddDefaultBinding( InputControlType.Pause );

			playerActions.ListenOptions.IncludeUnknownControllers = true;
			playerActions.ListenOptions.MaxAllowedBindings = 3;
			//			playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
			//			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;

			playerActions.ListenOptions.OnBindingFound = ( action, binding ) =>
			{
				if (binding == new KeyBindingSource( Key.Escape ))
				{
					action.StopListeningForBinding();
					return false;
				}
				return true;
			};

			playerActions.ListenOptions.OnBindingAdded += ( action, binding ) =>
			{
				Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
			};

			playerActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) =>
			{
				Debug.Log( "Binding rejected... " + reason );
			};

			return playerActions;
		}
	}

	static PlayerActions _input;
	public static PlayerActions input {
		get {
			if(_input == null)
				Debug.LogError("No instance of InputManager in scene.");
			return _input;
		}
	}
	//    public PlayerActions input;
	PlayerActions playerActions;
	string saveData;

	void OnEnable() {
		playerActions = PlayerActions.CreateWithDefaultBindings();
		LoadBindings();
		_input = playerActions;
	}

	void OnDisable() {
		// This properly disposes of the action set and unsubscribes it from
		// update events so that it doesn't do additional processing unnecessarily.
		playerActions.Destroy();
	}

	void SaveBindings() {
		saveData = playerActions.Save();
		PlayerPrefs.SetString( "Bindings", saveData );
	}

	void LoadBindings() {
		if (PlayerPrefs.HasKey( "Bindings" ))
		{
			saveData = PlayerPrefs.GetString( "Bindings" );
			playerActions.Load( saveData );
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}

	bool showInputBindings = false;
	void OnGUI() {
		if(!showInputBindings) {
			if(GUILayout.Button("Bindings")) {
				showInputBindings = !showInputBindings;
			}
		} else {
			if(GUILayout.Button("Hide")) {
				showInputBindings = !showInputBindings;
			}
			const float h = 22.0f;
			var y = 10.0f;

			GUI.Label( new Rect( 10, y, 300, y + h ), "Last Input Type: " + playerActions.LastInputType.ToString() );
			y += h;

			var actionCount = playerActions.Actions.Count;
			for (int i = 0; i < actionCount; i++)
			{
				var action = playerActions.Actions[i];

				var name = action.Name;
				if (action.IsListeningForBinding)
				{
					name += " (Listening)";
				}
				name += " = " + action.Value;
				GUI.Label( new Rect( 10, y, 300, y + h ), name );
				y += h;

				var bindingCount = action.Bindings.Count;
				for (int j = 0; j < bindingCount; j++)
				{
					var binding = action.Bindings[j];

					GUI.Label( new Rect( 45, y, 300, y + h ), binding.DeviceName + ": " + binding.Name );
					if (GUI.Button( new Rect( 20, y + 3.0f, 20, h - 5.0f ), "-" ))
					{
						action.RemoveBinding( binding );
					}
					y += h;
				}

				if (GUI.Button( new Rect( 20, y + 3.0f, 20, h - 5.0f ), "+" ))
				{
					action.ListenForBinding();
				}

				if (GUI.Button( new Rect( 50, y + 3.0f, 50, h - 5.0f ), "Reset" ))
				{
					action.ResetBindings();
				}

				y += 25.0f;
			}

			if (GUI.Button( new Rect( 20, y + 3.0f, 50, h ), "Load" ))
			{
				LoadBindings();
			}

			if (GUI.Button( new Rect( 80, y + 3.0f, 50, h ), "Save" ))
			{
				SaveBindings();
			}

			if (GUI.Button( new Rect( 140, y + 3.0f, 50, h ), "Reset" ))
			{
				playerActions.Reset();
			}
		}
	}
}
