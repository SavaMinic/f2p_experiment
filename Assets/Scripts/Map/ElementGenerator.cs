using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;
using UnityEngine.UI;

public class ElementGenerator : MonoBehaviour
{

	
	#region Static

	private static ElementGenerator instance;
	public static ElementGenerator I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<ElementGenerator>();
			return instance;
		}
	}

	public struct SpawnLocation
	{
		public TileElement.ElementType element;
		public Location location;
	}

	#endregion

	#region Fields

	[SerializeField]
	private List<Image> nextElementImages;

	private List<SpawnLocation> currentSequence = new List<SpawnLocation>();
	
	private List<TileElement.ElementType> possibleTypes = new List<TileElement.ElementType>();

	#endregion

	#region Public

	public void SetPossibleTypes(List<TileElement.ElementType> possible)
	{
		possibleTypes = possible;
	}

	public List<SpawnLocation> GetCurrentSequenceAndGenerateNew()
	{
		var ret = currentSequence;
		
		currentSequence = new List<SpawnLocation>();
		// Generate random for now
		for (int i = 0; i < nextElementImages.Count; i++)
		{
			var newElement = possibleTypes.GetRandom();
			var emptyLocation = TileController.I.FindEmptyLocation();
			if (emptyLocation.Equals(TileController.InvalidLocation))
			{
				Debug.LogError("No empty locations");
				break;
			}
			
			currentSequence.Add(new SpawnLocation()
			{
				element = newElement,
				location = emptyLocation
			});
			nextElementImages[i].sprite = GameSettings.I.GetSpriteForElement(newElement);
		}

		return ret;
	}

	#endregion
}
