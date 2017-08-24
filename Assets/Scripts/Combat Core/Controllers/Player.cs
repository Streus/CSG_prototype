using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
	/* Instance Vars */

	[SerializeField]
	private KeyCode key_up;
	[SerializeField]
	private KeyCode key_left;
	[SerializeField]
	private KeyCode key_down;
	[SerializeField]
	private KeyCode key_right;
	[SerializeField]
	private KeyCode useAbility;
	[SerializeField]
	private string abilityName;

	// A list of the states
	private BehaviorState[] states;

	private Vector2 direction;

	/* Instance Methods */
	public override void Awake ()
	{
		base.Awake ();
		setState (new BehaviorState("prime", this.updatePrime, this.fixedUpdatePrime, this.lateUpdatePrime));

		direction = Vector2.zero;
	}

	// Inject some test data into self
	public void Start()
	{
		if(abilityName != "")
			self.addAbility (Ability.get(abilityName));
	}

	private void updatePrime()
	{
		//invoke abilities
		if (Input.GetKey (useAbility)) //TODO swap for proper bindings later
			useAbility (0, Vector2.zero);
	}

	private void fixedUpdatePrime()
	{
		//movement
		Vector2 movementVector = Vector2.zero;

		bool up = Input.GetKey(key_up); //TODO swap for proper bindings later
		bool left = Input.GetKey(key_left);
		bool down = Input.GetKey(key_down);
		bool right = Input.GetKey(key_right);

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

	private void lateUpdatePrime()
	{

	}
}
