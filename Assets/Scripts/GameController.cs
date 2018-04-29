﻿using System;
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
		TileController.I.GenerateNewMap(numOfElements, possibleElements);
		ElementGenerator.I.SetPossibleTypes(possibleElements);
		
		NewTurn(false);
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
}
