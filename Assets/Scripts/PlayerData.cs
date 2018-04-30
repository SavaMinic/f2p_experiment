

using UnityEngine;

public class PlayerData
{

	private const string HighScoreKey = "HighScoreKey";
	private const string LevelFinishedKey = "LevelFinished";
	
	public static int HighScore
	{
		get { return PlayerPrefs.GetInt(HighScoreKey); }
		set { PlayerPrefs.SetInt(HighScoreKey, value);}
	}
	
	public static void FinishLevel(int level)
	{
		PlayerPrefs.SetInt(LevelFinishedKey + level, 1);
	}

	public static bool HasFinishedLevel(int level)
	{
		var key = LevelFinishedKey + level;
		return PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) == 1;
	}
	
}
