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

	[SerializeField]
	private float swapAnimationTime;

	[SerializeField]
	private GoEaseType swapEaseType;

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
			StartCoroutine(DoSwapSpriteAnimation(nextElementImages[i], newElement));
		}

		return ret;
	}

	#endregion

	#region Private

	private IEnumerator DoSwapSpriteAnimation(Image elementImage, TileElement.ElementType newElement)
	{
		Go.to(elementImage.rectTransform, swapAnimationTime / 2f, 
			new GoTweenConfig().vector3Prop("localScale", Vector3.zero).setEaseType(swapEaseType)
		);
		
		yield return new WaitForSecondsRealtime(swapAnimationTime / 2f);
		elementImage.sprite = GameSettings.I.GetSpriteForElement(newElement);
		
		Go.to(elementImage.rectTransform, swapAnimationTime / 2f, 
			new GoTweenConfig().vector3Prop("localScale", Vector3.one).setEaseType(swapEaseType)
		);
	}

	#endregion
}
