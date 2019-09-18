using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior: StateMachineBehaviour
{
    private Transform playerPos;
    public float speed;
    public float PhaseTimer;
    private float startPhaseTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        startPhaseTimer = PhaseTimer;
        SoundManagerScript.PlaySound("laugh");
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, speed * Time.deltaTime);
        if (Vector2.Distance(animator.transform.position, playerPos.position) < 0.5)
        {
            startPhaseTimer = 0;
        }
        
        if (startPhaseTimer <= 0)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isFollowing", false);
            animator.SetBool("isShooting", false);
            startPhaseTimer = PhaseTimer;
        }
        else
        {
            startPhaseTimer -= Time.deltaTime;
        }
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
