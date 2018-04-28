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
	
	#region Fields

	[SerializeField]
	private List<TileElement> tileElements;

	[SerializeField]
	private int elementCountX = 7;

	#endregion

	#region Public API

	public void OnTileElementClick(TileElement element)
	{
		var index = tileElements.IndexOf(element);
		var y = index / elementCountX;
		var x = index - y * elementCountX;
		Debug.LogError("CLICKED " + x + " " + y);
	}

	#endregion
}
