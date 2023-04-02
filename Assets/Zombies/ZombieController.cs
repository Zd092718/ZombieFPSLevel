using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Animator anim;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetBool("IsWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);

        if(agent.remainingDistance > agent.stoppingDistance)
        {
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsAttacking", false);
        } else
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsAttacking", true);
        }
        /*if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsAttacking", true);
        }
        else
        {
            anim.SetBool("IsAttacking", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("Death");
        }*/


    }
}
