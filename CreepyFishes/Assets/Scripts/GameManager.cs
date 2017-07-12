using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LoginData loginData;
    public delegate void GameStateChangeHandler(gamestate state);
    public static event GameStateChangeHandler gameStateChanged;
    public enum gamestate {LoadPlayer,StartUpdate ,StopUpdate, InGameCreatingMap};
    private static gamestate _gs;
    public static gamestate gs
    {
        get
        {
            return _gs;
        }
        set
        {
            _gs = value;
            if (gameStateChanged != null)
            {
                gameStateChanged(gs);
            }
        }
    }
    void gameStateChangedHandlerFunction(gamestate GS)
    {
        if(GS == gamestate.LoadPlayer)
        {
            loadPlayer();
        }else if (GS == gamestate.StartUpdate)
        {

            if(!LockCreepValuesUpdate)
            InvokeRepeating("FetchCreepCount", 0.1f, 0.1f);

        }else if(GS == gamestate.StopUpdate)
        {
            CancelInvoke("FetchCreepCount");
        }else if (GS == gamestate.InGameCreatingMap)
        {
            UIManager.gs = UIManager.guistate.InGameLearning;
            StageSpawner.spawnEnabled = true;

        }
    }

    public static bool LockCreepValuesUpdate = false;

    void FetchCreepCount()
    {
        playerData.FetchCreepData();
    }
    public static void SetCreepCountToPlayerPrefs(int []creepCounts)
    {
        PlayerPrefs.SetInt("Creep1Count", creepCounts[0]);
        PlayerPrefs.SetInt("Creep2Count", creepCounts[1]);
        PlayerPrefs.SetInt("Creep3Count", creepCounts[2]);
        PlayerPrefs.SetInt("Creep4Count", creepCounts[3]);
    }
    public static PlayerData playerData ;

    public void LockDB()
    {
        string url = "http://localhost/App1_Server/LockDB.php";

        WWWForm form = new WWWForm();

        form.AddField("username", loginData.playerName);
        form.AddField("password", loginData.password);
        WWW www = new WWW(url, form);

        StartCoroutine(Locking(www));
    }
    IEnumerator Locking(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {

            if(www.text == "1")
            {
                //Locked
            }else if(www.text == "0")
            {
                UIManager.gs = UIManager.guistate.Login;
            }
              
            
        }
        else
        {
            //TODO:UNABLE TO LOCK NO INTERNET ACCESS
            Debug.Log("WWW Error: " + www.error);
        }
    }
    public void UnlockDB()
    {
        string url = "http://localhost/App1_Server/UnlockDB.php";

        WWWForm form = new WWWForm();

        form.AddField("username", loginData.playerName);
        form.AddField("password", loginData.password);
        WWW www = new WWW(url, form);

        StartCoroutine(Unlocking(www));
    }
    IEnumerator Unlocking(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {

            if (www.text == "1")
            {
                playerData.SaveCreepCount();
            }
            else if (www.text == "0")
            {
                UIManager.gs = UIManager.guistate.Login;
            }


        }
        else
        {
            PlayerPrefs.SetFloat("UnlockLater", 1);
            Debug.Log("WWW Error: " + www.error);
        }
    }
    void loadPlayer()
    {
        if (PlayerPrefs.GetInt("UnlockLater") == 1)
        {
            UnlockDB();
        }
        playerData = GetComponent<PlayerData>();
        playerData.LoadPlayer(loginData);
    }

    void Start()
    {
        gameStateChanged += gameStateChangedHandlerFunction;

    }

    void OnApplicationQuit()
    {

        UnlockDB();
        
        
    }
    


}
