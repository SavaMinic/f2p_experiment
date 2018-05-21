using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class RankingsData
{

	private static List<PlayerLeaderboardEntry> topPlayersList = null;
	private static PlayerLeaderboardEntry localPlayerRanking = null;

	public static void GetRankings(int index, Action<List<PlayerLeaderboardEntry>, PlayerLeaderboardEntry> callback)
	{
		// Reset the list
		topPlayersList = null;
		localPlayerRanking = null;
		
		var request = new GetLeaderboardRequest
		{
			StatisticName = PlayerData.HighScoreKey,
			StartPosition = 0,
			MaxResultsCount = 100
		};
		PlayFabClientAPI.GetLeaderboard(request,
			result =>
			{
				topPlayersList = result.Leaderboard;
				if (topPlayersList != null && localPlayerRanking != null)
				{
					callback(topPlayersList, localPlayerRanking);
				}
			},
			error =>
			{
				Debug.LogWarning("Something went wrong with GetRankings :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);

		var localPlayerRequest = new GetLeaderboardAroundPlayerRequest
		{
			StatisticName = PlayerData.HighScoreKey,
			MaxResultsCount = 1,
		};
		PlayFabClientAPI.GetLeaderboardAroundPlayer(localPlayerRequest,
			result =>
			{
				localPlayerRanking = result.Leaderboard[0];
				if (topPlayersList != null && localPlayerRanking != null)
				{
					callback(topPlayersList, localPlayerRanking);
				}
			},
			error =>
			{
				Debug.LogWarning("Something went wrong with GetRankings local player :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}
	
}
