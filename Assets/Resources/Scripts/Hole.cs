using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public static int score; // record user score

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Balls")
        {
            collision.transform.position = this.transform.position;
            collision.collider.attachedRigidbody.AddForce(this.transform.forward * 10.0f);
            collision.collider.attachedRigidbody.useGravity = true;
            score += 1;
        }
    }
    
}
