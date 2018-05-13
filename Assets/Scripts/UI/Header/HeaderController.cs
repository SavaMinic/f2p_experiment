using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderController : MonoBehaviour
{
	#region Static

	private static HeaderController instance;
	public static HeaderController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<HeaderController>();
			return instance;
		}
	}

	#endregion
	
	#region Fields
	
	[Header("Highscore")]
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

	[Header("Currency")]
	[SerializeField]
	private Button softCurrencyButton;

	[SerializeField]
	private Text softCurrencyText;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private Color endColor;

	[SerializeField]
	private float colorChangeDuration;

	private Color initialColor;
	private int highScoreFontSize;
	
	#endregion

	#region Properties

	public Vector3 SoftCurrencyPosition
	{
		get { return softCurrencyText.rectTransform.position; }
	}

	#endregion

	#region Mono

	private void Awake()
	{
		highScoreFontSize = endlessHighScoreText.fontSize;
		initialColor = softCurrencyText.color;
		HideHeader();
		softCurrencyButton.onClick.AddListener(OnSoftCurrencyButtonClick);
	}

	private void Start()
	{
		GameController.I.OnSoftCurrencyChanged += OnSoftCurrencyChanged;
	}

	#endregion
	
	#region Public

	public void ShowHeader()
	{
		softCurrencyText.text = GameController.I.SoftCurrency.ToString();
		
		endlessHighScoreText.text = "HIGHSCORE: " + PlayerData.HighScore;
		endlessHighScoreText.color = defaultEndlessTextColor;
		
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		mainCanvasGroup.alpha = 1f;
	}

	public void HideHeader()
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = false;
		mainCanvasGroup.alpha = 0f;
	}

	public void UpdateHighScore(int newScore)
	{
		StartCoroutine(ChangeNewHighScore(newScore));
	}
	
	#endregion

	#region Events

	private void OnSoftCurrencyButtonClick()
	{
	}

	private void OnSoftCurrencyChanged(int newVal)
	{
		StartCoroutine(DoCurrencyColorAnimation(softCurrencyText, newVal));
	}

	#endregion

	#region Animations

	private IEnumerator DoCurrencyColorAnimation(Text currencyText, float newVal)
	{
		Go.to(currencyText, colorChangeDuration, new GoTweenConfig().colorProp("color", endColor));
		
		yield return new WaitForSecondsRealtime(colorChangeDuration * 0.5f);

		currencyText.text = newVal.ToString();
		
		yield return new WaitForSecondsRealtime(colorChangeDuration * 0.5f);
		
		Go.to(currencyText, colorChangeDuration, new GoTweenConfig().colorProp("color", initialColor));
	}

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

	#endregion
}
