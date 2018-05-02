using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorUtil : MonoBehaviour {

	[MenuItem("Farm Lines/Reset highscore")]
	static void ResetPlayerHighScore()
	{
		if (EditorUtility.DisplayDialog("Reset High score?",
			"Are you sure you want to reset player high score?", "Yes", "No"))
		{
			PlayerData.HighScore = 0;
		}
	}
	
	[MenuItem("Farm Lines/Reset level progress")]
	static void ResetPlayerLevelsFinished()
	{
		if (EditorUtility.DisplayDialog("Reset levels progress?",
			"Are you sure you want to reset player's level progress?", "Yes", "No"))
		{
			PlayerData.ResetPlayerLevelsFinished();
		}
	}
	
	[MenuItem("Farm Lines/Reset currencies")]
	static void ResetCurrencies()
	{
		if (EditorUtility.DisplayDialog("Reset currencies?",
			"Are you sure you want to reset player's currencies?", "Yes", "No"))
		{
			PlayerData.SoftCurrency = GameSettings.I.StartingSoftCurrency;
			PlayerData.HardCurrency = GameSettings.I.StartingHardCurrency;
		}
	}
	
}
