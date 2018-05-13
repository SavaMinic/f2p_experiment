

using System;
using UnityEngine;

public class PlayerData
{

	private const string PlayerIdKey = "PlayerId";
	
	private const string HighScoreKey = "HighScoreKey";

	private const string SoftCurrencyKey = "SoftCurrencyKey";

	private const string FreeGiftTimeKey = "FreeGiftTimeKey";
	
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

	public static TimeSpan FreeGiftClaimTimeRemaining
	{
		get
		{
			if (!PlayerPrefs.HasKey(FreeGiftTimeKey))
			{
				SetupFreeGiftClaimTime();
			}
			var diff = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString(FreeGiftTimeKey)));
			return diff.Subtract(DateTime.Now);
		}
	}

	public static void SetupFreeGiftClaimTime()
	{
		var time = DateTime.Now.AddMinutes(GameSettings.I.FreeGiftTimeInMinutes);
		PlayerPrefs.SetString(FreeGiftTimeKey, time.ToBinary().ToString());
	}

	public static bool CanClaimFreeGift
	{
		get
		{
			if (!PlayerPrefs.HasKey(FreeGiftTimeKey))
				return true;

			return FreeGiftClaimTimeRemaining.TotalSeconds <= 0;
		}
	}

	public static void ResetFreeGiftTimer()
	{
		PlayerPrefs.SetString(FreeGiftTimeKey, DateTime.Now.ToBinary().ToString());
	}
	
}
