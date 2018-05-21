using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class AnimalData
{

	private const string AnimalLevelData = "AnimalLevel_";

	#region Properties

	public static float AverageAnimalLevel
	{
		get
		{
			var sum = 0f;
			var types = (TileElement.ElementType[])Enum.GetValues(typeof(TileElement.ElementType));
			for (int i = 0; i < types.Length; i++)
			{
				sum += AnimalLevel(types[i]) + 1;
			}
			return sum / types.Length;
		}
	}

	#endregion
	
	#region Refresh data

	public static void RefreshAnimalData(Action onSuccess)
	{
		var request = new GetUserDataRequest();
		PlayFabClientAPI.GetUserData(request,
			userData =>
			{
				
				var types = (TileElement.ElementType[])Enum.GetValues(typeof(TileElement.ElementType));
				for (int i = 0; i < types.Length; i++)
				{
					var key = AnimalLevelData + types[i];
					if (userData.Data.ContainsKey(key))
					{
						PlayerPrefs.SetInt(key, int.Parse(userData.Data[key].Value));
					}
				}
				onSuccess();
			}, error =>
			{
				Debug.LogWarning("Something went wrong with RefreshAnimalData :(");
				Debug.LogError(error.GenerateErrorReport());
			}
		);
	}

	#endregion

	#region Animal levels

	public static int AnimalLevel(TileElement.ElementType type)
	{
		return PlayerPrefs.GetInt(AnimalLevelData + type, 0);
	}

	public static void IncreaseAnimalLevel(TileElement.ElementType type)
	{
		var level = AnimalLevel(type);
		var key = AnimalLevelData + type;
		PlayerPrefs.SetInt(key, level + 1);
		
		var request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>
			{
				{ key, (level + 1).ToString() }
			}
		};
		PlayFabClientAPI.UpdateUserData(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with IncreaseAnimalLevel :(");
			Debug.LogError(error.GenerateErrorReport());
		});
	}
	
	#endregion

	#region Reset

	public static void ResetAnimalLevels()
	{
		var request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>()
		};
		foreach (var type in Enum.GetValues(typeof(TileElement.ElementType)))
		{
			PlayerPrefs.SetInt(AnimalLevelData + type, 0);
			request.Data.Add(AnimalLevelData + type, "0");
		}
		PlayFabClientAPI.UpdateUserData(request, null, error =>
		{
			Debug.LogWarning("Something went wrong with ResetAnimalLevels :(");
			Debug.LogError(error.GenerateErrorReport());
		});
	}

	#endregion
	
}
