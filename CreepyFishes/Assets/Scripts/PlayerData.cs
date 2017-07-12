using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData :MonoBehaviour {

    private Data data;
    public int[] getCreeps()
    {
        return data.creeps.creeps;
    }
    public void LoadPlayer(LoginData _data)
    {
        data = new Data();
        data.name = PlayerPrefs.GetString("name");

        string url = "http://localhost/App1_Server/CreepsCount.php";

        WWWForm form = new WWWForm();
       

        form.AddField("username", _data.playerName);
        form.AddField("password", _data.password);
        WWW www = new WWW(url, form);

        StartCoroutine(CreepDataRequest(www));

    }

    IEnumerator CreepDataRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            if (www.text == "0")
            {
                UIManager.gs = UIManager.guistate.Login;
            }
            else if(www.text == "-1")
            {

            }
            else
            {
                data.creeps = JsonUtility.FromJson<Creeps>(www.text);
                GameManager.SetCreepCountToPlayerPrefs(data.creeps.creeps);
                UIManager.gs = UIManager.guistate.InGame;
                GameManager.gs = GameManager.gamestate.StartUpdate;

            }

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    //TODO: Check this;
    public void SaveCreepCount()
    {

        string url = "http://localhost/App1_Server/UpdateCreep.php";
        WWWForm form = new WWWForm();
        form.AddField("username", GameManager.loginData.playerName);
        form.AddField("password", GameManager.loginData.password);
        form.AddField("creep1Value", PlayerPrefs.GetInt("Creep1Count"));
        form.AddField("creep2Value", PlayerPrefs.GetInt("Creep2Count"));
        form.AddField("creep3Value", PlayerPrefs.GetInt("Creep3Count"));
        form.AddField("creep4Value", PlayerPrefs.GetInt("Creep4Count"));
        WWW www = new WWW(url, form);

        StartCoroutine(WaitforCreepCount(www));
    }

    public void FetchCreepData()
    {

        string url = "http://localhost/App1_Server/CreepsCount.php";
        WWWForm form = new WWWForm();
        form.AddField("username", GameManager.loginData.playerName);
        form.AddField("password", GameManager.loginData.password);
        WWW www = new WWW(url, form);

        StartCoroutine(WaitforCreepCount(www));
    }
    
    IEnumerator WaitforCreepCount(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            if (www.text == "0")
            {
                UIManager.gs = UIManager.guistate.Login;
                
            }
            else if (www.text == "-1")
            {

            }
            else
            {
                data.creeps = JsonUtility.FromJson<Creeps>(www.text);
                GameManager.SetCreepCountToPlayerPrefs(data.creeps.creeps);
                UIManager.gs = UIManager.guistate.UpdateCreepCountUI;
            }

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SavePlayerData(string Name)
    {
        PlayerPrefs.SetString("name", Name);
    }
}
[Serializable]
class Data
{
    public string name;
    public Creeps creeps;
}

[Serializable]
class Creeps
{
    public int[] creeps;
}