using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class AnalyticsData
{

	#region Keys
	
	private const string NewGameKey = "NewGame";
	private const string NewGameTypeKey = "NewGameType";
	
	private const string EndGameKey = "EndGame";
	private const string EndGameTypeKey = "EndGameType";
	private const string EndGameTurnsKey = "EndGameTurns";
	private const string EndGameTimeKey = "EndGameTime";
	private const string EndGamePointsKey = "EndGamePoints";
	
	private const string MilestoneReachedKey = "MilestoneReached";
	private const string MilestoneReachedIndexKey = "MilestoneReachedIndex";
	
	private const string LineExplodedKey = "LineExploded";
	private const string LineExplodedCountKey = "LineExplodedCount";
	private const string LineExplodedPointsKey = "LineExplodedPoints";

	private const string AnimalUpgradeKey = "AnimalUpgrade";
	private const string AnimalUpgradeTypeKey = "AnimalUpgradeType";
	private const string AnimalUpgradeLevelKey = "AnimalUpgradeLevel";
	
	private const string FreeGiftClaimedKey = "FreeGiftClaimed";
	
	#endregion
	
	#region Public

	public static void TrackNewGame(GameController.GameModeType gameType)
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = NewGameKey,
			Body = new Dictionary<string, object>
			{
				{ NewGameTypeKey, gameType.ToString() }
			}
		}; 
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}

	public static void TrackEndGame(GameController.EndGameType endGameType, int turns, float playTime, int points)
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = EndGameKey,
			Body = new Dictionary<string, object>
			{
				{ EndGameTypeKey, endGameType.ToString() },
				{ EndGameTurnsKey, turns.ToString() },
				{ EndGameTimeKey, playTime.ToString("0.00") },
				{ EndGamePointsKey, points.ToString() }
			}
		};
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}

	public static void TrackMilestoneReached(int milestoneIndex)
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = MilestoneReachedKey,
			Body = new Dictionary<string, object>
			{
				{ MilestoneReachedIndexKey, milestoneIndex.ToString() },
			}
		};
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}

	public static void TrackLineExploded(int count, int points)
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = LineExplodedKey,
			Body = new Dictionary<string, object>
			{
				{ LineExplodedCountKey, count.ToString() },
				{ LineExplodedPointsKey, points.ToString() },
			}
		};
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}

	public static void TrackAnimalUpgrade(TileElement.ElementType elementType, int level)
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = AnimalUpgradeKey,
			Body = new Dictionary<string, object>
			{
				{ AnimalUpgradeTypeKey, elementType.ToString() },
				{ AnimalUpgradeLevelKey, level.ToString() },
			}
		};
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}

	public static void TrackFreeGiftClaimed()
	{
		var request = new WriteClientPlayerEventRequest
		{
			EventName = FreeGiftClaimedKey
		}; 
		PlayFabClientAPI.WritePlayerEvent(request, null, null);
	}
	
	#endregion
	
}
