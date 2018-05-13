using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInfoMenuController : MonoBehaviour
{
	#region Static

	private static AnimalInfoMenuController instance;
	public static AnimalInfoMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<AnimalInfoMenuController>();
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
	private Button closeButton;

	[SerializeField]
	private Button backButton;
	
	[SerializeField]
	private Image animalImage;

	[SerializeField]
	private Text animalNameText;

	[SerializeField]
	private Button customizeAnimalButton;

	[SerializeField]
	private Text currentLevelText;

	[SerializeField]
	private List<Text> currentLevelPointTexts;

	[SerializeField]
	private Text nextLevelText;

	[SerializeField]
	private List<Text> nextLevelPointTexts;

	[SerializeField]
	private Text levelUpCostText;

	[SerializeField]
	private Color canLevelUpColor;

	[SerializeField]
	private Color missingSoftCurrencyColor;

	[SerializeField]
	private Button levelUpButton;

	[SerializeField]
	private Text levelUpButtonText;

	private TileElement.ElementType animalType;

	#endregion

	#region Mono

	private void Awake()
	{
		HideAnimalInfo(true);
		backButton.onClick.AddListener(OnCloseButtonClick);
		closeButton.onClick.AddListener(OnCloseButtonClick);
		
		levelUpButton.onClick.AddListener(OnLevelUpButtonClick);
	}

	private void Start()
	{
		GameController.I.OnSoftCurrencyChanged += OnSoftCurrencyChanged;
	}

	#endregion

	#region Public

	public void ShowAnimalInfo(bool instant = false, TileElement.ElementType type = TileElement.ElementType.None)
	{
		animalType = type;
		RefreshView(type, GameController.I.SoftCurrency);
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideAnimalInfo(bool instant = false)
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

	private void OnCloseButtonClick()
	{
		HideAnimalInfo();
	}

	private void OnSoftCurrencyChanged(int newVal)
	{
		RefreshView(animalType, newVal);
	}

	private void OnLevelUpButtonClick()
	{
		var currLevel = AnimalData.AnimalLevel(animalType);
		var cost = GameSettings.I.GetSoftCurrencyForUpgrade(currLevel);
		if (GameController.I.SoftCurrency >= cost)
		{
			AnimalData.IncreaseAnimalLevel(animalType);
			GameController.I.SoftCurrency -= cost;
		}
	}

	#endregion

	#region Private

	private void RefreshView(TileElement.ElementType type, int softCurrency)
	{
		if (type == TileElement.ElementType.None)
			return;

		animalImage.sprite = GameSettings.I.GetSpriteForElement(type);
		animalNameText.text = type.ToString().ToUpper();

		var currLevel = AnimalData.AnimalLevel(type);
		currentLevelText.text = "Lvl " + (currLevel + 1);

		var nextLevel = currLevel + 1;
		nextLevelText.text = "Lvl " + (nextLevel + 1);

		var currLevelBasePoints = GameSettings.I.GetBasePointsForElementOfLevel(currLevel);
		var nextLevelBasePoints = GameSettings.I.GetBasePointsForElementOfLevel(nextLevel);
		for (int i = 0; i < currentLevelPointTexts.Count; i++)
		{
			var currLevelPoints = (4 + i) * currLevelBasePoints * (i + 1);
			var nextLevelPoints = (4 + i) * nextLevelBasePoints * (i + 1);
			currentLevelPointTexts[i].text = (4 + i) + " line: " + currLevelPoints + "pts";
			nextLevelPointTexts[i].text = "+ " + (nextLevelPoints - currLevelPoints);
		}

		var cost = GameSettings.I.GetSoftCurrencyForUpgrade(currLevel);
		levelUpCostText.text = softCurrency + "/" + cost;
		var canLevelUp = softCurrency >= cost;
		levelUpCostText.color = canLevelUp ? canLevelUpColor : missingSoftCurrencyColor;
		levelUpButton.interactable = canLevelUp;
		levelUpButtonText.color = levelUpButtonText.color.GetWithAlpha(canLevelUp ? 1f : 0.3f);
	}

	#endregion
}
