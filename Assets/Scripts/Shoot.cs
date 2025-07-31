using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
    public Button button;
    public Animator anim;
    public ParticleSystem flash;
    public bool canShoot = true;
    public float delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if(canShoot == true)
        {
            anim.CrossFadeInFixedTime("Shoot",0.25f);
            flash.Play();
            StartCoroutine(ShootDelay());
            Transform cam = transform.parent;
            RaycastHit hit;
            if(Physics.Raycast(cam.position,cam.forward,out hit, 20))
            {
                if(hit.transform.gameObject.tag == "Sahur" || hit.transform.gameObject.tag == "brr brr patapim (1)")
                {
                    move sahur = hit.transform.gameObject.GetComponent<move>();
                    sahur.health -= 1;
                    if(sahur.health <= 0)
                    {
                        sahur.gameObject.GetComponent<Collider>().enabled = false;
                        Destroy(sahur.gameObject.GetComponent<Rigidbody>());
                        sahur.anim.CrossFadeInFixedTime("Death",.25f);
                        sahur.attacking = false;
                        sahur.running = false;
                        sahur.enabled = false;
                    }
                }
            }
        }
    }

    IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
