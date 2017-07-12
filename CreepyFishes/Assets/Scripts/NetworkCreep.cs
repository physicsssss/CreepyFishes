using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCreep : Photon.MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }
    
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {

            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 20);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 20);
        }
        else
        {
            transform.Translate(Vector3.forward);
            
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine)
        {
            if (collision.gameObject.name == "WALL")
            {
                Vector3 collisionNormal = collision.contacts[0].normal;
                float dot = Vector3.Dot(collisionNormal, (-transform.forward));
                float cosTheta = dot / ((-transform.forward.magnitude) * collisionNormal.magnitude);
                float theta = Mathf.Acos(cosTheta);

                transform.Rotate(0, 0, 180 - (2 * theta));
            }
        }
    }

  
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);


        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

        }
    }

}
