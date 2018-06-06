using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainerController : MonoBehaviour
{
	[SerializeField]
	private float iphoneXOffset = 150f;

	private RectTransform myRectTransform;

	void Start()
	{
		myRectTransform = GetComponent<RectTransform>();
		
#if UNITY_IOS
		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
		{
			myRectTransform.offsetMax = new Vector2(myRectTransform.offsetMax.x, -iphoneXOffset);
		}
#endif
		
	}
}
