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

	[SerializeField]
	private Button hardCurrencyButton;

	[SerializeField]
	private Button softCurrencyButton;

	[SerializeField]
	private Text hardCurrencyText;

	[SerializeField]
	private Text softCurrencyText;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private Color endColor;

	[SerializeField]
	private float colorChangeDuration;

	private Color initialColor;
	
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
		initialColor = softCurrencyText.color;
		HideHeader();
		softCurrencyButton.onClick.AddListener(OnSoftCurrencyButtonClick);
		hardCurrencyButton.onClick.AddListener(OnHardCurrencyButtonClick);
	}

	private void Start()
	{
		GameController.I.OnSoftCurrencyChanged += OnSoftCurrencyChanged;
		GameController.I.OnHardCurrencyChanged += OnHardCurrencyChanged;
	}

	#endregion
	
	#region Public

	public void ShowHeader()
	{
		RefreshCurrencies(GameController.I.SoftCurrency, GameController.I.HardCurrency);
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		mainCanvasGroup.alpha = 1f;
	}

	public void HideHeader()
	{
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = false;
		mainCanvasGroup.alpha = 0f;
	}
	
	#endregion

	#region Events

	private void OnSoftCurrencyButtonClick()
	{
	}

	private void OnHardCurrencyButtonClick()
	{
	}

	private void OnSoftCurrencyChanged(int newVal)
	{
		StartCoroutine(DoCurrencyColorAnimation(softCurrencyText, newVal));
	}

	private void OnHardCurrencyChanged(int newVal)
	{
		StartCoroutine(DoCurrencyColorAnimation(hardCurrencyText, newVal));
	}

	#endregion

	#region Private

	private void RefreshCurrencies(int softCurrency, int hardCurrency)
	{
		softCurrencyText.text = softCurrency.ToString();
		hardCurrencyText.text = hardCurrency.ToString();
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

	#endregion
}
