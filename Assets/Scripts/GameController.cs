using System;
using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
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

	#region Actions

	public Action OnNewTurn;
	public Action<int> OnScoreChanged;

	#endregion

	#region Fields/Properties

	public GameState CurrentState { get; private set; }
	public bool IsPlaying
	{
		get { return CurrentState == GameState.Playing; }
	}

	private int score;
	public int Score
	{
		get { return score; }
		set
		{
			if (score == value)
				return;
			
			OnScoreChanged.CallIfNotNull(value);
			score = value;
		}
	}

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
		Score = 0;
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
		ElementGenerator.I.SetPossibleTypes(possibleElements);
		
		NewTurn(true);
		CurrentState = GameState.Playing;
	}

	public void NewTurn(bool generateNewSequence)
	{
		if (generateNewSequence)
		{
			var sequence = ElementGenerator.I.GetCurrentSequenceAndGenerateNew();
			TileController.I.AddNewElements(sequence);
		}
		OnNewTurn.CallIfNotNull();
	}

	#endregion
}
