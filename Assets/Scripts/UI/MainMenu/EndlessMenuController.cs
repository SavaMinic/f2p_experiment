using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using PlayFab.ClientModels;
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

	[Header("Navigation")]
	[SerializeField]
	private List<Button> navigationButtons;
	
	[SerializeField]
	private List<Image> navigationButtonImages;

	[SerializeField]
	private List<CanvasGroup> tabCanvasGroups;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[SerializeField]
	private Color defaultTabColor;

	[SerializeField]
	private Color activeTabColor;

	[Header("Main panel")]
	[SerializeField]
	private RectTransform mainPanel;
	
	[SerializeField]
	private Button playButton;

	[SerializeField]
	private Text highscoreText;
	
	[SerializeField]
	private Button freeGiftButton;

	[SerializeField]
	private Text freeGiftText;

	[SerializeField]
	private Text freeGiftRewardText;

	[Header("Animal info")]
	[SerializeField]
	private List<AnimalInfoPanel> animalInfoPanels;

	[Header("Rankings")]
	[SerializeField]
	private RectTransform rankingsPanel;
	
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

	[SerializeField]
	private RankingView myPlayerRankingView;

	[SerializeField]
	private Text loadingRankingsText;

	[SerializeField]
	private CanvasGroup rankingContentCanvasGroup;

	[Header("Shop")]
	[SerializeField]
	private RectTransform shopPanel;

	public enum MenuTabs
	{
		Ranking,
		Home,
		Shop
	}
	private int currentlyShownTab = -1;
	private GoTween rankingsShowTween;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideEndlessMenu(true);
		playButton.onClick.AddListener(OnPlayButtonClick);
		freeGiftButton.onClick.AddListener(OnFreeGiftButtonClick);

		for (int i = 0; i < rankingsButtons.Count; i++)
		{
			var ii = i;
			rankingsButtons[i].onClick.AddListener(() =>
			{
				OnRankingButtonClick(ii);
			});
		}

		for (int i = 0; i < navigationButtons.Count; i++)
		{
			var ii = i;
			navigationButtons[i].onClick.AddListener(() =>
			{
				OnNavigationButtonClick(ii);
			});
		}
	}

	private void Update()
	{
		if (!Application.isPlaying)
			return;

		var canClaim = PlayerData.CanClaimFreeGift;
		freeGiftButton.interactable = canClaim;
		freeGiftRewardText.text = GameSettings.I.FreeGiftTotalAmount.ToString();
		if (canClaim)
		{
			freeGiftText.text = "CLAIM";
			freeGiftText.color = freeGiftText.color.GetWithAlpha(1f);
			freeGiftRewardText.color = freeGiftRewardText.color.GetWithAlpha(1f);
		}
		else
		{
			var remaining = (float) PlayerData.FreeGiftClaimTimeRemaining.TotalSeconds;
			freeGiftText.text = "Ready in\n" + remaining.FormatToTime();
			freeGiftText.color = freeGiftText.color.GetWithAlpha(0.4f);
			freeGiftRewardText.color = freeGiftRewardText.color.GetWithAlpha(0.4f);
		}
	}

	#endregion
	
	#region Public

	public void ShowEndlessMenu(bool instant = false)
	{
		HeaderController.I.ShowHeader();
		highscoreText.text = PlayerData.HighScore.ToString();

		for (int i = 0; i < animalInfoPanels.Count; i++)
		{
			animalInfoPanels[i].Refresh((TileElement.ElementType)(i + 1));
		}

		ShowCanvasGroup(mainCanvasGroup);
		ShowTab(MenuTabs.Home);
	}

	public void HideEndlessMenu(bool instant = false)
	{
		currentlyShownTab = -1;
		HideCanvasGroup(mainCanvasGroup, instant);
	}

	public void ShowTab(MenuTabs tab)
	{
		if (currentlyShownTab == (int)tab)
			return;

		if (currentlyShownTab != -1)
		{
			HideCanvasGroup(tabCanvasGroups[currentlyShownTab]);
		}
		else
		{
			for (int i = 0; i < tabCanvasGroups.Count; i++)
			{
				if (i != (int) tab)
				{
					HideCanvasGroup(tabCanvasGroups[i], true);
				}
			}
		}

		if (currentlyShownTab != (int) tab)
		{
			switch (tab)
			{
				case MenuTabs.Ranking:
					ShowRankingsTab(0);
					break;
			}
		}
		
		currentlyShownTab = (int)tab;
		ShowCanvasGroup(tabCanvasGroups[currentlyShownTab]);

		for (int i = 0; i < navigationButtonImages.Count; i++)
		{
			navigationButtonImages[i].color = i == currentlyShownTab ? activeTabColor : defaultTabColor;
		}
	}
	
	#endregion

	#region Events

	private void OnNavigationButtonClick(int index)
	{
		ShowTab((MenuTabs)index);
	}

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

	private void OnFreeGiftButtonClick()
	{
		GameController.I.SoftCurrency += GameSettings.I.FreeGiftTotalAmount;
		PlayerData.SetupFreeGiftClaimTime();
		AnalyticsData.TrackFreeGiftClaimed();
	}

	#endregion

	#region Private

	private void ShowCanvasGroup(CanvasGroup group, bool instant = false)
	{
		group.interactable = group.blocksRaycasts = true;
		if (instant)
		{
			group.alpha = 1f;
			return;
		}
		Go.to(group, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	private void HideCanvasGroup(CanvasGroup group, bool instant = false)
	{
		group.interactable = group.blocksRaycasts = false;
		if (instant)
		{
			group.alpha = 0f;
			return;
		}
		Go.to(group, fadeDuration, new GoTweenConfig().floatProp("alpha", 0f));
	}

	private void ShowRankingsTab(int index)
	{
		for (int i = 0; i < rankingsButtons.Count; i++)
		{
			rankingsButtons[i].GetComponent<Image>().color = index == i ? rankingButtonActiveColor : rankingButtonDefaultColor;
		}
		
		loadingRankingsText.gameObject.SetActive(true);
		if (rankingsShowTween != null && rankingsShowTween.state == GoTweenState.Running)
		{
			rankingsShowTween.destroy();
		}
		rankingContentCanvasGroup.alpha = 0f;
		RankingsData.GetRankings(index, ShowRankingContent);
	}

	private void ShowRankingContent(List<PlayerLeaderboardEntry> rankings, PlayerLeaderboardEntry myPlayerRanking)
	{
		if (rankings == null)
			return;
		
		loadingRankingsText.gameObject.SetActive(false);
		rankingsShowTween = Go.to(rankingContentCanvasGroup, 0.4f, new GoTweenConfig().floatProp("alpha", 1f));
		
		for (int i = 0; i < playerRankingViews.Count; i++)
		{
			if (i < rankings.Count)
			{
				playerRankingViews[i].gameObject.SetActive(true);
				playerRankingViews[i].Refresh(rankings[i]);
			}
			else
			{
				playerRankingViews[i].gameObject.SetActive(false);
			}
		}

		myPlayerRankingView.Refresh(myPlayerRanking);

		var h = 0f;
		if (rankings.Count > 0)
		{
			h = playerRankingViews[0].Height * rankings.Count + rankingViewsLayoutGroup.spacing * (rankings.Count - 1);
		}
		rankingViewsContainer.sizeDelta = new Vector2(rankingViewsContainer.sizeDelta.x, h);
		rankingScrollRect.verticalNormalizedPosition = 1;
	}

	#endregion
}
