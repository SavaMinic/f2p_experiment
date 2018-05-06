using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalsView : MonoBehaviour
{

	#region Static

	private static GoalsView instance;
	public static GoalsView I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<GoalsView>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[Header("Target score")]
	[SerializeField]
	private RectTransform targetScorePanel;

	[SerializeField]
	private Text targetScoreText;

	[SerializeField]
	private Slider targetScoreSlider;

	[SerializeField]
	private float scoreChangeDuration = 0.2f;

	[SerializeField]
	private GoEaseType scoreChangeEaseType;

	[SerializeField]
	private float scoreChangeDelay = 0.2f;
	
	[Header("Endless")]
	[SerializeField]
	private RectTransform endlessScorePanel;

	[SerializeField]
	private Text endlessHighScoreText;

	[SerializeField]
	private Color defaultEndlessTextColor;

	[SerializeField]
	private Color newEndlessTextColor;

	[SerializeField]
	private float newHighscoreDelay = 0.4f;

	[SerializeField]
	private float newHishscoreDuration = 0.4f;

	[SerializeField]
	private GoEaseType newHishscoreEaseType;

	[SerializeField]
	private float newHishscoreScaleIncrease = 2f;

	[SerializeField]
	private Text milestoneScoreText;

	[SerializeField]
	private Text milestoneRewardText;

	[SerializeField]
	private Slider milestoneScoreSlider;
	
	[Header("Collections")]
	[SerializeField]
	private RectTransform collectionsPanel;

	[Header("Limits")]
	[SerializeField]
	private RectTransform limitsPanel;

	[SerializeField]
	private Text limitTitleText;
	
	[SerializeField]
	private Text limitValueText;
	

	private GoTween scoreChangeTween;
	private GoTween milestoneChangeTween;
	
	private int highScoreFontSize;

	#endregion

	#region Mono

	private void Start()
	{
		highScoreFontSize = endlessHighScoreText.fontSize;
		GameController.I.OnScoreChanged += OnScoreChanged;
		GameController.I.OnNewTurn += OnNewTurn;
		GameController.I.OnMilestoneChanged += OnMilestoneChanged;
	}

	private void Update()
	{
		if (!Application.isPlaying || !GameController.I.IsPlaying)
			return;

		if (GameController.I.IsTimeLimitMode)
		{
			RefreshTimeLeft();
		}
	}

	#endregion

	#region Public

	public void Initialize()
	{
		var mode = GameController.I.GameMode;
		targetScorePanel.gameObject.SetActive(mode == GameController.GameModeType.TargetScore);
		endlessScorePanel.gameObject.SetActive(mode == GameController.GameModeType.Endless);
		collectionsPanel.gameObject.SetActive(mode == GameController.GameModeType.Collections);
		
		limitsPanel.gameObject.SetActive(GameController.I.IsTimeLimitMode || GameController.I.IsTurnsMode);
		limitTitleText.text = GameController.I.IsTimeLimitMode ? "TIME" : "TURNS";

		endlessHighScoreText.text = "HIGHSCORE: " + PlayerData.HighScore;
		endlessHighScoreText.color = defaultEndlessTextColor;

		milestoneRewardText.text = GameController.I.MilestoneReward.ToString();
		milestoneScoreText.text = "SCORE " + GameController.I.MilestonePoint + " FOR REWARD";

		// INITIAL STATE
		if (mode == GameController.GameModeType.TargetScore)
		{
			targetScoreText.text = "TARGET: " + GameController.I.TargetScore;
			targetScoreSlider.value = (float)GameController.I.Score / GameController.I.TargetScore;;
		}
		else if (mode == GameController.GameModeType.Endless)
		{
			milestoneScoreSlider.value = 0f;
		}
		
		if (GameController.I.IsTurnsMode)
		{
			limitValueText.text = GameController.I.TurnsLimit.ToString();
		}
		else if (GameController.I.IsTimeLimitMode)
		{
			RefreshTimeLeft();
		}
	}

	#endregion

	#region Events

	private void OnMilestoneChanged()
	{
		var milestoneProgress = GameController.I.MilestoneProgress(GameController.I.Score);
		if (milestoneChangeTween != null && milestoneChangeTween.state == GoTweenState.Running)
		{
			milestoneChangeTween.destroy();
		}
		milestoneChangeTween = Go.to(milestoneScoreSlider, scoreChangeDuration,
			new GoTweenConfig().floatProp("value", milestoneProgress)
				.setEaseType(scoreChangeEaseType)
				.setDelay(scoreChangeDelay)
		);
		
		milestoneRewardText.text = GameController.I.MilestoneReward.ToString();
		milestoneScoreText.text = "SCORE " + GameController.I.MilestonePoint + " FOR REWARD";
	}
	
	private void OnScoreChanged(int newScore)
	{
		if (GameController.I.GameMode == GameController.GameModeType.Endless)
		{
			if (newScore > PlayerData.HighScore)
			{
				PlayerData.HighScore = newScore;
				StartCoroutine(ChangeNewHighScore(newScore));
			}
			
			var milestoneProgress = GameController.I.MilestoneProgress(newScore);
			if (milestoneChangeTween != null && milestoneChangeTween.state == GoTweenState.Running)
			{
				milestoneChangeTween.destroy();
			}
			milestoneChangeTween = Go.to(milestoneScoreSlider, scoreChangeDuration,
				new GoTweenConfig().floatProp("value", milestoneProgress)
				.setEaseType(scoreChangeEaseType)
				.setDelay(scoreChangeDelay)
			);
			milestoneChangeTween.setOnCompleteHandler(t =>
			{
				if (newScore >= GameController.I.MilestonePoint)
				{
					GameController.I.NextMilestone();
				}
			});
		}
		
		if (!GameController.I.HasTargetScore)
			return;
		
		var progress = (float)newScore / GameController.I.TargetScore;
		if (GameController.I.IsPlaying)
		{
			if (scoreChangeTween != null && scoreChangeTween.state == GoTweenState.Running)
			{
				scoreChangeTween.destroy();
			}
			scoreChangeTween = Go.to(targetScoreSlider, scoreChangeDuration,
				new GoTweenConfig().floatProp("value", progress)
				.setEaseType(scoreChangeEaseType)
				.setDelay(scoreChangeDelay)
			);
		}
		else
		{
			// instant change
			targetScoreSlider.value = progress;
		}
	}

	private void OnNewTurn()
	{
		if (GameController.I.IsTurnsMode)
		{
			limitValueText.text = GameController.I.TurnsLimit.ToString();
		}
	}

	#endregion

	#region Private

	private IEnumerator ChangeNewHighScore(int newScore)
	{
		yield return new WaitForSecondsRealtime(newHighscoreDelay);
		
		Go.to(endlessHighScoreText, newHishscoreDuration / 3f, new GoTweenConfig()
			.intProp("fontSize", Mathf.RoundToInt(newHishscoreScaleIncrease * highScoreFontSize)).setEaseType(newHishscoreEaseType)
		);
		
		yield return new WaitForSecondsRealtime(newHishscoreDuration / 3f);
		
		endlessHighScoreText.text = "HIGHSCORE: " + newScore;
		endlessHighScoreText.color = newEndlessTextColor;
		
		yield return new WaitForSecondsRealtime(newHishscoreDuration / 3f);
		
		Go.to(endlessHighScoreText, newHishscoreDuration / 3f, new GoTweenConfig()
			.intProp("fontSize", highScoreFontSize).setEaseType(newHishscoreEaseType)
		);
	}

	private void RefreshTimeLeft()
	{
		limitValueText.text = Mathf.Max(0f, GameController.I.TimeLimit).ToString("0") + "s";
	}

	#endregion

}
