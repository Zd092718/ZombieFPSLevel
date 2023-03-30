using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.W))
        //{
        //    anim.SetBool("IsWalking", true);
        //} else
        //{
        //    anim.SetBool("IsWalking", false);
        //}

        //if (Input.GetKey(KeyCode.R))
        //{
        //    anim.SetBool("IsRunning", true);
        //} else
        //{
        //    anim.SetBool("IsRunning", false);
        //}       
        //if (Input.GetKey(KeyCode.A))
        //{
        //    anim.SetBool("IsAttacking", true);
        //} else
        //{
        //    anim.SetBool("IsAttacking", false);
        //}

        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    anim.SetTrigger("Death");
        //}


    }
}
