using System;
using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;

public class TileController : MonoBehaviour 
{

	#region Static

	private static TileController instance;
	public static TileController I
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

	[SerializeField]
	private int elementCountY = 10;

	#endregion

	#region Properties

	public TileElement SelectedTile { get; private set; }

	#endregion

	#region Public API

	public TileElement ElementAt(int x, int y)
	{
		var i = y * elementCountX + x;
		if (i < 0 || i >= tileElements.Count)
			return null;

		return tileElements[i];
	}

	public void OnTileElementClick(TileElement newTile)
	{
		var index = tileElements.IndexOf(newTile);
		var y = index / elementCountX;
		var x = index - y * elementCountX;

		if (SelectedTile == null && !newTile.IsEmpty)
		{
			SelectedTile = newTile;
			newTile.Select();
		}
		else if (SelectedTile == newTile)
		{
			SelectedTile = null;
			newTile.Unselect();
		}
		else if (!newTile.IsEmpty)
		{
			Debug.LogError("TAKEN");
		}
		else if (SelectedTile != null)
		{
			var movingType = SelectedTile.Type;
			SelectedTile.Unselect();
			SelectedTile.SetType(TileElement.ElementType.None);
			
			newTile.SetType(movingType);
			SelectedTile = null;
		}
	}

	public void GenerateNewMap(int numOfElements = 6, List<TileElement.ElementType> possibleElements = null)
	{
		if (possibleElements == null || possibleElements.Count == 0)
		{
			possibleElements = new List<TileElement.ElementType>
			{
				TileElement.ElementType.Cat,
				TileElement.ElementType.Chicken,
				TileElement.ElementType.Cow,
				TileElement.ElementType.Dog,
				TileElement.ElementType.Pig,
				TileElement.ElementType.Sheep,
			};
		}
		
		for (int y = 0; y < elementCountY; y++)
		{
			for (int x = 0; x < elementCountX; x++)
			{
				ElementAt(x,y).SetType(TileElement.ElementType.None);
			}
		}

		while (numOfElements > 0)
		{
			var randX = UnityEngine.Random.Range(0, elementCountX);
			var randY = UnityEngine.Random.Range(0, elementCountY);
			if (ElementAt(randX, randY).Type == TileElement.ElementType.None)
			{
				ElementAt(randX, randY).SetType(possibleElements.GetRandom());
				numOfElements--;
			}
		}
	}

	#endregion
}
