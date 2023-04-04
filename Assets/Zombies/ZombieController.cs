using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private GameObject ragdoll;
    private Animator anim;
    private NavMeshAgent agent;

    private enum State { IDLE, WANDER, ATTACK, CHASE, DEAD };
    private State state = State.IDLE;

    public GameObject Target { get => target; set => target = value; }
    public GameObject Ragdoll { get => ragdoll; set => ragdoll = value; }
    private State ZombieState { get => state; set => state = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }


    public void TurnOffTriggers()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsAttacking", false);
        Anim.SetBool("IsRunning", false);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(Target.transform.position, transform.position);
    }

    private bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 20)
        {
            return true;
        }

        return false;
    }

    private bool AbandonChase()
    {
        if (DistanceToPlayer() > 40)
        {
            return true;
        }
        return false;
    }

    public void KillZombie()
    {
        TurnOffTriggers();
        agent.enabled = false;
        anim.SetTrigger("Death");
        state = State.DEAD;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    int randomNum = Random.Range(0, 10);
        //    if (randomNum <= 5)
        //    {
        //        GameObject rd = Instantiate(ragdoll, this.transform.position, this.transform.rotation);
        //        rd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000);
        //        Destroy(this.gameObject);
        //    } else if(randomNum > 5)
        //    {
        //        TurnOffTriggers();
        //        agent.enabled = false;
        //        anim.SetTrigger("Death");
        //        state = State.DEAD;
        //    }
        //    return;
        //}
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }
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
                if (!Agent.hasPath)
                {
                    float newX = this.transform.position.x + Random.Range(-5, 5);
                    float newZ = this.transform.position.z + Random.Range(-5, 5);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));
                    Vector3 dest = new Vector3(newX, newY, newZ);
                    Agent.SetDestination(dest);
                    Agent.stoppingDistance = 0;
                    Agent.speed = walkingSpeed;
                    TurnOffTriggers();
                    Anim.SetBool("IsWalking", true);
                }
                if (CanSeePlayer())
                {
                    state = State.CHASE;
                }
                else if (Random.Range(0, 5000) < 5)
                {
                    state = State.IDLE;
                    TurnOffTriggers();
                    Agent.ResetPath();
                }
                break;
            case State.CHASE:
                Agent.SetDestination(Target.transform.position);
                Agent.stoppingDistance = 3;
                Agent.speed = runningSpeed;
                TurnOffTriggers();
                Anim.SetBool("IsRunning", true);

                if (Agent.remainingDistance <= Agent.stoppingDistance && !Agent.pathPending)
                {
                    state = State.ATTACK;
                }

                if (AbandonChase())
                {
                    state = State.WANDER;
                    Agent.ResetPath();
                }

                break;
            case State.ATTACK:
                TurnOffTriggers();
                Anim.SetBool("IsAttacking", true);
                transform.LookAt(Target.transform.position + new Vector3(0, -1, 0));
                if (DistanceToPlayer() > Agent.stoppingDistance + 2)
                {
                    state = State.CHASE;
                }
                break;
            case State.DEAD:
                break;
        }


    }
}
