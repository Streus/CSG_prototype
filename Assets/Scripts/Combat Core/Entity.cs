using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
	[SerializeField]
	private Faction faction;
	public Faction getFaction() { return faction; }

	public Stat movespeed = new Stat(0, 0, 25);

	public Stat sleepDarts = new Stat (0, 0, 3);

	private Stat stunned;
	private Stat sleeping;

	private List<Status> statuses;

	[SerializeField]
	private List<Ability> abilities = new List<Ability>();

	public void Awake()
	{
		stunned = new Stat (0, 0);
		sleeping = new Stat (0, 0);

		statuses = new List<Status> ();
	}

	public void Update()
	{
		//update all statuses
		foreach (Status s in statuses)
			s.updateDuration (this, Time.deltaTime);

		//update all abilities
		foreach (Ability a in abilities)
			a.updateCooldown (Time.deltaTime);
	}

	// Add a status to the Entity and begin listening for its end
	public void addStatus(Status s)
	{
		Status existing = statuses.Find (delegate(Status obj) { return obj.Equals(s); });
		if (existing != null)
		{
			//a status of this type is already on this Entity
			existing.stack (this, 1);
			return;
		}

		//this status is new to this Entity
		statuses.Add (s);
		s.durationCompleted += removeStatus;
		s.OnApply (this);

		//notify listeners
		if (statusAdded != null)
			statusAdded (s);
	}

	// Either a status naturally ran out, or it is being manually removed
	public void removeStatus(Status s)
	{
		s.OnRevert (this);
		s.durationCompleted -= removeStatus;

		//notify listeners
		if (statusAdded != null)
			statusRemoved (s);
	}

	// --- Ability Handling ---

	// Add an ability to this Entity
	public void addAbility(Ability a)
	{
		//add the ability and set it to active
		abilities.Add (a);
		a.active = true;

		//notify listeners
		if (abilityAdded != null)
			abilityAdded (a);
	}

	// Remove an ability from this Entity
	public void removeAbility(Ability a)
	{
		//remove the ability and set it to inactive
		abilities.Remove (a);
		a.active = false;

		//notify listeners
		if (abilityRemoved != null)
			abilityRemoved (a);
	}
	#pragma warning disable 0168
	public void removeAbility(int index)
	{
		try
		{
			removeAbility(abilities[index]);
		}
		catch(IndexOutOfRangeException ioore)
		{
			Debug.LogError ("Attempted to remove a non-existant Ability."); //DEBUG
		}
	}

	// Swap the ability at index for a new ability. Returns the the old ability (null if fails)
	public Ability swapAbility(Ability a, int index)
	{
		//swap 'em
		Ability old = null;
		try
		{
			old = abilities [index];
			abilities [index] = a;
			a.active = true;
			old.active = false;
		}
		catch(IndexOutOfRangeException ioore)
		{
			Debug.LogError ("Tried to swap out non-existant Ability"); //DEBUG
			return null;
		}

		if (abilitySwapped != null)
			abilitySwapped (a, old, index);

		return old;
	}
	#pragma warning restore 0168

	// Ability getter
	public Ability getAbility(int index)
	{
		return abilities [index];
	}

	public bool setStunned(bool val)
	{
		if (val)
		{
			stunned += 1;

			foreach (Status s in statuses)
				s.OnStunned (this);
		}
		else
			stunned -= 1;
		return isStunned ();
	}
	public bool isStunned()
	{
		return stunned.value > 0;
	}

	public bool setSleeping(bool val)
	{
		if (val)
		{
			sleeping += 1;

			foreach (Status s in statuses)
				s.OnSleeping (this);
		}
		else
			sleeping -= 1;
		return isSleeping ();
	}
	public bool isSleeping()
	{
		return sleeping.value > 0;
	}

	// This Entity has died
	public void OnDeath()
	{
		foreach (Status s in statuses)
			s.OnDeath (this);

		if (died != null)
			died ();

		Destroy(gameObject);
	}

	public delegate void StatusChanged(Status s);
	public event StatusChanged statusAdded;
	public event StatusChanged statusRemoved;

	public delegate void AbilityChanged(Ability a);
	public event AbilityChanged abilityAdded;
	public event AbilityChanged abilityRemoved;
	public delegate void AbilitySwap(Ability a, Ability old, int index);
	public event AbilitySwap abilitySwapped;

	public delegate void EntityGeneric();
	public event EntityGeneric died;

	public enum Faction
	{
		player, enemy, neutral
	}
}
