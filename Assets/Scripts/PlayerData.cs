

using UnityEngine;

public class PlayerData
{

	private const string HighScoreKey = "HighScoreKey";
	
	public static int HighScore
	{
		get { return PlayerPrefs.GetInt(HighScoreKey); }
		set { PlayerPrefs.SetInt(HighScoreKey, value);}
	}
	
}
