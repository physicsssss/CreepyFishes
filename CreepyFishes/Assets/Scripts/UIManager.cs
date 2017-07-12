using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public delegate void GuiStateChangeHandler(guistate state);
    public static event GuiStateChangeHandler guiStateChanged;
    public enum guistate { Login, InGame, UpdateCreepCountUI,LoadOrCreateMapMenu,SelecMapMenu, MapNameMenu , InGameLearning };
    private static guistate _gs;
    public static guistate gs
    {
        get
        {
            return _gs;
        }
        set
        {
            _gs = value;
            if (guiStateChanged != null)
            {
                guiStateChanged(gs);
            }
        }
    }
    void guiStateChangedHandlerFunction(guistate GS)
    {
        if (GS == guistate.Login)
        {
            DisableAll();
            loginSignupMenu.SetActive(true);
        }
        else if (GS == guistate.InGame)
        {
            DisableAll();
            Ingame();
        }
        else if (GS == guistate.UpdateCreepCountUI)
        {
            UpdateCreepCountUI();
        }
        else if (GS == guistate.LoadOrCreateMapMenu)
        {
            DisableAll();
            loadOrCreateMapMenu.SetActive(true);
        }
        else if (GS == guistate.SelecMapMenu)
        {
            DisableAll();
            mapSelectorMenu.SetActive(true);
        }
        else if(GS == guistate.MapNameMenu)
        {
            DisableAll();
            mapNameMenu.SetActive(true);
        }else if(GS == guistate.InGameLearning)
        {
            DisableAll();
            learningMainMenu.SetActive(true);
        }
    }
    private void DisableAll()
    {
    
        ingameMenu.SetActive(false);
        loadOrCreateMapMenu.SetActive(false);
        loginSignupMenu.SetActive(false);
        mapSelectorMenu.SetActive(false);
        mapNameMenu.SetActive(false);
        learningMainMenu.SetActive(false);

    }
    List<Text>CreepCountText;
    List<int> creepCounts;
    InputField userName;
    InputField password;
    public Text loginLabel;
    public GameObject ingameMenu;
    public GameObject loginSignupMenu;
    public GameObject loadOrCreateMapMenu;
    public GameObject mapNameMenu;
    public GameObject mapSelectorMenu;
    public GameObject learningMainMenu;




    string _userName;
    string _password;


    void UpdateCreepCountUI()
    {
        creepCounts = new List<int>();
        if (CreepCountText == null)
        {
            CreepCountText = new List<Text>();
            for (int i = 0; i < 4; i++)
            {
                creepCounts.Add(GameManager.playerData.getCreeps()[i]);
                CreepCountText.Add(GameObject.Find("CreepCount" + (i + 1)).GetComponent<Text>());
                CreepCountText[i].text = creepCounts[i].ToString();
            }
        }
        else
        {
            for(int i = 0; i < 4; i++)
            {
                creepCounts.Add( GameManager.playerData.getCreeps()[i]);
                CreepCountText[i].text = creepCounts[i].ToString();
            }
        }
    }

    public void CreateMapButton()
    {
        gs = guistate.MapNameMenu;
        //GetComponent<PhotonManager>().ConnectPlayer();
    }

    public void SubmitMapNameButton()
    {
        GameObject g = GameObject.Find("MapNameText");
        string mapName = g.GetComponent<Text>().text;
        GetComponent<AreaLoadingStartup>().CreateNewMap(mapName);
        //GetComponent<PhotonManager>().ConnectPlayer();

    }

    public void EndGameButton()
    {
        GetComponent<AreaLearningInGameController>().Save();
    }
    public void LoadareaButton()
    {
        //GetComponent<PhotonManager>().ConnectPlayer();

        GetComponent<AreaLoadingStartup>().RegisterApplication();

    }
    
    public void SignUp()
    {
        string url = "http://localhost/App1_Server/SignUp.php";

        WWWForm form = new WWWForm();
        _userName = userName.text;
        _password = password.text;

        form.AddField("username", userName.text);
        form.AddField("password", password.text);
        WWW www = new WWW(url, form);

        StartCoroutine(SignupRequest(www));
    }
    public void Login()
    {
        string url = "http://localhost/App1_Server/Login.php";

        WWWForm form = new WWWForm();
        _userName = userName.text;
        _password = password.text;

        form.AddField("username", userName.text);
        form.AddField("password", password.text);
        WWW www = new WWW(url, form);

        StartCoroutine(LoginRequest(www));
    }
    IEnumerator LoginRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {

            GameManager.loginData = JsonUtility.FromJson<LoginData>(www.text);
            if (GameManager.loginData.loginStatus == 1)
            {
                GameManager.gs = GameManager.gamestate.LoadPlayer;
               
            }
            else
            {
                loginLabel.text = "Invalid username or password. Try Again.";
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    public void LockToggle(Text bText)
    {
        GameManager.LockCreepValuesUpdate = !GameManager.LockCreepValuesUpdate;

        if (GameManager.LockCreepValuesUpdate)
        {
            bText.text = "Unlock";
            GameManager.gs = GameManager.gamestate.StopUpdate;
        }
        else
        {
            bText.text = "Lock";
            GameManager.gs = GameManager.gamestate.StartUpdate;
        }
    }
    IEnumerator SignupRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            if (www.text == "1")
            {
                userName.text = "";
                password.text = "";
                loginLabel.text = "Signed up successfuly. Log in.";
            }
            else if (www.text == "0")
            {
                loginLabel.text = "Username exists. Try again.";
            }else if(www.text == "-1")
            {
                loginLabel.text = "Something went wrong, Please try again.";
            }

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SpawnCreep(int CreepCount)
    {
        int RandomNumber = CreepCount;
        if (creepCounts[RandomNumber] > 0)
        {

            Instantiate(Resources.Load("Prefabs/Creep" + (RandomNumber + 1)));
            CreepCountText[RandomNumber].text = (--creepCounts[RandomNumber]).ToString();
            if (!GameManager.LockCreepValuesUpdate)
            {
                string url = "http://localhost/App1_Server/RemoveCreep.php";
                WWWForm form = new WWWForm();
                form.AddField("username", GameManager.loginData.playerName);
                form.AddField("password", GameManager.loginData.password);
                form.AddField("creepID", RandomNumber + 1);
                form.AddField("valueToRemove", 1);

                WWW www = new WWW(url, form);

                StartCoroutine(AddCreepstoDB(www));
            }

            GameManager.SetCreepCountToPlayerPrefs(creepCounts.ToArray());

        }

    }
   
    IEnumerator AddCreepstoDB(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            if (www.text == "1")
            {

            }
            else if (www.text == "-1")
            {
                Debug.Log("NO NETWORK ACCESS");
            }

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    public void SaveMapButton()
    {
        GetComponent<AreaLearningInGameController>().Save();
    }

    void Ingame()
    {
        
        ingameMenu.SetActive(true);

        //UpdateCreepCountUI();
    }
    // Use this for initialization
    void Start()
    {

        
        
        guiStateChanged += guiStateChangedHandlerFunction;
        //ingameMenu.SetActive(false);
        //DisableAll();
        gs = guistate.LoadOrCreateMapMenu;


    }

}
[Serializable]
public class LoginData
{
    public string playerName;
    public string password;
    public int loginStatus;
}
