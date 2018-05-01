using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessMenuController : MonoBehaviour
{
	#region Static

	private static EndlessMenuController instance;
	public static EndlessMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<EndlessMenuController>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[SerializeField]
	private Button backButton;

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
		HideEndlessMenu(true);
		backButton.onClick.AddListener(OnBackButtonClick);
		playButton.onClick.AddListener(OnPlayButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowEndlessMenu(bool instant = false)
	{
		HeaderController.I.ShowHeader();
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideEndlessMenu(bool instant = false)
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
		HeaderController.I.HideHeader();
		GameController.I.NewEndless(new List<TileElement.ElementType>
		{
			TileElement.ElementType.Cat,
			TileElement.ElementType.Chicken,
			TileElement.ElementType.Cow,
			TileElement.ElementType.Dog,
			TileElement.ElementType.Pig,
			TileElement.ElementType.Sheep,
		});
		HideEndlessMenu();
	}

	private void OnBackButtonClick()
	{
		HideEndlessMenu();
		MainMenuController.I.ShowMainMenu();
	}

	#endregion
}
