using System.Collections;
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
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[SerializeField]
	private Button endlessButton;

	[SerializeField]
	private Button settingsButton;

	#endregion

	#region Mono

	private void Awake()
	{
		settingsButton.onClick.AddListener(OnSetttingsButtonClick);
		endlessButton.onClick.AddListener(OnEndlessButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowMainMenu(bool instant = false)
	{
		HeaderController.I.HideHeader();
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			AudioController.I.InitializeBackgroundMusic();
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

	private void OnSetttingsButtonClick()
	{
		
	}

	private void OnEndlessButtonClick()
	{
		EndlessMenuController.I.ShowEndlessMenu();
		HideMainMenu();
	}

	#endregion
}
