using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackBarController : MonoBehaviour {
	private Slider specialBar;
	private float lastUp;
	private float upSpeed = 1f;
	private Text text;
	private Image fillColor;
    private bool charged = false;
    private bool activated = false;

	void Start () {
		specialBar = GetComponent<Slider>();
		text = gameObject.transform.GetChild(4).gameObject.GetComponent<Text>();
		fillColor = gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
	}

	void Update () {
		if (Time.time > lastUp + upSpeed)
		{
			lastUp = Time.time;
			specialBar.value += 1;
		}
		CheckCharged();
	}

	void CheckCharged()
	{
		if (charged == false && specialBar.value == specialBar.maxValue)
		{
			fillColor.color = new Color32(0x00, 0xEE, 0xEE, 0xFF);
			text.text = "Press E to activate";
            charged = true;
		}
	}

    public void ToggleActivate()
    {
        if (!activated)
        {
            text.text = "LMB/RMB to use";
            activated = true;
        }
        else
        {
            text.text = "Press E to activate";
            activated = false;
        }
    }

	public void AddCharge(int charge)
	{
		specialBar.value += charge;
		CheckCharged();
	}

    public bool CanUseSpecial()
    {
        return charged;
    }

	public void UseSpecial() {
        specialBar.value = 0;
        fillColor.color = new Color32(0x11, 0x00, 0xEE, 0xFF);
        charged = false;
        activated = false;
        text.text = "Charging";
    }
}
