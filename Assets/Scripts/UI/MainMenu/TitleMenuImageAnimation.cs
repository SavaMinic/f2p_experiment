using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuImageAnimation : MonoBehaviour
{

	[SerializeField]
	private float minScale = 0.85f;
	
	[SerializeField]
	private float maxScale = 1.15f;
	
	[SerializeField]
	private float duration = 10f;

	private RectTransform myRect;
	private bool isGrowing;
	
	private void Start()
	{
		myRect = GetComponent<RectTransform>();
		isGrowing = Random.value > 0.5f;
		StartCoroutine(DoShrinkAndGrow());
	}

	private IEnumerator DoShrinkAndGrow()
	{
		yield return new WaitForSecondsRealtime(Random.value * 2f);
		while (true)
		{
			Go.to(myRect, duration, new GoTweenConfig().vector3Prop("localScale", (isGrowing ? maxScale : minScale) * Vector3.one));
			yield return new WaitForSecondsRealtime(duration);
			isGrowing = !isGrowing;
		}
	}
}
