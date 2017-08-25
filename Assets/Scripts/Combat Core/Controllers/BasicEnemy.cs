﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Controller
{
	/* Instance Vars */
	private BehaviorState[] states;

	[SerializeField]
	private GameObject[] patrolPoints;
	private int currentPoint;

	private GameObject pursuitTarget;

	private Vector2 direction;

	/* Instance Methods */
	public override void Awake ()
	{
		base.Awake ();
		states = new BehaviorState[] {
			new BehaviorState("patrolling", this.u_patrol, this.fu_patrol, this.lu_default),
			new BehaviorState("pursuit", this.u_pursuit, this.fu_pursuit, this.lu_default)
		};
		setState (states [0]);
	}

	public void Start()
	{
		self.addAbility (Ability.get ("Stun Gun"));
	}

	/* BehaviorState Methods */
	private void lu_default()
	{

	}

	// --- Patrolling ---
	private void u_patrol()
	{

	}
	private void fu_patrol()
	{
		Vector2 movementVector = Vector2.zero;

		if (Vector2.Distance (transform.position, patrolPoints [currentPoint].transform.position) > 0.3f)
		{
			move ((Vector2)patrolPoints [currentPoint].transform.position);
		}
		else
		{
			currentPoint = (currentPoint + 1) % patrolPoints.Length;
		}

		RaycastHit2D hit;
		hit = Physics2D.CircleCast (transform.position, 2f, Vector2.zero, 0f, 1 << 9);
		if (hit.collider != null && hit.collider.GetComponent<Entity>() != null)
		{
			setState (states [1]);
			pursuitTarget = hit.collider.gameObject;
		}
	}

	// --- Pursuit ---
	private void u_pursuit()
	{
		if (pursuitTarget != null && Vector2.Distance (transform.position, pursuitTarget.transform.position) < 0.5f)
			useAbility (0, Vector2.zero, pursuitTarget.GetComponent<Entity> ());
	}
	private void fu_pursuit()
	{
		if (Vector2.Distance (transform.position, pursuitTarget.transform.position) > 4f)
		{
			pursuitTarget = null;
			setState (states [0]);
		}

		if (pursuitTarget != null)
		{
			self.movespeed.lockTo (15);
			move ((Vector2)pursuitTarget.transform.position);
		}
		else
		{
			self.movespeed.unlock ();
			setState (states [0]);
		}
	}

	private void move(Vector2 targetLocation)
	{
		Vector2 movementVector = Vector2.zero;

		bool up = targetLocation.y - transform.position.y > 0.1f;
		bool left = targetLocation.x - transform.position.x < -0.1f;
		bool down = targetLocation.y - transform.position.y < -0.1f;
		bool right = targetLocation.x - transform.position.x > 0.1f;

		if (up)
			movementVector += Vector2.up;
		if (left)
			movementVector += Vector2.left;
		if (down)
			movementVector += Vector2.down;
		if (right)
			movementVector += Vector2.right;

		physbody.AddForce (movementVector * self.movespeed.value);

		if (movementVector != Vector2.zero)
			direction = movementVector;
		facePoint (direction + (Vector2)transform.position);
	}
}
