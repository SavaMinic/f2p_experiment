using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{

	private const string AnimalLevelData = "AnimalLevel_";

	public static int AnimalLevel(TileElement.ElementType type)
	{
		if (!PlayerPrefs.HasKey(AnimalLevelData + type))
		{
			PlayerPrefs.SetInt(AnimalLevelData + type, 0);	
		}
		return PlayerPrefs.GetInt(AnimalLevelData + type);
	}

	public static void IncreaseAnimalLevel(TileElement.ElementType type)
	{
		var level = AnimalLevel(type);
		PlayerPrefs.SetInt(AnimalLevelData + type, level + 1);
	}

	public static void ResetAnimalLevels()
	{
		foreach (var type in Enum.GetValues(typeof(TileElement.ElementType)))
		{
			PlayerPrefs.SetInt(AnimalLevelData + type, 0);
		}
	}
	
}
