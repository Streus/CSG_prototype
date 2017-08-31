using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	/* Static Vars */


	/* Instance Vars */

	// The speed at which this bullet will begin traveling at instantiation
	[SerializeField]
	private int movespeed;

	// The faction of this bullet
	[SerializeField]
	private Entity.Faction faction;

	// Whether this bullet will be destroyed in a collision event
	[SerializeField]
	private bool destroyOnHit;

	// How long this bullet will exist without colliding with anything
	[SerializeField]
	private float duration;

	// The Entity that shot this bullet
	private Entity source;

	// References to important components
	private Collider2D colbody;
	private Rigidbody2D physbody;

	/* Static Methods */

	// Create a new bullet instance
	public static Bullet create(GameObject bullet, Entity source, Entity.Faction faction = Entity.Faction.neutral)
	{
		GameObject inst = (GameObject)Instantiate (bullet, source.transform.position, source.transform.rotation);
		Bullet b = inst.GetComponent<Bullet> ();
		b.faction = faction;
		b.source = source;
		return b;
	}
	public static Bullet create(string prefabName, Entity source, Entity.Faction faction = Entity.Faction.neutral)
	{
		GameObject go = Resources.Load<GameObject> ("Prefabs/Bullets/" + prefabName);
		return create (go, source, faction);

	}

	/* Instance Methods */
	public virtual void Awake()
	{
		colbody = GetComponent<Collider2D> ();
		physbody = GetComponent<Rigidbody2D> ();

		physbody.drag = 0f;
		physbody.AddForce (transform.up * movespeed, ForceMode2D.Impulse);
	}

	public virtual void Start()
	{

	}

	public virtual void Update()
	{
		duration -= Time.deltaTime;
		if (duration <= 0f)
		{
			OnDeath ();
		}
	}

	public virtual void FixedUpdate()
	{

	}

	public virtual void LateUpdate()
	{

	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		Entity e = col.GetComponent<Entity> ();
		Destructable d = col.GetComponent<Destructable> ();

		if (col.tag == "indes")
		{
			OnHit (col);
			OnDeath ();
		}
		else if (e != null)
		{
			if (faction != e.getFaction ())
			{
				OnEntHit (col, e);
				if (destroyOnHit)
					OnDeath ();
			}
		}
		else if (d != null)
		{
			OnHit (col);
		}
	}

	// Collision with an indestructable target
	public void OnCollisionEnter2D(Collision2D col)
	{
		OnHit (col.collider);
		OnDeath ();
	}

	protected virtual void OnEntHit(Collider2D col, Entity hit)
	{

	}
	protected virtual void OnHit(Collider2D col)
	{
		OnEntHit (col, null);
	}

	public virtual void OnDeath()
	{
		//finish up by destroying this bullet
		Destroy(gameObject);
	}

	// Create stuff in here and I'll knife you
	public void OnDestroy()
	{
		// Call the bulletDied event, which most of the time will remove this bullet
		//from any collision logs it might be in
		if (bulletDied != null)
			bulletDied (this);
	}

	// Event for Entities to listen to so they can cull their collison logs
	public delegate void BulletDeath(Bullet corpse);
	public event BulletDeath bulletDied;
}