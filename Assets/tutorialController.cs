using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class tutorialController : MonoBehaviour {

    GameObject mainCanvas;
    Player playerComp;
    private dummyEnemy dummy;
    private ResourceBarController resourceBar;
    private SpecialAttackBarController specialBar;

    public bool inCombatSection;
    public bool inSpecialSection;

    private bool movementDone;
    private bool dropHelperDone;
    private bool choiceMakerDone;



	// Use this for initialization
	void Start () {
        mainCanvas = GameObject.FindWithTag("MainCanvas") as GameObject;
	    resourceBar = GameObject.FindObjectOfType<ResourceBarController>();
	    specialBar = GameObject.FindObjectOfType<SpecialAttackBarController>();
	    playerComp = FindObjectOfType<Player>();
	    dummy = FindObjectOfType<dummyEnemy>();
	    inCombatSection = false;
	    inSpecialSection = false;
	    movementDone = false;
	    dropHelperDone = false;
        choiceMakerDone = false;


        StartCoroutine(coMovementControls());
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playerComp.transform.position.x > 52 && dropHelperDone == false)
	    {
	        StartCoroutine(dropHelper());
	        dropHelperDone = true;
	    }

        if (playerComp.transform.position.x > 111 && choiceMakerDone == false)
        {
            StartCoroutine(choiceMaker());
        }



    }

    private IEnumerator choiceMaker()
    {
        choiceMakerDone = true;
        GameObject message = Instantiate(Resources.Load("tutorial_menus/levelDecisionHelper"), mainCanvas.transform) as GameObject;
        yield return new WaitForSeconds(5f);
        Destroy(message);

    }

    private IEnumerator dropHelper()
    {
        yield return new WaitForSeconds(5f);
        if (playerComp.transform.position.x < 60 && playerComp.transform.position.y > 4f)
        {
            GameObject message = Instantiate(Resources.Load("tutorial_menus/movementControls"), mainCanvas.transform) as GameObject;
            message.GetComponent<Text>().text = "Drop through the platform by pushing S!";
            while (playerComp.transform.position.y > 4f)
            {
                yield return null;
            }
            Destroy(message);
        }
    }

    private IEnumerator coMovementControls()
    {
        GameObject message = Instantiate(Resources.Load("tutorial_menus/movementControls"), mainCanvas.transform) as GameObject;

        while (!Input.GetKeyDown(KeyCode.D))
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Press space to Jump";

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Press space 2x to double jump";

        while (!playerComp.isDoubleJumping)
        {
            yield return null;
        }

        Destroy(message);

        movementDone = true;

        StartCoroutine(coCombatControls());

    }

    private IEnumerator coCombatControls()
    {
        GameObject message = Instantiate(Resources.Load("tutorial_menus/combatControls"), mainCanvas.transform) as GameObject;
        inCombatSection = true;
        while (!dummy.hasBeenHitMelee)
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Using melee attacks fills the resource bar with white";

        GameObject indicatorArrow = Instantiate(Resources.Load("tutorial_menus/meleeIndicator"), mainCanvas.transform) as GameObject;
        yield return new WaitForSeconds(3f);
        Destroy(indicatorArrow);

        message.GetComponent<Text>().text = "Right click for ranged attacks \n Try it on the dummy!";
        while (!dummy.hasBeenHitRanged)
        {
            yield return null;
        }
        message.GetComponent<Text>().text = "Using ranged attacks fills the resource bar with black";
        indicatorArrow = Instantiate(Resources.Load("tutorial_menus/rangedIndicator"), mainCanvas.transform) as GameObject;
        yield return new WaitForSeconds(3f);
        Destroy(indicatorArrow);

        message.GetComponent<Text>().text = "Try filling the resource bar with either color!";

        while (resourceBar.CanAttackMelee() && resourceBar.CanAttackRanged())
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Doing this disables that attack type temporarily\nuntil enough resource is available.";
        yield return new WaitForSeconds(5f);

        message.GetComponent<Text>().text = "So be sure to balance your attacks!";
        yield return new WaitForSeconds(3f);


        inSpecialSection = true;
        message.GetComponent<Text>().text = "Landing attacks fills your special bar.";
        indicatorArrow = Instantiate(Resources.Load("tutorial_menus/specialIndicator"), mainCanvas.transform) as GameObject;
        yield return new WaitForSeconds(3f);
        Destroy(indicatorArrow);

        while(!specialBar.CanUseSpecial())
        {
            yield return null;
        }
        message.GetComponent<Text>().text = "When your special bar changes color, you can\nqueue up a special attack with E. Try it out!";

        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Once you've queued it, use either attack\nto use their respective specials.";
        yield return new WaitForSeconds(3f);


        message.GetComponent<Text>().text = "Try a special attack on the dummy!";

        while (!dummy.hasBeenHitSpecial)
        {
            yield return null;
        }

        message.GetComponent<Text>().text = "Nice work! You've mastered combat controls.\nGood luck!";
        yield return new WaitForSeconds(3f);
        Destroy(message);

        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner s in spawners)
        {
            s.spawnerActive = true;
        }

        for (int i  = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }


}
