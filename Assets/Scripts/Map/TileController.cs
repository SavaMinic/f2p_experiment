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
	
	private static readonly Location[] MovementDirections = new [] {
		new Location(1, 0), // to right of tile
		new Location(0, -1), // above tile
		new Location(-1, 0), // to left of tile
		new Location(0, 1), // below tile
	};
	
	private static readonly Location[] ExplosionDirection = new [] {
		new Location(1, 0), // to right of tile
		new Location(0, 1), // below tile
		new Location(1, 1), // diagonal bottom right
		new Location(-1, 1), // diagonal bottom left
	};

	public static Location InvalidLocation = new Location(-1, -1);

	#endregion
	
	#region Fields

	[SerializeField]
	private float topOffsetXIphone = -150f;

	[SerializeField]
	private List<TileElement> tileElements;

	[SerializeField]
	private int elementCountX = 7;

	[SerializeField]
	private int elementCountY = 10;

	[SerializeField]
	private int requiredCountForExplosion = 5;

	[Header("Movement")]
	[SerializeField]
	private Image movingImage;

	[SerializeField]
	private Vector2 tileSize;

	[SerializeField]
	private Vector2 tileSpacing;

	[SerializeField]
	private float movementDuration = 0.4f;

	[SerializeField]
	private GoEaseType movementEaseType;

	[Header("Explosions")]
	[SerializeField]
	private float waitNoExplosions;

	[SerializeField]
	private float waitAfterExplosions;

	[SerializeField]
	private float delayForExtraExplosionCheck = 0.5f;

	[Header("Bonus")]
	[SerializeField]
	private GameObject bonusAnimationPrefab;

	private bool isMovingElement;

	private RectTransform myRect;
	private Vector2 startOffset;
	
	#endregion

	#region Properties

	public TileElement SelectedTile { get; private set; }

	#endregion

	#region Mono

	private void Awake()
	{
		myRect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		movingImage.rectTransform.position = tileElements[0].RectTransform.position;
		startOffset = movingImage.rectTransform.anchoredPosition;
	}

	#endregion

	#region Public API

	public TileElement ElementAt(int x, int y)
	{
		if (x < 0 || x >= elementCountX || y < 0 || y >= elementCountY)
			return null;
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
		for (int i = 0; i < MovementDirections.Length; i++)
		{
			var dir = MovementDirections[i];
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
			if (SelectedTile != null)
			{
				SelectedTile.CantBeSelected();
				SelectedTile.Unselect();
			}
			SelectedTile = newTile;
			newTile.Select();
		}
		else if (SelectedTile != null)
		{
			var path = Pathfinding.FindPath(SelectedTile.Location, newTile.Location);
			if (path.Count == 0)
			{
				SelectedTile.CantBeSelected();
				return;
			}

			StartCoroutine(DoMovingAnimation(path));
		}
	}

	public void GenerateNewMap(int numOfElements, List<TileElement.ElementType> possibleElements)
	{
		movingImage.gameObject.SetActive(false);
		
		for (int y = 0; y < elementCountY; y++)
		{
			for (int x = 0; x < elementCountX; x++)
			{
				ElementAt(x,y).Initialize(x, y);
			}
		}

		while (numOfElements-- > 0)
		{
			var emptyLocation = FindEmptyLocation();
			if (emptyLocation.Equals(InvalidLocation))
			{
				Debug.LogError("No empty locations");
				break;
			}
			ElementAt(emptyLocation).SetType(possibleElements.GetRandom());
		}
	}

	public void AddNewElements(List<ElementGenerator.SpawnLocation> spawnLocations)
	{
		for (int i = 0; i < spawnLocations.Count; i++)
		{
			// if it has location for it, and it's not empty
			if (ElementAt(spawnLocations[i].location).IsEmpty)
			{
				// spawn it directly
				ElementAt(spawnLocations[i].location).Spawn(spawnLocations[i].element);
			}
			else
			{
				// find an random empty place to spawn it
				var emptyLocation = FindEmptyLocation();
				if (emptyLocation.Equals(InvalidLocation))
				{
					Debug.LogWarning("No empty locations");
					break;
				}
				ElementAt(emptyLocation).Spawn(spawnLocations[i].element);
			}
		}

		var emptyTiles = tileElements.CountAll(t => t.IsEmpty);
		if (emptyTiles == 0)
		{
			GameController.I.EndGame(GameController.EndGameType.NoEmptySpace);
			return;
		}

		StartCoroutine(CheckForExplosions(true));
	}

	public Location FindEmptyLocation()
	{
		int counter = 1000;
		while (counter-- > 0)
		{
			var randX = UnityEngine.Random.Range(0, elementCountX);
			var randY = UnityEngine.Random.Range(0, elementCountY);
			if (ElementAt(randX, randY).IsEmpty)
			{
				return new Location(randX, randY);
			}
		}
		
		// can't find it randomly, go linear
		for (int x = 0; x < elementCountX; x++)
		{
			for (int y = 0; y < elementCountY; y++)
			{
				if (ElementAt(x, y).IsEmpty)
				{
					return new Location(x, y);
				}
			}
		}

		// no empty space
		return InvalidLocation;
	}

	#endregion

	#region Moving animation

	private Vector2 CalculateAnchoredPosition(Location loc)
	{
		return new Vector2(
			startOffset.x + loc.x * tileSize.x + loc.x * tileSpacing.x,
			startOffset.y - loc.y * tileSize.y - loc.y * tileSpacing.y
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
		movingImage.gameObject.SetActive(false);
		isMovingElement = false;

		yield return CheckForExplosions();
	}
	
	#endregion

	#region Explosion animation

	private IEnumerator CheckForExplosions(bool skipNewTurn = false)
	{
		if (skipNewTurn)
		{
			yield return new WaitForSecondsRealtime(delayForExtraExplosionCheck);
		}
		
		var points = 0;
		var haveExplosions = false;
		// GO THROUGH EVERY ROW ...
		for (int y = 0; y < elementCountY; y++)
		{
			// ... AND EVERY COLUMN!
			for (int x = 0; x < elementCountX; x++)
			{
				// only non-empty elements
				var e = ElementAt(x, y);
				if (e.IsEmpty)
					continue;
				
				// CHECK EVERY DIRECTION FROM EACH ELEMENT
				for (int d = 0; d < ExplosionDirection.Length; d++)
				{
					var selectedElements = new List<TileElement>();
					selectedElements.Add(e);
					
					// continue to go down the direction
					var dir = ExplosionDirection[d];
					var neighbour = ElementAt(e.Location.x + dir.x, e.Location.y + dir.y);
					while (neighbour != null && neighbour.Type == e.Type)
					{
						selectedElements.Add(neighbour);
						neighbour = ElementAt(neighbour.Location.x + dir.x, neighbour.Location.y + dir.y);
					}
					
					// check how many we have in this direction
					if (selectedElements.Count >= requiredCountForExplosion)
					{
						haveExplosions = true;
						
						// DO EXPLOSION
						var pointsPerExplosion = 0;
						for (int i = 0; i < selectedElements.Count; i++)
						{
							pointsPerExplosion += selectedElements[i].Explode(i);
						}
						// if there is more than required, increase points multiplier
						var multiplier = selectedElements.Count - requiredCountForExplosion + 1;
						points += multiplier * pointsPerExplosion;
						
						// do some multiplier animation (x2, x3...)
						if (multiplier > 1 && pointsPerExplosion > 0)
						{
							Vector3 pos = Vector3.zero;
							for (int i = 0; i < selectedElements.Count; i++)
							{
								pos += selectedElements[i].RectTransform.position;
							}
							DoBonusAnimation(multiplier, pos / selectedElements.Count);
						}

						AnalyticsData.TrackLineExploded(selectedElements.Count, pointsPerExplosion);
					}
				}
			}
		}

		if (haveExplosions)
		{
			AudioController.I.PlayPositiveAlternateSFX();
		}
		GameController.I.Score += points;

		yield return new WaitForSecondsRealtime(haveExplosions ? waitAfterExplosions : waitNoExplosions);
		
		// start new turn
		if (!skipNewTurn)
		{
			GameController.I.NewTurn(!haveExplosions);
		}
		else
		{
			GameController.I.JustCheckForWin();
		}
	}

	private void DoBonusAnimation(int multiplier, Vector3 position)
	{
		var bonusPanel = Instantiate(bonusAnimationPrefab, transform.parent).GetComponent<BonusPanel>();
		bonusPanel.ShowAnimation(multiplier, position);
	}
	
	#endregion

	#region Private

	private void SetButtonsInteractive(bool interactable)
	{
		for (int i = 0; i < tileElements.Count; i++)
		{
			tileElements[i].Button.interactable = interactable;
		}
	}

	#endregion
}
