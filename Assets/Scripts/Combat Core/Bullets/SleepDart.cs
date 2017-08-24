using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{
	public class SleepDart : Bullet
	{
		protected override void OnEntHit (Collider2D col, Entity hit)
		{
			base.OnEntHit (col, hit);
			hit.setSleeping (true);
		}
	}
}
