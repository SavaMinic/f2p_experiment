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
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideHeader();
		softCurrencyButton.onClick.AddListener(OnSoftCurrencyButtonClick);
		hardCurrencyButton.onClick.AddListener(OnHardCurrencyButtonClick);
	}

	#endregion
	
	#region Public

	public void ShowHeader()
	{
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

	#endregion
}
