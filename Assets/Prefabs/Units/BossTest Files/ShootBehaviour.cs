using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : StateMachineBehaviour
{
    public float speed;
    public float stopDistance;
    public float backDistance;
    private float timeBetweenShots;
    public float startTimeBetweenShots;

    public GameObject projectile;
    private Transform player;
    public float PhaseTimer;
    private float startPhaseTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManagerScript.PlaySound("icandoanything");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
        startPhaseTimer = PhaseTimer;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(Vector2.Distance(animator.transform.position, player.position) > stopDistance)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(animator.transform.position, player.position) < stopDistance && Vector2.Distance(animator.transform.position, player.position) > backDistance)
        {
            animator.transform.position = animator.transform.position;
        }
        else if (Vector2.Distance(animator.transform.position, player.position) < backDistance)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, player.position, -speed * Time.deltaTime);
        }

        if(timeBetweenShots <= 0)
        {
            Vector3 direction = (player.position - animator.transform.position);
            direction.Normalize();
            float angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;
            //Create projectile and define its orientation
            Instantiate(projectile, animator.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));          
            
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

        if (startPhaseTimer <= 0)
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isFollowing", true);
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
