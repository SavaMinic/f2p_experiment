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

}
