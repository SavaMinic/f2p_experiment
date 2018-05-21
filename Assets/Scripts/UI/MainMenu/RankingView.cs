using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : MonoBehaviour
{

	#region Fields

	[SerializeField]
	private Text playerNameText;

	[SerializeField]
	private Text playerScoreText;

	[SerializeField]
	private Image playerImage;

	[SerializeField]
	private Image backImage;
	
	[SerializeField]
	private Color normalColor;

	[SerializeField]
	private Color localPlayerColor;
	
	public float Height { get; private set; }

	#endregion

	#region Mono

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnButtonClick);
		Height = GetComponent<RectTransform>().sizeDelta.y;
	}

	#endregion

	#region Public

	public void Refresh(PlayerLeaderboardEntry playerRanking)
	{
		if (playerRanking == null)
		{
			playerNameText.text = "?. Unknown";
			playerScoreText.text = "?";
			backImage.color = normalColor;
			return;
		}

		var name = playerRanking.DisplayName ?? playerRanking.PlayFabId;
		playerNameText.text = (playerRanking.Position + 1) + ". " + name;
		playerScoreText.text = playerRanking.StatValue.ToString();

		backImage.color = playerRanking.PlayFabId == PlayerData.PlayFabId ? localPlayerColor : normalColor;
	}

	#endregion

	#region Events

	private void OnButtonClick()
	{
		
	}

	#endregion
}
