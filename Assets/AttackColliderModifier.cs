using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class attackColliderModifier : StateMachineBehaviour {

    private Player player;
    private Transform playerTransform;
    private GameObject mainSpell;

    private BoxCollider2D playerCollider;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        mainSpell = Instantiate(Resources.Load("Nova"), playerTransform) as GameObject;

        if (player.lastLookDir < 0)
        {
            mainSpell.transform.localPosition = new Vector3(-mainSpell.transform.localPosition.x, mainSpell.transform.localPosition.y, mainSpell.transform.localPosition.z);
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy(mainSpell);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
