

using UnityEngine;

public class PlayerData
{

	private const string PlayerIdKey = "PlayerId";
	
	private const string HighScoreKey = "HighScoreKey";
	private const string LevelFinishedKey = "LevelFinished_";

	private const string SoftCurrencyKey = "SoftCurrencyKey";
	private const string HardCurrencyKey = "HardCurrencyKey";
	
	public static int HighScore
	{
		get { return PlayerPrefs.GetInt(HighScoreKey); }
		set { PlayerPrefs.SetInt(HighScoreKey, value);}
	}

	public static int PlayerId
	{
		get
		{
			// TODO: should get it somehow unique
			if (!PlayerPrefs.HasKey(PlayerIdKey))
			{
				PlayerPrefs.SetInt(PlayerIdKey, 666);
			}
			return PlayerPrefs.GetInt(PlayerIdKey);
		}
	}
	
	public static int SoftCurrency
	{
		get {
			if (!PlayerPrefs.HasKey(SoftCurrencyKey))
			{
				PlayerPrefs.SetInt(SoftCurrencyKey, GameSettings.I.StartingSoftCurrency);
			}
			return PlayerPrefs.GetInt(SoftCurrencyKey);
		}
		set { PlayerPrefs.SetInt(SoftCurrencyKey, value);}
	}
	
	public static int HardCurrency
	{
		get {
			if (!PlayerPrefs.HasKey(HardCurrencyKey))
			{
				PlayerPrefs.SetInt(HardCurrencyKey, GameSettings.I.StartingHardCurrency);
			}
			return PlayerPrefs.GetInt(HardCurrencyKey);
		}
		set { PlayerPrefs.SetInt(HardCurrencyKey, value);}
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

	public static void ResetPlayerLevelsFinished()
	{
		for (int level = 0; level < 1000; level++)
		{
			if (HasFinishedLevel(level))
			{
				PlayerPrefs.DeleteKey(LevelFinishedKey + level);
			}
		}
	}
	
}
