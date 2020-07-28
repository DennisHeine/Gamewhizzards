using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoCollission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public AudioClip impact;
    public bool Stoped;
    // Update is called once per frame
    void Update()
    {
      
      
            
      
    }
   
    //private string myname="";
    public bool isAttacking = false;
    public bool isDead = false;
    private System.DateTime lastAttack=System.DateTime.Now;
 

    System.Threading.Thread t;


    private void OnTriggerExit(Collider collision)
    {
       if (collision.gameObject.tag == "Player")
        {
            isAttacking = false;
            Stoped=false;
        }
        
    }

    private void OnTriggerStay(Collider collision)
    {
       
        if ((collision.gameObject.tag == "Player"))
        {
            Stoped=true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {

        
        bool obj1Attacking = false;
        bool obj2Attacking = false;

       
        if ((collision.gameObject.tag == "Player"))
        {
            Stoped=true;
        }
     
        
    }

}
