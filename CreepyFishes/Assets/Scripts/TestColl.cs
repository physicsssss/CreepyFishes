using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "WALL")
        {
            print("COLLISIONBOY");
            Vector3 collisionNormal = collision.contacts[0].normal;
            float dot = Vector3.Dot(collisionNormal, (-transform.forward));
            //float cosTheta = dot / ((-transform.forward.magnitude) * collisionNormal.magnitude);
            float theta = Mathf.Acos(dot);
            theta = Mathf.Rad2Deg * theta;
            //transform.position = Vector3.Reflect(transform.forward, collisionNormal);
            print(theta.ToString());
            if (transform.forward.x >= 0 && transform.forward.z >= 0 && collisionNormal.z < 0)
            {
                transform.Rotate(0, 180 - (2 * theta), 0);
            }
            else if (transform.forward.x >= 0 && transform.forward.z >= 0 && collisionNormal.z >= 0)
            {
                transform.Rotate(0, 180 + (2 * theta), 0);
            }
            else if (transform.forward.x < 0 && transform.forward.z >= 0 && collisionNormal.z < 0)
            {
                transform.Rotate(0, 180 + (2 * theta), 0);
            }else if(transform.forward.x < 0 && transform.forward.z >= 0 && collisionNormal.z >= 0)
            {
                transform.Rotate(0, 180 - (2 * theta), 0);
            }
            else if (transform.forward.x >= 0 && transform.forward.z < 0 && collisionNormal.z > 0)
            {
                transform.Rotate(0, 180 + (2 * theta), 0);
            }else if(transform.forward.x >= 0 && transform.forward.z < 0 && collisionNormal.z <= 0)
            {
                transform.Rotate(0, 180 - (2 * theta), 0);
            }else if(transform.forward.x < 0 && transform.forward.z < 0 && collisionNormal.z > 0)
            {
                transform.Rotate(0, 180 - (2 * theta), 0);
            }else if(transform.forward.x < 0 && transform.forward.z < 0 && collisionNormal.z <= 0)
            {
                transform.Rotate(0, 180 + (2 * theta), 0);
            }
        }
        
    }
    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward*Time.deltaTime*4);
    }
}
