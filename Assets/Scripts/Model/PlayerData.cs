using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayerData
{

	#region Keys
	
	private const string PlayerIdKey = "PlayerId";
	
	private const string PlayFabIdKey = "PlayFabKey";
	private const string PlayFabDisplayNameKey = "PlayFabDisplayName";
	
	public const string HighScoreKey = "HighScoreKey";

	private const string SoftCurrencyKey = "SoftCurrencyKey";

	private const string FreeGiftTimeKey = "FreeGiftTimeKey";

	private const string PlayerHasSetDisplayNameKey = "PlayerHasSetDisplayName";
	
	#endregion
	
	#region Refresh data

	public static void RefreshUserStatistics(Action onSuccess)
	{
		var request = new GetPlayerStatisticsRequest();
		PlayFabClientAPI.GetPlayerStatistics(request,
			playerStats =>
			{
				Debug.Log("RefreshUserStatistics success");
				var highScore = HighScore;
				var score = playerStats.Statistics.Find(s => s.StatisticName == HighScoreKey);
				if (score != null)
				{
					highScore = score.Value;
				}
				PlayerPrefs.SetInt(HighScoreKey, highScore);
				onSuccess();
			},
			error =>
			{
				Debug.LogWarning("Something went wrong with RefreshUserStatistics :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}

	public static void RefreshUserData(Action onSuccess)
	{
		var request = new GetUserDataRequest();
		PlayFabClientAPI.GetUserData(request,
			userData =>
			{
				Debug.Log("RefreshUserData success");
				if (userData.Data.ContainsKey(SoftCurrencyKey))
				{
					PlayerPrefs.SetInt(SoftCurrencyKey, int.Parse(userData.Data[SoftCurrencyKey].Value));
				}
				if (userData.Data.ContainsKey(FreeGiftTimeKey))
				{
					PlayerPrefs.SetString(FreeGiftTimeKey, userData.Data[FreeGiftTimeKey].Value);
				}
				onSuccess();
			},
			error =>
			{
				Debug.LogWarning("Something went wrong with RefreshUserData :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}

	public static void RefreshAccountData(Action onSuccess)
	{
		var request = new GetAccountInfoRequest();
		PlayFabClientAPI.GetAccountInfo(request,
			accountInfo =>
			{
				Debug.Log("RefreshAccountData success");
				PlayerPrefs.SetString(PlayFabIdKey, accountInfo.AccountInfo.PlayFabId);
				if (!string.IsNullOrEmpty(accountInfo.AccountInfo.TitleInfo.DisplayName))
				{
					PlayerPrefs.SetString(PlayFabDisplayNameKey, accountInfo.AccountInfo.TitleInfo.DisplayName);
				}
				onSuccess();
			},
			error =>
			{
				Debug.LogWarning("Something went wrong with RefreshAccountData :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}
	
	#endregion
	
	#region Highscore

	public static int HighScore
	{
		get
		{
			if (!PlayerPrefs.HasKey(HighScoreKey))
			{
				SetHighscore(0);
			}
			return PlayerPrefs.GetInt(HighScoreKey);
		}
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

	#region Soft currency
	
	public static int SoftCurrency
	{
		get { return PlayerPrefs.GetInt(SoftCurrencyKey, GameSettings.I.StartingSoftCurrency); }
	}
	
	public static void SetSoftCurrency(int value)
	{
		PlayerPrefs.SetInt(SoftCurrencyKey, value);
		var request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>
			{
				{ SoftCurrencyKey, value.ToString() }
			}
		};
		PlayFabClientAPI.UpdateUserData(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with SetSoftCurrency :(");
			Debug.LogError(error.GenerateErrorReport());
		});
	}

	#endregion

	#region Player ID

	public static string PlayFabId { get { return PlayerPrefs.GetString(PlayFabIdKey, "-1"); } }

	public static string PlayerId
	{
		get
		{
			// don't call Guid.NewGuid() if it's not needed
			if (!PlayerPrefs.HasKey(PlayerIdKey))
			{
				PlayerPrefs.SetString(PlayerIdKey, Guid.NewGuid().ToString());
			}
			return PlayerPrefs.GetString(PlayerIdKey);
		}
	}

	#endregion

	#region Player display name
	
	public static bool PlayerHasSetDisplayName { get { return PlayerPrefs.GetInt(PlayerHasSetDisplayNameKey, 0) == 1; } }

	public static string PlayerDisplayName
	{
		get { return PlayerPrefs.GetString(PlayFabDisplayNameKey, "Player_" + PlayFabId.Substring(0, 8)); }
	}
	
	public static void UpdatePlayerDisplayName(string name, Action onSuccess)
	{
		PlayerPrefs.SetInt(PlayerHasSetDisplayNameKey, 1);
		PlayerPrefs.SetString(PlayFabDisplayNameKey, name);
		
		var request = new UpdateUserTitleDisplayNameRequest
		{
			DisplayName = name
		};
		PlayFabClientAPI.UpdateUserTitleDisplayName(request,
			result => { onSuccess(); },
			error =>
			{
				Debug.LogWarning("Something went wrong with UpdatePlayerDisplayName :(");
				Debug.LogError(error.GenerateErrorReport());	
			}
		);
	}

	#endregion

	#region Free Gift timer

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
		var timeString = time.ToBinary().ToString();
		PlayerPrefs.SetString(FreeGiftTimeKey, timeString);
		
		var request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>
			{
				{ FreeGiftTimeKey, timeString }
			}
		};
		PlayFabClientAPI.UpdateUserData(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with SetupFreeGiftClaimTime :(");
			Debug.LogError(error.GenerateErrorReport());
		});
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

	#endregion

	#region Reset keys
	
	public static void ResetFreeGiftTimer()
	{
		var timeString = DateTime.Now.ToBinary().ToString();
		PlayerPrefs.SetString(FreeGiftTimeKey, timeString);
		var request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>
			{
				{ FreeGiftTimeKey, timeString }
			}
		};
		PlayFabClientAPI.UpdateUserData(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with ResetFreeGiftTimer :(");
			Debug.LogError(error.GenerateErrorReport());
		});
	}

	public static void ResetPlayer()
	{
		PlayerPrefs.DeleteKey(PlayerIdKey);
		PlayerPrefs.DeleteKey(PlayFabIdKey);
		PlayerPrefs.DeleteKey(PlayFabDisplayNameKey);
		PlayerPrefs.DeleteKey(FreeGiftTimeKey);
		PlayerPrefs.DeleteKey(HighScoreKey);
		PlayerPrefs.DeleteKey(SoftCurrencyKey);
		PlayerPrefs.DeleteKey(PlayerHasSetDisplayNameKey);
	}
	
	#endregion
	
}
