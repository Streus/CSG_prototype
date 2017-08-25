using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

public class Destructable : MonoBehaviour
{
	/* Instance Vars */
	[SerializeField]
	private float health;

	/* Instance Methods */
	public void Start()
	{
		Entity parEnt = GetComponentInParent<Entity> ();
		Destructable parDes = GetComponentInParent<Destructable> ();

		if (parEnt != null)
			parEnt.died += OnDeath;
		else if (parDes != null)
			parDes.destructed += OnDeath;
	}

	public void damage(float amount)
	{
		OnHit (amount);

		health -= amount;
		if (health <= 0f)
			OnDeath ();
	}

	protected virtual void OnHit(float damage)
	{
		OnDeath ();
	}

	public virtual void OnDeath()
	{
		if (destructed != null)
			destructed ();

		Destroy (gameObject);
	}

	public void OnDestroy()
	{
		//create stuff in here and I'll cut you
	}

	/* Events and Delegates */
	public delegate void DestructableDestroyed();
	public event DestructableDestroyed destructed;
}
