using System;
using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;
using UnityEngine.UI;

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
	
	private static readonly Location[] Directions = new [] {
		new Location(1, 0), // to right of tile
		new Location(0, -1), // below tile
		new Location(-1, 0), // to left of tile
		new Location(0, 1), // above tile
	};

	#endregion
	
	#region Fields

	[SerializeField]
	private List<TileElement> tileElements;

	[SerializeField]
	private int elementCountX = 7;

	[SerializeField]
	private int elementCountY = 10;

	[SerializeField]
	private Image movingImage;

	[SerializeField]
	private Vector2 tileSize;

	[SerializeField]
	private Vector2 startOffset;

	[SerializeField]
	private Vector2 tileSpacing;

	[SerializeField]
	private float movementDuration = 0.4f;

	[SerializeField]
	private GoEaseType movementEaseType;

	private bool isMovingElement;

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

	public TileElement ElementAt(Location loc)
	{
		return ElementAt(loc.x, loc.y);
	}
	
	public bool InBounds(Location id) {
		return 0 <= id.x && id.x < elementCountX 
			 && 0 <= id.y && id.y < elementCountY;
	}
	
	public List<Location> Neighbors(Location id)
	{
		var ret = new List<Location>();
		for (int i = 0; i < Directions.Length; i++)
		{
			var dir = Directions[i];
			Location neighbour = new Location(id.x + dir.x, id.y + dir.y);
			if (InBounds(neighbour) && ElementAt(neighbour).IsEmpty) {
				ret.Add(neighbour);
			}
		}
		return ret;
	}

	public void OnTileElementClick(TileElement newTile)
	{
		if (isMovingElement)
			return;

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
			var path = Pathfinding.FindPath(SelectedTile.Location, newTile.Location);
			if (path.Count == 0)
			{
				return;
			}

			StartCoroutine(DoMovingAnimation(path));
		}
	}

	public void GenerateNewMap(int numOfElements = 6, List<TileElement.ElementType> possibleElements = null)
	{
		movingImage.gameObject.SetActive(false);
		
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
				ElementAt(x,y).Initialize(x, y);
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

	#region Moving animation

	private Vector2 CalculateAnchoredPosition(Location loc)
	{
		return new Vector2(
			startOffset.x + loc.x * tileSize.x + (loc.x - 1) * tileSpacing.x,
			startOffset.y - loc.y * tileSize.y - (loc.y - 1) * tileSpacing.y
		);
	}

	private IEnumerator DoMovingAnimation(List<Location> path)
	{
		isMovingElement = true;
		SetButtonsInteractive(false);
		
		// activate moving image
		var movingType = SelectedTile.Type;
		movingImage.sprite = SelectedTile.Sprite;
		movingImage.gameObject.SetActive(true);
		var pos = CalculateAnchoredPosition(SelectedTile.Location);
		movingImage.rectTransform.anchoredPosition = pos;
		
		// unselect start tile, and clear it's type
		SelectedTile.Unselect();
		SelectedTile.SetType(TileElement.ElementType.None);
		SelectedTile = null;

		for (int i = 0; i < path.Count; i++)
		{
			var target = CalculateAnchoredPosition(path[i]);
			Go.to(movingImage.rectTransform, movementDuration, 
				new GoTweenConfig().vector2Prop("anchoredPosition", target)
				.setEaseType(movementEaseType)
			);
			
			yield return new WaitForSecondsRealtime(movementDuration / 2f);
			
			ElementAt(path[i]).CrossFade(movementDuration * 4f);
			
			yield return new WaitForSecondsRealtime(movementDuration / 2f);
			
		}
		
		// last one
		ElementAt(path[path.Count - 1]).SetType(movingType);
		SetButtonsInteractive(true);
		isMovingElement = false;
	}

	private void SetButtonsInteractive(bool interactable)
	{
		for (int i = 0; i < tileElements.Count; i++)
		{
			tileElements[i].Button.interactable = interactable;
		}
	}

	#endregion
}
