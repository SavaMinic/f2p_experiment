using System;
using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class EditNamePopup : MonoBehaviour
{

	#region Static

	private static EditNamePopup instance;
	public static EditNamePopup I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<EditNamePopup>();
			return instance;
		}
	}

	#endregion
	
	#region Fields

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[SerializeField]
	private float fadeDuration = 0.3f;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Button backButton;

	[SerializeField]
	private InputField usernameInput;

	[SerializeField]
	private Button updateButton;

	private Action onSuccessCallback;
	
	#endregion

	#region Mono

	private void Awake()
	{
		HideEditNamePopup(true);
		backButton.onClick.AddListener(OnCloseButtonClick);
		closeButton.onClick.AddListener(OnCloseButtonClick);
		
		updateButton.onClick.AddListener(OnUpdateButtonClick);
	}

	#endregion

	#region Public

	public void ShowEditNamePopup(bool instant = false, bool canClosePopup = true, Action onSuccess = null)
	{
		onSuccessCallback = onSuccess;
		closeButton.gameObject.SetActive(canClosePopup);
		backButton.interactable = canClosePopup;
		
		updateButton.interactable = true;
		usernameInput.interactable = true;
		usernameInput.text = PlayerData.PlayerDisplayName;
		
		mainCanvasGroup.interactable = mainCanvasGroup.blocksRaycasts = true;
		if (instant)
		{
			mainCanvasGroup.alpha = 1f;
			return;
		}
		Go.to(mainCanvasGroup, fadeDuration, new GoTweenConfig().floatProp("alpha", 1f));
	}

	public void HideEditNamePopup(bool instant = false)
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

	private void OnCloseButtonClick()
	{
		HideEditNamePopup();
	}

	private void OnUpdateButtonClick()
	{
		var username = usernameInput.text.Trim();

		// change name
		if (username.Length > 0 && username.Length < usernameInput.characterLimit)
		{
			updateButton.interactable = false;
			usernameInput.interactable = false;
			
			PlayerData.UpdatePlayerDisplayName(username, () =>
			{
				onSuccessCallback.CallIfNotNull();
				HideEditNamePopup();
			});
			
		}
	}

	#endregion
}
