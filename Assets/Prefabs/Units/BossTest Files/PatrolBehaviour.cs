using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private PatrolSpots patrol;
    public float speed;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public float PhaseTimer;
    private float startPhaseTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        patrol = GameObject.FindGameObjectWithTag("PatrolSpots").GetComponent<PatrolSpots>();
        randomSpot = Random.Range(0, patrol.patrolPoints.Length);
        waitTime = startWaitTime;
        startPhaseTimer = PhaseTimer;
        SoundManagerScript.PlaySound("chaoschaos");
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, patrol.patrolPoints[randomSpot].position) > 0.2f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrol.patrolPoints[randomSpot].position, speed * Time.deltaTime);
        }
        else
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, patrol.patrolPoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }

                
        }
        
        if (startPhaseTimer <= 0)
        {
            animator.SetBool("isShooting", true);
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isFollowing", false);
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
