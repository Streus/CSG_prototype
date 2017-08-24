using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This file holds all the methods and dictionary entries used for abilities
 */
public partial class Ability
{
	/* Repo Instantiation */
	static Ability()
	{
		repository = new Dictionary<string, Ability> ();
		repository.Add ("Sleep Dart", new Ability ("Sleep Dart", "Fire a sleep dart", "", 5f, "sleepShoot", "checkNumSleepDarts"));
		repository.Add ("Interact", new Ability ("Interact", "Hack something.", "", 1f, "interact"));
	}

	/* Use Effects */

	// Just a basic bullet-shooting ability
	private bool sleepShoot(Entity subject, Vector2 targetPosition, params object[] args)
	{
		Bullet.create ("SleepDart", subject, subject.getFaction ());
		subject.sleepDarts -= 1;
		return true;
	}

	// Proto-hack ability
	private bool interact(Entity subject, Vector2 targetPosition, params object[] args)
	{
		return true; //TODO
	}

	/* Prereq Checks */

	// Check the number of sleep darts the subject has
	private bool checkNumSleepDarts(Entity subject)
	{
		return subject.sleepDarts.value > 0;
	}
}
