using System.Collections;
using System.Collections.Generic;
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

	public static List<PlayerRanking> GetWorldRankings()
	{
		// TODO: add proper staff
		var list = new List<PlayerRanking>()
		{
			new PlayerRanking { Id = 51, Name = "HoShiMin", Score = 17634, ImageId = 1 },
			new PlayerRanking { Id = 52, Name = "Hjughao", Score = 16567, ImageId = 1 },
			new PlayerRanking { Id = 53, Name = "Rainbow123", Score = 16345, ImageId = 1 },
			new PlayerRanking { Id = 54, Name = "TrumpSucks45", Score = 15234, ImageId = 1 },
			new PlayerRanking { Id = 55, Name = "Ghighao", Score = 11887, ImageId = 1 },
			new PlayerRanking { Id = 56, Name = "Wtf2", Score = 11659, ImageId = 1 },
			new PlayerRanking { Id = 57, Name = "REERE", Score = 10234, ImageId = 1 },
			new PlayerRanking { Id = 58, Name = "WEWEW", Score = 10201, ImageId = 1 },
			new PlayerRanking { Id = 59, Name = "PayToWin2", Score = 10200, ImageId = 1 },
			new PlayerRanking { Id = 60, Name = "SWE123", Score = 10195, ImageId = 1 },
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
	
	public static List<PlayerRanking> GetLocalRankings()
	{
		// TODO: add proper staff
		var list = new List<PlayerRanking>()
		{
			new PlayerRanking { Id = 101, Name = "LudiBuljko", Score = 7300, ImageId = 1 },
			new PlayerRanking { Id = 102, Name = "Milos25", Score = 7239, ImageId = 1 },
			new PlayerRanking { Id = 103, Name = "Minecraft", Score = 7106, ImageId = 1 },
			new PlayerRanking { Id = 104, Name = "StaStaSta", Score = 6921, ImageId = 1 },
			new PlayerRanking { Id = 105, Name = "Katarina", Score = 6730, ImageId = 1 },
			new PlayerRanking { Id = 106, Name = "beograd", Score = 6670, ImageId = 1 },
			new PlayerRanking { Id = 107, Name = "Auauaua", Score = 6489, ImageId = 1 },
			new PlayerRanking { Id = 108, Name = "wowowow", Score = 5567, ImageId = 1 },
			new PlayerRanking { Id = 109, Name = "MilosLjubica", Score = 5434, ImageId = 1 },
			new PlayerRanking { Id = 110, Name = "Aleksa86", Score = 5401, ImageId = 1 },
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
	
}
