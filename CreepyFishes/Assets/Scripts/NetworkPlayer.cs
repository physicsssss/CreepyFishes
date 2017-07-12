using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
        cameraTransform = Camera.main.GetComponent<Transform>();
	}
    private Transform cameraTransform;
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    // Update is called once per frame
    void Update()
    {
        
        if (!photonView.isMine)
        {
            
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime*20 );
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime*20 );
        }
        else
        {
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
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
