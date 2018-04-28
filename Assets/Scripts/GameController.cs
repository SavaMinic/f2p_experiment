using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	#region Static

	private static GameController instance;
	public static GameController I
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

	private void Start()
	{
		CurrentState = GameState.MainMenu;
		
		// just start it immediately
		NewGame(10);
	}

	#endregion

	#region Public

	public void NewGame(int numOfElements = 6, List<TileElement.ElementType> possibleElements = null)
	{
		CurrentState = GameState.Playing;
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
	}

	#endregion
}
