using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridAlign : MonoBehaviour 
{
	public Vector2 gridDivisor; 
	public Vector2 offset; 

	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR

		/*
		float newX = transform.position.x - (transform.position.x % gridDivisor.x) + offset.x; 
		float newY = transform.position.y - (transform.position.y % gridDivisor.y) + offset.y; 
		float newZ = transform.position.z - (transform.position.z % gridDivisor.z) + offset.z; 
		transform.position = new Vector3 (newX, newY, newZ); 
		*/ 


		transform.position = new Vector3(
			Mathf.Round( (transform.position.x - offset.x) / gridDivisor.x ) + offset.x,
			Mathf.Round( (transform.position.y - offset.y) / gridDivisor.y ) + offset.y,
			transform.position.z);

		#endif
	}
}
