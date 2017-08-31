using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{
	public class SleepDart : Bullet
	{
		protected override void OnHit (Collider2D col)
		{
			base.OnHit (col);

			Instantiate(Resources.Load<GameObject> ("Prefabs/Bullets/NoiseMaker"), transform.position, Quaternion.identity);
		}
	}
}
