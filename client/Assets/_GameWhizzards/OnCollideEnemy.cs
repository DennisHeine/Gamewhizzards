using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideEnemy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            Physics.IgnoreCollision((Collider)collision.gameObject.GetComponent<Collider>(), this.GetComponent<CapsuleCollider>());
        }
    }
}
