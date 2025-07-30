using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class move : MonoBehaviour
{
    public float speed = 1f;       // Movement speed
    public float attackDistance = 1.5f;
    public float attackDelay = 0.5f;
    public bool attacking = false;
    public bool running = false;

    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the AR camera (the player)
        Transform playerTransform = Camera.main.transform;
        Vector3 direction = (playerTransform.position - transform.position);
        direction = new Vector3(direction.x,0,direction.z);
        
        if(direction.magnitude < attackDistance)
        {
            running = false;
            if(attacking == false)
            {
                attacking = true;
                StartCoroutine(Attack());
            }
            return;
        }
        else
        {
            StopAllCoroutines();
            attacking = false;
            if(running == false)
            {
                running = true;
                anim.CrossFadeInFixedTime("Idle",.25f);
            }
            transform.position += direction.normalized * speed * Time.deltaTime;
            transform.forward = direction;
            anim.SetFloat("Speed",speed);
        }
    }

    IEnumerator Attack()
    {   
        while(attacking == true)
        {
            Debug.Log("Attack");
            int animNum = (int)Random.Range(0,3);
            anim.SetFloat("Random",animNum);
            anim.CrossFadeInFixedTime("Attack",0.25f);
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
