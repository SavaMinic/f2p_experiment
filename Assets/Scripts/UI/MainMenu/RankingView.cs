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

	[SerializeField]
	private Button editPlayerButton;
	
	public float Height { get; private set; }

	#endregion

	#region Mono

	private void Awake()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(OnButtonClick);
		// for now, it's disabled
		button.interactable = false;
		
		Height = GetComponent<RectTransform>().sizeDelta.y;
		editPlayerButton.onClick.AddListener(OnEditPlayerButtonClick);
	}

	#endregion

	#region Public

	public void Refresh(PlayerLeaderboardEntry playerRanking, bool isHeader = false)
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
		
		playerScoreText.gameObject.SetActive(!isHeader);
		editPlayerButton.gameObject.SetActive(isHeader);
		
		backImage.color = playerRanking.PlayFabId == PlayerData.PlayFabId ? localPlayerColor : normalColor;
	}

	#endregion

	#region Events

	private void OnButtonClick()
	{
		
	}

	private void OnEditPlayerButtonClick()
	{
		EditNamePopup.I.ShowEditNamePopup(onSuccess: EndlessMenuController.I.RefreshRankings);
	}

	#endregion
}
