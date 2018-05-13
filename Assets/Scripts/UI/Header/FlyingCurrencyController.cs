using System;
using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;

public class FlyingCurrencyController : MonoBehaviour
{
	#region Static

	private static FlyingCurrencyController instance;
	public static FlyingCurrencyController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<FlyingCurrencyController>();
			return instance;
		}
	}

	#endregion

	#region Fields

	[SerializeField]
	private GameObject flyingCurrencyPrefab;

	[SerializeField]
	private float flyingDuration;

	[SerializeField]
	private GoEaseType flyingEaseType;

	#endregion

	#region Public

	public void AnimateFlyingCurrency(Vector3 startPosition, Vector3 endPosition, Action callback = null)
	{
		var flyingCurrency = Instantiate(flyingCurrencyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		flyingCurrency.transform.SetParent(transform);
		flyingCurrency.transform.position = startPosition;

		StartCoroutine(DoAnimateFlyingCurrency(flyingCurrency, endPosition, callback));
	}

	#endregion

	#region Animations

	private IEnumerator DoAnimateFlyingCurrency(GameObject flyingCurrency, Vector3 endPosition, Action callback = null)
	{
		Go.to(flyingCurrency.transform, flyingDuration,
			new GoTweenConfig().vector3Prop("position", endPosition).setEaseType(flyingEaseType)
			.onComplete((t) =>
			{
				Destroy(flyingCurrency);
			})
		);
		
		// do callback somewhere before it ends
		yield return new WaitForSecondsRealtime(flyingDuration * 0.8f);
		
		callback.CallIfNotNull();
	}

	#endregion
}
