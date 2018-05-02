using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInfoPanel : MonoBehaviour
{

	#region Fields

	[SerializeField]
	private Button myButton;

	[SerializeField]
	private Image animalImage;

	[SerializeField]
	private Text animalNameText;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private Image backImage;

	[SerializeField]
	private Color normalColor;

	[SerializeField]
	private Color upgradeableColor;

	private TileElement.ElementType animalType;

	#endregion

	#region Mono

	private void Awake()
	{
		myButton.onClick.AddListener(OnMyButtonClick);

		GameController.I.OnSoftCurrencyChanged += RefreshUpgradableState;
	}

	#endregion

	#region Public

	public void Refresh(TileElement.ElementType type)
	{
		animalType = type;
		animalImage.sprite = GameSettings.I.GetSpriteForElement(type);
		animalNameText.text = type.ToString().ToUpper();
		levelText.text = "Lvl. " + (AnimalData.AnimalLevel(type) + 1);
		RefreshUpgradableState(GameController.I.SoftCurrency);
	}

	#endregion

	#region Events

	private void OnMyButtonClick()
	{
		
	}

	private void RefreshUpgradableState(int newVal)
	{
		var level = AnimalData.AnimalLevel(animalType);
		var cost = GameSettings.I.GetSoftCurrencyForUpgrade(level);
		backImage.color = newVal >= cost ? upgradeableColor : normalColor;
	}

	#endregion
}
