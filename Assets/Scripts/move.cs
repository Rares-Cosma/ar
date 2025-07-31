using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    public GameLogic manager;

    public float health = 2;
    public float speed = 1f;       // Movement speed
    public float attackDistance = 1.5f;
    public float attackDelay = 0.5f;
    public bool attacking = false;
    public bool running = false;

    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] objects = scene.GetRootGameObjects();

        foreach (var obj in objects)
        {
            if (obj.GetComponent<GameLogic>() != null)
            {
                manager = obj.GetComponent<GameLogic>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the AR camera (the player)
        Transform playerTransform = Camera.main.transform;
        Vector3 direction = (playerTransform.position - transform.position);
        direction = new Vector3(direction.x,0,direction.z);
        transform.forward = direction;

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
            float animLength = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength/2);
            if(attacking == true)
                manager.healthSystem.health -= 15;
            yield return new WaitForSeconds(attackDelay-animLength/2);
        }
    }
}
