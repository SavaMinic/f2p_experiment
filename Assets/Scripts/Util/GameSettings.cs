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

	public int DefaultStartingCount = 10;
}
