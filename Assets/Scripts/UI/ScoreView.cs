using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{

	#region Fields

	[SerializeField]
	private Text scoreText;

	[SerializeField]
	private float scaleIncrease = 2f;

	[SerializeField]
	private float increaseDuration = 0.4f;

	[SerializeField]
	private GoEaseType increaEaseType;

	[SerializeField]
	private Text scoreIncreaseText;

	[SerializeField]
	private CanvasGroup scoreIncreaseCanvasGroup;

	private IEnumerator scoreIncreaseCoroutine;
	private int fontSize;

	#endregion

	#region Mono

	private void Start()
	{
		fontSize = scoreText.fontSize;
		scoreIncreaseCanvasGroup.alpha = 0f;
		GameController.I.OnScoreChanged += OnScoreChanged;
	}

	#endregion

	#region Private

	private void OnScoreChanged(int newScore)
	{
		if (GameController.I.IsPlaying)
		{
			if (scoreIncreaseCoroutine != null)
			{
				StopCoroutine(scoreIncreaseCoroutine);
			}
			scoreIncreaseCoroutine = DoScoreChangedAnimation(newScore);
			StartCoroutine(scoreIncreaseCoroutine);
		}
		else
		{
			scoreText.text = newScore.ToString();
		}
	}

	private IEnumerator DoScoreChangedAnimation(int newScore)
	{
		scoreIncreaseText.text = "+" + (newScore - GameController.I.Score);
		Go.to(scoreText, increaseDuration / 4f, new GoTweenConfig()
			.intProp("fontSize", Mathf.RoundToInt(scaleIncrease * fontSize)).setEaseType(increaEaseType)
		);
		Go.to(scoreIncreaseCanvasGroup, increaseDuration / 6f, new GoTweenConfig()
			.floatProp("alpha", 1f).setEaseType(increaEaseType)
		);
		scoreText.fontStyle = FontStyle.Bold;
		yield return new WaitForSecondsRealtime(increaseDuration / 4f);
		
		scoreText.text = newScore.ToString();
		
		yield return new WaitForSecondsRealtime(increaseDuration / 4f);
		scoreText.fontStyle = FontStyle.Normal;
		Go.to(scoreText, increaseDuration / 4f, new GoTweenConfig()
			.intProp("fontSize", fontSize).setEaseType(increaEaseType)
		);
		Go.to(scoreIncreaseCanvasGroup, increaseDuration / 2f, new GoTweenConfig()
			.floatProp("alpha", 0).setEaseType(increaEaseType)
		);
	}

	#endregion
}
