using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour
{
	
	#region Static

	private static EndMenuController instance;
	public static EndMenuController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<EndMenuController>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[SerializeField]
	private Button continueButton;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[SerializeField]
	private Text statusText;

	[SerializeField]
	private Text scoreText;

	[SerializeField]
	private RectTransform starsPanel;

	[SerializeField]
	private List<CanvasGroup> starCanvasGroups;

	[SerializeField]
	private RectTransform endlessStatsPanel;
	
	[SerializeField]
	private Text endlessTurnsText;

	[SerializeField]
	private Text endlessTimeText;

	[SerializeField]
	private float starShowDuration;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideEndMenu(true);
		continueButton.onClick.AddListener(OnContinueButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowEndMenu(GameController.EndGameType endType)
	{
		AudioController.I.SetBackgroundMusicVolume(0.25f);


		if (GameController.I.GameMode == GameController.GameModeType.Endless)
		{
			starsPanel.gameObject.SetActive(false);
			endlessStatsPanel.gameObject.SetActive(true);
			
			
			statusText.text = "Level up animals to get better score!";
			AudioController.I.PlayPositiveSFX();

			scoreText.text = GameController.I.Score + "pts";
			endlessTurnsText.text = "Turns\n" + GameController.I.GameTurn;
			endlessTimeText.text = "Time\n" + GameController.I.GameTime.FormatToTime();
		}
		else
		{
			switch (endType)
			{
				case GameController.EndGameType.Manual:
					statusText.text = "Better luck next time!";
					AudioController.I.PlayNeutralSFX();
					break;
				case GameController.EndGameType.NoEmptySpace:
					statusText.text = "No more spaces left :(";
					AudioController.I.PlayNegativeSFX();
					break;
				case GameController.EndGameType.TimeLimit:
					statusText.text = "No more time :(";
					AudioController.I.PlayNegativeSFX();
					break;
				case GameController.EndGameType.TurnLimit:
					statusText.text = "No more turns :(";
					AudioController.I.PlayNegativeSFX();
					break;
				case GameController.EndGameType.Win:
					statusText.text = "GREAT JOB :)";
					AudioController.I.PlayPositiveSFX();
					break;
			}
			
			starsPanel.gameObject.SetActive(true);
			endlessStatsPanel.gameObject.SetActive(false);
			var starCount = 0;
			if (GameController.I.GameMode == GameController.GameModeType.TargetScore)
			{
				scoreText.text = GameController.I.Score + " / " + GameController.I.TargetScore;
				starCount = GameController.I.Score / GameController.I.TargetScore * 3;
			}
			else if (GameController.I.GameMode == GameController.GameModeType.Collections)
			{
				scoreText.text = GameController.I.Score + "pts";
				// TODO: implement this depending on points
				starCount = 2;
			}
			StartCoroutine(DoStarsAnimation(starCount));
		}
		
		// fade in
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideEndMenu(bool instant = false)
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = false;
		if (instant)
		{
			mainCanvasGroup.alpha = 0f;
			return;
		}
		AudioController.I.SetBackgroundMusicVolume(1f, 1.5f);
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 0f));
	}
	
	#endregion

	#region Events

	private void OnContinueButtonClick()
	{
		HideEndMenu();
		GameController.I.ExitToMainMenu();
	}

	#endregion

	#region Animation

	private IEnumerator DoStarsAnimation(int starCount)
	{
		for (int i = 0; i < starCanvasGroups.Count; i++)
		{
			starCanvasGroups[i].alpha = 0f;
		}

		yield return new WaitForSecondsRealtime(fadeDuration * 3f);
		
		for (int i = 0; i < starCount; i++)
		{
			Go.to(starCanvasGroups[i], starShowDuration, new GoTweenConfig().floatProp("alpha", 1f));
			yield return new WaitForSecondsRealtime(starShowDuration * 0.3f);
		}
	}

	#endregion
}
