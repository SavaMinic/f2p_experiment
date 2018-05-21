using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonScriptableObject<GameSettings>
{
	public List<Sprite> elementSprites;

	public Sprite GetSpriteForElement(TileElement.ElementType type)
	{
		return elementSprites[(int) type - 1];
	}

	public int StartingSoftCurrency = 100;
	public int StartingHardCurrency = 50;

	public int DefaultStartingCount = 10;

	public int DefaultTargetScore = 100;
	public int DefaultTurnsLimit = 30;
	public int DefaultTimeLimit = 300;

	public List<int> SoftCurrencyForAnimalUpgrade;

	public int GetSoftCurrencyForUpgrade(int currLevel)
	{
		currLevel = Mathf.Clamp(currLevel, 0, SoftCurrencyForAnimalUpgrade.Count - 1);
		return SoftCurrencyForAnimalUpgrade[currLevel];
	}

	public List<int> BasePointsPerElementOfLevel;
	public int GetBasePointsForElementOfLevel(int currLevel)
	{
		currLevel = Mathf.Clamp(currLevel, 0, BasePointsPerElementOfLevel.Count - 1);
		return BasePointsPerElementOfLevel[currLevel];
	}

	public List<int> MilestonePoints;
	public List<int> MilestoneRewards;

	public int FreeGiftTimeInMinutes;
	public int FreeGiftBaseAmount;

	public int FreeGiftTotalAmount
	{
		get { return Mathf.RoundToInt(FreeGiftBaseAmount * AnimalData.AverageAnimalLevel); }
	}

	[Header("PLAYFAB")]
	public string PlayFabGameId = "7A4C";

}
