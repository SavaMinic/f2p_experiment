using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenuController : MonoBehaviour
{

	#region Static

	private static LevelSelectionMenuController instance;
	public static LevelSelectionMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<LevelSelectionMenuController>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[SerializeField]
	private Button backButton;

	[SerializeField]
	private List<Button> levelButtons;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[SerializeField]
	private Color normalLevelColor;

	[SerializeField]
	private Color finishedLevelColor;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideLevelSelectionMenu(true);
		backButton.onClick.AddListener(OnBackButtonClick);

		for (int i = 0; i < levelButtons.Count; i++)
		{
			var ii = i;
			levelButtons[i].onClick.AddListener(() =>
			{
				OnLevelButtonClick(ii);
			});
			levelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
		}
	}

	#endregion
	
	#region Public

	public void ShowLevelSelectionMenu(bool instant = false)
	{
		HeaderController.I.ShowHeader();
		RefreshLevelColors();
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideLevelSelectionMenu(bool instant = false)
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

	private void OnLevelButtonClick(int level)
	{
		HeaderController.I.HideHeader();
		GameController.I.NewGame(level, new List<TileElement.ElementType>
		{
			TileElement.ElementType.Cat,
			TileElement.ElementType.Chicken,
			TileElement.ElementType.Cow,
			TileElement.ElementType.Dog,
			TileElement.ElementType.Pig,
			TileElement.ElementType.Sheep,
		});
		HideLevelSelectionMenu();
	}

	private void OnBackButtonClick()
	{
		HideLevelSelectionMenu();
		MainMenuController.I.ShowMainMenu();
	}

	#endregion

	#region Private

	private void RefreshLevelColors()
	{
		for (int i = 0; i < levelButtons.Count; i++)
		{
			levelButtons[i].GetComponent<Image>().color = PlayerData.HasFinishedLevel(i) ? finishedLevelColor : normalLevelColor;
		}
	}

	#endregion
}
