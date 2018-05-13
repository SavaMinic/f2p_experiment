using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class RankingsData
{

	public class PlayerRanking
	{
		public int Id;
		public string Name;
		public int Score;
		public int Index = -1;
		public int ImageId;
	}

	public static void Initialize()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://animal-lines.firebaseio.com/");
	}

	public static List<PlayerRanking> GetFriendsRankings()
	{
		// TODO: add proper staff
		var list = new List<PlayerRanking>()
		{
			new PlayerRanking { Id = 1, Name = "Katarina", Score = 6730, ImageId = 1 },
			new PlayerRanking { Id = 2, Name = "Sava", Score = 2200, ImageId = 1 },
			new PlayerRanking { Id = 3, Name = "Dusan", Score = 1298, ImageId = 1 },
			new PlayerRanking { Id = 4, Name = "Sandra", Score = 764, ImageId = 1 },
			new PlayerRanking { Id = 5, Name = "Ana", Score = 356, ImageId = 1 },
			new PlayerRanking { Id = 6, Name = "Putin", Score = 80, ImageId = 1 },
			new PlayerRanking { Id = PlayerData.PlayerId, Name = "YOU", Score = PlayerData.HighScore, ImageId = 1 }
		};
		list.Sort((u1, u2) => { return u2.Score - u1.Score; });
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Index == -1)
				list[i].Index = i;
		}
		return list;
	}

	public static void GetRankings(Action<List<PlayerRanking>> onDoneCallback, string country = "all")
	{
		FirebaseDatabase.DefaultInstance
		.GetReference("users/" + country)
		.OrderByChild("score").LimitToFirst(100)
		.GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted)
			{
				Debug.LogError(task.Exception);
				return;
			}
			
			if (task.IsCompleted)
			{
				DataSnapshot users = task.Result;
				if (users.ChildrenCount > 0)
				{
					var list = new List<PlayerRanking>();
					var index = (int)users.ChildrenCount;
					foreach (var user in users.Children)
					{
						list.Insert(0, new PlayerRanking
						{
							Index = --index,
							Id = int.Parse(user.Key),
							Name = user.Child("username").Value.ToString(),
							Score = int.Parse(user.Child("score").Value.ToString()),
							ImageId = 1
						});
					}
					onDoneCallback(list);
				}
			}
		});
	}
	
}
