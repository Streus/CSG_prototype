using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
	[SerializeField]
	private float duration = 0.1f;

	public void Update()
	{
		duration -= Time.deltaTime;
		if (duration < 0f)
			Destroy (gameObject);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<BasicEnemy> () != null)
		{
			col.GetComponent<BasicEnemy> ().distract ((Vector2)transform.position);
		}
	}
}
