using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    private Animator anim;
    private NavMeshAgent agent;

    private enum State { IDLE, WANDER, ATTACK, CHASE, DEAD };
    private State state = State.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void TurnOffTriggers()
    {
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsRunning", false);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    private bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 10)
        {
            return true;
        }

        return false;
    }

    private bool AbandonChase()
    {
        if (DistanceToPlayer() > 20)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                if (CanSeePlayer())
                {
                    state = State.CHASE;
                }
                else if (Random.Range(0, 5000) < 5)
                {
                    state = State.WANDER;
                }
                break;
            case State.WANDER:
                if (!agent.hasPath)
                {
                    float newX = this.transform.position.x + Random.Range(-5, 5);
                    float newZ = this.transform.position.z + Random.Range(-5, 5);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));
                    Vector3 dest = new Vector3(newX, newY, newZ);
                    agent.SetDestination(dest);
                    agent.stoppingDistance = 0;
                    agent.speed = walkingSpeed;
                    TurnOffTriggers();
                    anim.SetBool("IsWalking", true);
                }
                if (CanSeePlayer())
                {
                    state = State.CHASE;
                } else if(Random.Range(0, 5000) < 5)
                {
                    state = State.IDLE;
                    TurnOffTriggers();
                    agent.ResetPath();
                }
                break;
            case State.CHASE:
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 3;
                agent.speed = runningSpeed;
                TurnOffTriggers();
                anim.SetBool("IsRunning", true);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = State.ATTACK;
                }

                if (AbandonChase())
                {
                    state = State.WANDER;
                    agent.ResetPath();
                }

                break;
            case State.ATTACK:
                TurnOffTriggers();
                anim.SetBool("IsAttacking", true);
                transform.LookAt(target.transform.position + new Vector3(0, -1, 0));
                if (DistanceToPlayer() > agent.stoppingDistance + 2)
                {
                    state = State.CHASE;
                }
                break;
            case State.DEAD:
                break;
        }


    }
}
