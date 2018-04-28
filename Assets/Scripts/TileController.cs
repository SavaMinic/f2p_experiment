using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour 
{

	#region Static

	private static TileController instance;
	public static TileController Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<TileController>();
			return instance;
		}
	}

	#endregion

	#region Actions

	public Action<TileElement,TileElement> OnSelectionTileChanged;

	#endregion
	
	#region Fields

	[SerializeField]
	private List<TileElement> tileElements;

	[SerializeField]
	private int elementCountX = 7;

	#endregion

	#region Properties

	public TileElement SelectedTile { get; private set; }

	#endregion

	#region Public API

	public void OnTileElementClick(TileElement element)
	{
		var index = tileElements.IndexOf(element);
		var y = index / elementCountX;
		var x = index - y * elementCountX;
		Debug.LogError("CLICKED " + x + " " + y);

		if (SelectedTile == null || SelectedTile != element)
		{
			OnSelectionTileChanged(SelectedTile, element);
			SelectedTile = element;
		}
		else if (SelectedTile == element)
		{
			OnSelectionTileChanged(SelectedTile, null);
			SelectedTile = null;
		}
	}

	#endregion
}
