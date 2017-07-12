using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tango;

public class AreaLoadingStartup : MonoBehaviour, ITangoLifecycle
{
    private TangoApplication m_tangoApplication;
    private GameObject MapSelectorButton;
    public GameObject MapSelectorPanel;
    private GameObject loadOrCreateMapMenu;
    private string mapName;
    private string uuid;
    public bool isNewMap;
    public void Start()
    {
        //loadOrCreateMapMenu = GameObject.Find("LoadOrCreateMapMenu");
        MapSelectorButton = (GameObject) Resources.Load("Prefabs/MapSelectButton");
        //RegisterApplication();
        
    }
    
    public void RegisterApplication()
    {
        isNewMap = false;
        UIManager.gs = UIManager.guistate.SelecMapMenu;        
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            m_tangoApplication.RequestPermissions();
        }
    }
    //public void RegisterTangoForExistingRoom()
    //{
    //    m_tangoApplication = FindObjectOfType<TangoApplication>();
    //    if (m_tangoApplication != null)
    //    {
    //        m_tangoApplication.Register(this);
    //        m_tangoApplication.RequestPermissions();
    //    }
    //}
    private AreaDescription[] list;
    public void OnTangoPermissions(bool permissionsGranted)
    {
        if (permissionsGranted)
        {
            if (!isNewMap)
            {
                list = AreaDescription.GetList();
                if (list.Length > 0)
                {

                    foreach (AreaDescription areaDescription in list)
                    {
                        
                        AreaDescription.Metadata metadata = areaDescription.GetMetadata();

                        GameObject b = Instantiate(MapSelectorButton);
                        b.transform.SetParent(MapSelectorPanel.transform);
                        Button tempButton = b.GetComponent<Button>();
                        tempButton.GetComponentInChildren<Text>().text = metadata.m_name;
                        tempButton.onClick.AddListener(() => SelectMapButton(tempButton.GetComponentInChildren<Text>().text));


                    }

                }
                else
                {
                    // No Area Descriptions available.
                    m_tangoApplication.AreaDescriptionLearningMode = true;
                    isNewMap = true;
                    mapName = "NewMap";
                    m_tangoApplication.Startup(null);
                    //GetComponent<PhotonManager>().ConnectPlayer();
                    //UIManager.gs = UIManager.guistate.InGameLearning;
                    GameManager.gs = GameManager.gamestate.InGameCreatingMap;
                }
            }
            else
            {
                m_tangoApplication.AreaDescriptionLearningMode = true;
                m_tangoApplication.Startup(null);
                //GetComponent<PhotonManager>().ConnectPlayer();
                //UIManager.gs = UIManager.guistate.InGameLearning;
                GameManager.gs = GameManager.gamestate.InGameCreatingMap;
            }
            
        }
    }
    public string GetName()
    {
        return mapName;
    }
    public string GetUUID()
    {
        return uuid;
    }
    public void CreateNewMap(string Name)
    {
        isNewMap = true;
        mapName = Name;
        
        m_tangoApplication = FindObjectOfType<TangoApplication>();

        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            m_tangoApplication.RequestPermissions();
        }
    }
    void SelectMapButton(string metadataName)
    {
        foreach (AreaDescription areaDescription in list)
        {
            AreaDescription.Metadata metadata = areaDescription.GetMetadata();
            if(metadata.m_name == metadataName)
            {
                FindObjectOfType<TangoApplication>().AreaDescriptionLearningMode = false;
                mapName = metadataName;
                
                UIManager.gs = UIManager.guistate.InGame;
                m_tangoApplication.Startup(areaDescription);
                GetComponent<PhotonManager>().ConnectPlayer();
                return;
            }
        }

    }
        public void OnTangoServiceConnected()
    {
    }

    public void OnTangoServiceDisconnected()
    {
    }
}