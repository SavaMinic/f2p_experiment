using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPanel : MonoBehaviour {

	#region Fields
	
	[SerializeField]
	private Text bonusText;
	
	[SerializeField]
	private float showDuration;

	[SerializeField]
	private Vector3 showOffset;

	[SerializeField]
	private GoEaseType showEaseType;

	private CanvasGroup myCanvasGroup;
	private RectTransform myRectTransform;

	#endregion

	#region Mono

	private void Awake()
	{
		myRectTransform = GetComponent<RectTransform>();
		myCanvasGroup = GetComponent<CanvasGroup>();
		myCanvasGroup.alpha = 0f;
	}

	#endregion

	#region Public

	public void ShowAnimation(int bonusMultiplier, Vector3 position)
	{
		bonusText.text = "x" + bonusMultiplier;
		myRectTransform.position = position;
		StartCoroutine(DoShowAnimation());
	}

	#endregion

	#region Private

	private IEnumerator DoShowAnimation()
	{
		var targetEnd = myRectTransform.position + showOffset;
		Go.to(myRectTransform, showDuration * 4f, new GoTweenConfig().vector3Prop("position", targetEnd).setEaseType(showEaseType));
		
		Go.to(myCanvasGroup, showDuration, new GoTweenConfig().floatProp("alpha", 1f));
		
		yield return new WaitForSecondsRealtime(3 * showDuration);
		Go.to(myCanvasGroup, showDuration, new GoTweenConfig().floatProp("alpha", 0f));
		
		yield return new WaitForSecondsRealtime(showDuration);
		
		Destroy(gameObject);
	}

	#endregion
}
