﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	
	#region Static

	private static MainMenuController instance;
	public static MainMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<MainMenuController>();
			return instance;
		}
	}

	#endregion

	#region Fields

	[SerializeField]
	private Button playButton;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	#endregion

	#region Mono

	private void Awake()
	{
		playButton.onClick.AddListener(OnPlayButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowMainMenu(bool instant = false)
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideMainMenu(bool instant = false)
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = false;
		if (instant)
		{
			mainCanvasGroup.alpha = 0f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 0f));
	}
	
	#endregion

	#region Events

	private void OnPlayButtonClick()
	{
		// just start it immediately
		GameController.I.NewGame(GameSettings.I.DefaultStartingCount, new List<TileElement.ElementType>
		{
			TileElement.ElementType.Cat,
			TileElement.ElementType.Chicken,
			TileElement.ElementType.Cow,
			TileElement.ElementType.Dog,
			TileElement.ElementType.Pig,
			TileElement.ElementType.Sheep,
		});
		HideMainMenu();
	}

	#endregion
}
