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

	private void Awake()
	{
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		CurrentState = GameState.MainMenu;
		
		// just start it immediately
		NewGame(10, new List<TileElement.ElementType>
		{
			TileElement.ElementType.Cat,
			TileElement.ElementType.Chicken,
			TileElement.ElementType.Cow,
			TileElement.ElementType.Dog,
			TileElement.ElementType.Pig,
			TileElement.ElementType.Sheep,
		});
	}

	#endregion

	#region Public

	public void NewGame(int numOfElements, List<TileElement.ElementType> possibleElements)
	{
		CurrentState = GameState.Playing;
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
		ElementGenerator.I.SetPossibleTypes(possibleElements);
		ElementGenerator.I.GetCurrentSequenceAndGenerateNew();
	}

	public void NewTurn()
	{
		var sequence = ElementGenerator.I.GetCurrentSequenceAndGenerateNew();
		TileController.I.AddNewElements(sequence);
	}

	#endregion
}
