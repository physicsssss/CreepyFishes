using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tango;

public class PhotonManager : Photon.PunBehaviour {


    private PhotonView myPhotonView;
    public static bool playerExists=false;
    public GameObject networkStatus;
    // Use this for initialization
    void Start () {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //PhotonNetwork.ConnectUsingSettings("1.0");
    }
    public void DisconnectPlayer()
    {
        PhotonNetwork.Disconnect();
    }
    
    public void ConnectPlayer()
    {
        networkStatus.GetComponent<Text>().text = "Connecting";
        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("JoinRandom");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        
        PhotonNetwork.JoinOrCreateRoom("Den", roomOptions, TypedLobby.Default);
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        networkStatus.GetComponent<Text>().text = "Connected to Master";

        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
        //PhotonNetwork.JoinRandomRoom();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Den", roomOptions, TypedLobby.Default);
    }

    //public void OnPhotonRandomJoinFailed()
    //{
    //    PhotonNetwork.CreateRoom(null);
    //}

   
    public override void OnJoinedRoom()
    {
        networkStatus.GetComponent<Text>().text = "JOINED";
        UIManager.gs = UIManager.guistate.InGame;
        if (PhotonNetwork.playerList.Length <= 1)
        {
            //GetComponent<AreaLoadingStartup>().RegisterTangoApplication();
            //GameObject player = PhotonNetwork.Instantiate("TangoPlayer", new Vector3(5, 0, 10), Quaternion.identity, 0);
            //player.GetComponent<TangoPoseController>().isControllable = true;
            //player.GetComponent<Camera>().enabled = true;
            //myPhotonView = player.GetComponent<PhotonView>();
            SpawnObjectAR.touchEnabled = true;
        }
        else
        {
            //GameObject player = PhotonNetwork.Instantiate("TangoPlayer", new Vector3(5,0,5), Quaternion.identity, 0);
            //player.GetComponent<TangoPoseController>().isControllable = true;
            //player.GetComponent<Camera>().enabled = true;
            //myPhotonView = player.GetComponent<PhotonView>();
            SpawnObjectAR.touchEnabled = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
