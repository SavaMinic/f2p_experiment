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
	private Button playButton;

	[SerializeField]
	private Text highscoreText;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[Header("Animal info")]
	[SerializeField]
	private List<AnimalInfoPanel> animalInfoPanels;

	[Header("Rankings")]
	[SerializeField]
	private List<Button> rankingsButtons;
	
	[SerializeField]
	private Color rankingButtonDefaultColor;

	[SerializeField]
	private Color rankingButtonActiveColor;

	[SerializeField]
	private ScrollRect rankingScrollRect;

	[SerializeField]
	private VerticalLayoutGroup rankingViewsLayoutGroup;

	[SerializeField]
	private RectTransform rankingViewsContainer;

	[SerializeField]
	private List<RankingView> playerRankingViews;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideEndlessMenu(true);
		playButton.onClick.AddListener(OnPlayButtonClick);

		for (int i = 0; i < rankingsButtons.Count; i++)
		{
			var ii = i;
			rankingsButtons[i].onClick.AddListener(() =>
			{
				OnRankingButtonClick(ii);
			});
		}
	}

	#endregion
	
	#region Public

	public void ShowEndlessMenu(bool instant = false)
	{
		HeaderController.I.ShowHeader();
		ShowRankingsTab(0);
		highscoreText.text = PlayerData.HighScore.ToString();

		for (int i = 0; i < animalInfoPanels.Count; i++)
		{
			animalInfoPanels[i].Refresh((TileElement.ElementType)(i + 1));
		}
		
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

	private void OnRankingButtonClick(int index)
	{
		ShowRankingsTab(index);
	}

	private void OnPlayButtonClick()
	{
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

	#endregion

	#region Private

	private void ShowRankingsTab(int index)
	{
		for (int i = 0; i < rankingsButtons.Count; i++)
		{
			rankingsButtons[i].GetComponent<Image>().color = index == i ? rankingButtonActiveColor : rankingButtonDefaultColor;
		}
		
		List<RankingsData.PlayerRanking> rankings = null;
		if (index == 0)
		{
			rankings = RankingsData.GetFriendsRankings();
		}
		else if (index == 1)
		{
			rankings = RankingsData.GetWorldRankings();
		}
		else
		{
			rankings = RankingsData.GetLocalRankings();
		}

		for (int i = 0; i < playerRankingViews.Count; i++)
		{
			if (rankings != null && i < rankings.Count)
			{
				playerRankingViews[i].gameObject.SetActive(true);
				playerRankingViews[i].Refresh(i + 1, rankings[i]);
			}
			else
			{
				playerRankingViews[i].gameObject.SetActive(false);
			}
		}

		var h = 0f;
		if (rankings != null && rankings.Count > 0)
		{
			h = playerRankingViews[0].Height * rankings.Count + rankingViewsLayoutGroup.spacing * (rankings.Count - 1);
		}
		rankingViewsContainer.sizeDelta = new Vector2(rankingViewsContainer.sizeDelta.x, h);
		rankingScrollRect.verticalNormalizedPosition = 0;
	}

	#endregion
}
