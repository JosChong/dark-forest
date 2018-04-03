using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarController : MonoBehaviour {
	private Slider resourceBar;
	private float lastDrain;
	private float drainSpeed = 1f;
	private int rangedCost = 2;
	private int meleeCost = 10;
	private int altRangedCost = 10;
	private int altMeleeCost = 20;
	
	void Start ()
	{
		resourceBar = GetComponent<Slider>();
		lastDrain = Time.time;
	}

	public void UpdateBar(String attackType)
	{
		if (attackType.Equals("Ranged"))
		{
			resourceBar.value += rangedCost;
		} else if (attackType.Equals("Melee"))
		{
			resourceBar.value -= meleeCost;
		} else if (attackType.Equals("AltRanged"))
		{
			resourceBar.value += altRangedCost;
		} else if (attackType.Equals("AltMelee"))
		{
			resourceBar.value -= altMeleeCost;
		}
	}
	
	public bool CanAttackRanged()
	{
		return resourceBar.value < resourceBar.maxValue - rangedCost;
	}

	public bool CanAttackMelee()
	{
		return resourceBar.value > 0 + meleeCost;
	}
	
	public bool CanAttackAltRanged()
	{
		return resourceBar.value < resourceBar.maxValue - altRangedCost;
	}

	public bool CanAttackAltMelee()
	{
		return resourceBar.value > 0 + altMeleeCost;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastDrain + drainSpeed)
		{
			lastDrain = Time.time;

            if (resourceBar.value > resourceBar.maxValue / 2)
            {
                resourceBar.value -= 1;
            }
            else if (resourceBar.value < resourceBar.maxValue / 2)
            {
                resourceBar.value += 1;
            }
        }
	}
}
