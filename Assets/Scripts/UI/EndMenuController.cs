using System.Collections;
using System.Collections.Generic;
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
	private List<CanvasGroup> starCanvasGroups;

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

	public void ShowEndMenu(GameController.EndGameType endType, bool instant = false)
	{
		scoreText.text = GameController.I.Score + "pts";
		switch (endType)
		{
			case GameController.EndGameType.Manual:
				statusText.text = "Just relax and play!";
				break;
			case GameController.EndGameType.NoEmptySpace:
				statusText.text = "No more spaces left!";
				break;
			case GameController.EndGameType.TimeLimit:
				statusText.text = "No more turns!";
				break;
			case GameController.EndGameType.TurnLimit:
				statusText.text = "No more time!";
				break;
			case GameController.EndGameType.Win:
				statusText.text = "Congratulations!";
				break;
		}
		StartCoroutine(DoStarsAnimation(2));
		
		// fade in
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
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
