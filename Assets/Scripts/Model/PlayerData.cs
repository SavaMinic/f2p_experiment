

using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerData
{

	private const string PlayerIdKey = "PlayerId";
	
	private const string HighScoreKey = "HighScoreKey";

	private const string SoftCurrencyKey = "SoftCurrencyKey";

	private const string FreeGiftTimeKey = "FreeGiftTimeKey";

	public static void RefreshUserStatistics(Action onSuccess)
	{
		var request = new GetPlayerStatisticsRequest();
		PlayFabClientAPI.GetPlayerStatistics(request,
			playerStats =>
			{
				var highScore = HighScore;
				var score = playerStats.Statistics.Find(s => s.StatisticName == HighScoreKey);
				if (score != null)
				{
					highScore = score.Value;
				}
				PlayerPrefs.SetInt(HighScoreKey, highScore);
				onSuccess();
			}, error =>
			{
				Debug.LogWarning("Something went wrong with RefreshUserStatistics :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}

	public static void RefreshUserData(Action onSuccess)
	{
		
	}
	
	#region Highscore

	public static int HighScore
	{
		get { return PlayerPrefs.GetInt(HighScoreKey, 0); }
	}

	public static void SetHighscore(int value)
	{
		PlayerPrefs.SetInt(HighScoreKey, value);
		var request = new UpdatePlayerStatisticsRequest()
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate { StatisticName = HighScoreKey, Value = value }
			}
		};
		PlayFabClientAPI.UpdatePlayerStatistics(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with SetHighscore :(");
			Debug.LogError(error.GenerateErrorReport());
		});
	}
	
	#endregion

	public static string PlayerId
	{
		get
		{
			if (!PlayerPrefs.HasKey(PlayerIdKey))
			{
				PlayerPrefs.SetString(PlayerIdKey, Guid.NewGuid().ToString());
			}
			return PlayerPrefs.GetString(PlayerIdKey);
		}
	}

	public static void ResetPlayerId()
	{
		PlayerPrefs.DeleteKey(PlayerIdKey);
	}
	
	public static int SoftCurrency
	{
		get
		{
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
