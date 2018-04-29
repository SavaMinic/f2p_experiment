using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{

	#region Fields

	[SerializeField]
	private Button menuButton;

	#endregion

	#region Mono

	private void Awake()
	{
		menuButton.onClick.AddListener(OnMenuButtonClick);
	}

	#endregion

	#region Events

	private void OnMenuButtonClick()
	{
		GameController.I.PauseGame();
		PausedMenuController.I.ShowPauseMenu();
	}

	#endregion
}
