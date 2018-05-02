using System.Collections;
using System.Collections.Generic;
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
	
	public float Width { get; private set; }

	#endregion

	#region Mono

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnButtonClick);
		Width = GetComponent<RectTransform>().sizeDelta.x;
	}

	#endregion

	#region Public

	public void Refresh(int index, RankingsData.PlayerRanking playerRanking)
	{
		playerNameText.text = index + ". " + playerRanking.Name;
		playerScoreText.text = playerRanking.Score.ToString();

		backImage.color = playerRanking.Id == PlayerData.PlayerId ? localPlayerColor : normalColor;
	}

	#endregion

	#region Events

	private void OnButtonClick()
	{
		
	}

	#endregion
}
