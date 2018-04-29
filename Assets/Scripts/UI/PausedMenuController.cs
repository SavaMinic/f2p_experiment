using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedMenuController : MonoBehaviour
{
	#region Static

	private static PausedMenuController instance;
	public static PausedMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<PausedMenuController>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[SerializeField]
	private Button backButton;

	[SerializeField]
	private Button continueButton;

	[SerializeField]
	private Button optionsButton;

	[SerializeField]
	private Button exitButton;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HidePauseMenu(true);
		continueButton.onClick.AddListener(OnContinueButtonClick);
		backButton.onClick.AddListener(OnContinueButtonClick);
		optionsButton.onClick.AddListener(OnOptionsButtonClick);
		exitButton.onClick.AddListener(OnExitButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowPauseMenu(bool instant = false)
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HidePauseMenu(bool instant = false)
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

	private void OnContinueButtonClick()
	{
		HidePauseMenu();
		GameController.I.UnpauseGame();
	}
	
	private void OnOptionsButtonClick()
	{
		// TODO: implement this
	}
	
	private void OnExitButtonClick()
	{
		HidePauseMenu();
		GameController.I.ExitGame();
	}

	#endregion
}
