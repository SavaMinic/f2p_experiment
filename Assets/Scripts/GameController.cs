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

	public Action<int> OnSoftCurrencyChanged;
	public Action<int> OnHardCurrencyChanged;

	public Action OnMilestoneChanged;

	#endregion

	#region Fields/Properties

	public GameState CurrentState { get; private set; }
	public bool IsPlaying
	{
		get { return CurrentState == GameState.Playing; }
	}
	
	public GameModeType GameMode { get; private set; }
	
	public int GameTurn { get; private set; }
	public float GameTime { get; private set; }
	
	public int TargetScore { get; private set; }
	public bool HasTargetScore { get { return TargetScore > 0; } }
	
	public int TurnsLimit { get; private set; }
	public bool IsTurnsMode { get { return TurnsLimit != -1; } }
	
	public float TimeLimit { get; private set; }
	public bool IsTimeLimitMode { get { return !TimeLimit.Approximately(-1f); } }
	
	public int CurrentLevel { get; private set; }
	
	public int CurrentMilestone { get; private set; }
	public int MilestonePoint
	{
		get
		{
			var i = Mathf.Clamp(CurrentMilestone, 0, GameSettings.I.MilestonePoints.Count - 1);
			return Mathf.RoundToInt(GameSettings.I.MilestonePoints[i] * AnimalData.AverageAnimalLevel);
		}
	}
	public int MilestoneReward
	{
		get
		{
			var i = Mathf.Clamp(CurrentMilestone, 0, GameSettings.I.MilestoneRewards.Count - 1);
			return GameSettings.I.MilestoneRewards[i];
		}
	}

	private int score = -1;
	public int Score
	{
		get { return score; }
		set
		{
			if (score == value)
				return;

			if (HasTargetScore)
			{
				value = Mathf.Min(value, TargetScore);
			}
			
			OnScoreChanged.CallIfNotNull(value);
			score = value;
		}
	}

	public int SoftCurrency
	{
		get { return PlayerData.SoftCurrency; }
		set
		{
			if (PlayerData.SoftCurrency == value)
				return;
			
			OnSoftCurrencyChanged.CallIfNotNull(value);
			PlayerData.SoftCurrency = value;
		}
	}

	public int HardCurrency
	{
		get { return PlayerData.HardCurrency; }
		set
		{
			if (PlayerData.HardCurrency == value)
				return;
			
			OnHardCurrencyChanged.CallIfNotNull(value);
			PlayerData.HardCurrency = value;
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

	private void Update()
	{
		if (!Application.isPlaying || !IsPlaying)
			return;

		if (IsPlaying)
		{
			GameTime += Time.deltaTime / Time.timeScale;
		}

		if (IsTimeLimitMode)
		{
			TimeLimit -= Time.deltaTime / Time.timeScale;
			if (TimeLimit <= 0f)
			{
				EndGame(EndGameType.TimeLimit);
			}
		}
	}

	#endregion

	#region Public

	public void NewEndless(List<TileElement.ElementType> possibleElements)
	{
		Score = 0;
		GameMode = GameModeType.Endless;
		TargetScore = -1;
		TurnsLimit = -1;
		TimeLimit = -1;

		StartGame(GameSettings.I.DefaultStartingCount, possibleElements);
	}

	public void NewGame(int level, List<TileElement.ElementType> possibleElements)
	{
		CurrentLevel = level;
		Score = 0;
		
		// TODO: make this much smarter

		var difficulty = level / 8;
		
		GameMode = GameModeType.TargetScore;
		
		var numOfElements = GameSettings.I.DefaultStartingCount + difficulty;
		TargetScore = GameSettings.I.DefaultTargetScore + Mathf.FloorToInt(level / 4f) * 100;

		if (level % 4 == 1)
		{
			TurnsLimit = GameSettings.I.DefaultTurnsLimit - difficulty * 2;
			TimeLimit = -1;
		}
		else
		{
			TurnsLimit = -1;
			if (level % 5 == 2)
			{
				TimeLimit = GameSettings.I.DefaultTimeLimit - difficulty * 15;
			}
			else
			{
				TimeLimit = -1;
			}
		}

		StartGame(numOfElements, possibleElements);
	}

	public void JustCheckForWin()
	{
		if (CheckForWin())
		{
			EndGame(EndGameType.Win);
		}

	}

	public void NewTurn(bool isNormalTurn)
	{
		if (CheckForWin())
		{
			EndGame(EndGameType.Win);
			return;
		}
		
		if (isNormalTurn)
		{
			GameTurn++;
			if (IsTurnsMode)
			{
				if (--TurnsLimit == 0)
				{
					EndGame(EndGameType.TurnLimit);
					return;
				}
			}
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
		if (endType == EndGameType.Win)
		{
			if (GameMode != GameModeType.Endless)
			{
				PlayerData.FinishLevel(CurrentLevel);
			}
		}
		CurrentState = GameState.EndScreen;
		EndMenuController.I.ShowEndMenu(endType);
	}

	public void ExitToMainMenu()
	{
		CurrentState = GameState.MainMenu;
		EndlessMenuController.I.ShowEndlessMenu(true);
	}

	public void NextMilestone()
	{
		SoftCurrency += MilestoneReward;
		CurrentMilestone++;
		OnMilestoneChanged.CallIfNotNull();
	}
	
	public float MilestoneProgress(int score)
	{
		var i = Mathf.Clamp(CurrentMilestone, 0, GameSettings.I.MilestonePoints.Count - 1);
		var previous = CurrentMilestone > 0 ? GameSettings.I.MilestonePoints[i - 1] * AnimalData.AverageAnimalLevel : 0f;
		var current = GameSettings.I.MilestonePoints[i] * AnimalData.AverageAnimalLevel;
		return (score - previous) / (current - previous);
	}

	#endregion

	#region Private

	private void StartGame(int numOfElements, List<TileElement.ElementType> possibleElements)
	{
		GameTurn = 0;
		GameTime = 0f;
		CurrentMilestone = 0;
		
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
		ElementGenerator.I.SetPossibleTypes(possibleElements);
		GoalsView.I.Initialize();
		
		NewTurn(false);
		CurrentState = GameState.Playing;
	}

	private bool CheckForWin()
	{
		if (GameMode == GameModeType.TargetScore && HasTargetScore)
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
