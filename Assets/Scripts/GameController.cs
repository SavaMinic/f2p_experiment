using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	#region Static

	private static GameController instance;
	public static GameController Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<GameController>();
			return instance;
		}
	}

	#endregion
	
	#region Enums

	public enum GameState
	{
		MainMenu,
		Playing,
		Paused,
		EndScreen
	}
	
	#endregion

	#region Fields/Properties

	public GameState CurrentState { get; private set; }

	#endregion

	#region Mono methods

	private void Awake()
	{
		CurrentState = GameState.Playing;
	}

	#endregion
}
