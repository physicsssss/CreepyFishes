using System.Collections;
using UnityEngine;
using Tango;

public class AreaLearningStartup : MonoBehaviour, ITangoLifecycle
{
    private TangoApplication m_tangoApplication;

    public void Start()
    {
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            m_tangoApplication.RequestPermissions();
        }
    }

    public void OnTangoPermissions(bool permissionsGranted)
    {
        // No Area Descriptions available.
        m_tangoApplication.Startup(null);

    }
    void Update()
    {
    }
   

    public void OnTangoServiceConnected()
    {
    }

    public void OnTangoServiceDisconnected()
    {
    }
}