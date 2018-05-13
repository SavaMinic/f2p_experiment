using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCurrency : MonoBehaviour
{

	[SerializeField]
	private float rotationSpeed;
	
	void Update()
	{
		if (!Application.isPlaying)
			return;
		
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
