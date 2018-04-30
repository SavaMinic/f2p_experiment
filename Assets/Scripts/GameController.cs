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

	public enum EndGameType
	{
		NoEmptySpace,
		Manual,
		TurnLimit,
		TimeLimit,
		Win
	}

	public enum GameModeType
	{
		TargetScore,
		Endless,
		Collections
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
	
	public GameModeType GameMode { get; private set; }
	
	public int TargetScore { get; private set; }
	public bool HasTargetScore { get { return TargetScore > 0; } }
	
	public int TurnsLimit { get; private set; }
	public bool IsTurnsMode { get { return TurnsLimit != -1; } }
	
	public float TimeLimit { get; private set; }
	public bool IsTimeLimitMode { get { return !TimeLimit.Approximately(-1f); } }

	private int score = -1;
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
		MainMenuController.I.ShowMainMenu(true);
	}

	#endregion

	#region Public

	public void NewGame(int numOfElements, List<TileElement.ElementType> possibleElements)
	{
		Score = 0;
		GameMode = GameModeType.TargetScore;
		TargetScore = 100;
		TurnsLimit = -1;
		TimeLimit = -1;
		
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
		ElementGenerator.I.SetPossibleTypes(possibleElements);
		GoalsView.I.Initialize();
		
		NewTurn(false);
		CurrentState = GameState.Playing;
	}

	public void NewTurn(bool generateNewSequence)
	{
		if (CheckForWin())
		{
			EndGame(EndGameType.Win);
			return;
		}
		
		if (generateNewSequence)
		{
			var sequence = ElementGenerator.I.GetCurrentSequenceAndGenerateNew();
			TileController.I.AddNewElements(sequence);
		}
		OnNewTurn.CallIfNotNull();
	}

	public void PauseGame()
	{
		CurrentState = GameState.Paused;
	}

	public void UnpauseGame()
	{
		CurrentState = GameState.Playing;
	}

	public void EndGame(EndGameType endType)
	{
		CurrentState = GameState.EndScreen;
		EndMenuController.I.ShowEndMenu(endType);
	}

	public void ExitToMainMenu()
	{
		CurrentState = GameState.MainMenu;
		MainMenuController.I.ShowMainMenu(true);
	}

	#endregion

	#region Private

	private bool CheckForWin()
	{
		if (GameMode == GameModeType.TargetScore)
		{
			return Score >= TargetScore;
		}

		if (GameMode == GameModeType.Collections)
		{
			// TODO: add collections
		}
		
		// Endless doesn't have win condition
		return false;
	}

	#endregion
}
