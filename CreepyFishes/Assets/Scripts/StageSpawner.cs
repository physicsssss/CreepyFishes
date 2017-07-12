using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour {

    private GameObject spawnObject;
    private TangoPointCloud m_pointCloud;

    void Start()
    {
        spawnObject = (GameObject) Resources.Load("Prefabs/SampleStage");
        m_pointCloud = FindObjectOfType<TangoPointCloud>();
    }
    public static bool spawnEnabled = false;
    private bool isExists=false;
    void Update()
    {
        if (spawnEnabled && !isExists)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                PlaceStage(Input.mousePosition);
            }
            if (Input.touchCount == 1)
            {
                // Trigger place stage function when single touch ended.
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Ended)
                {
                    PlaceStage(t.position);
                }
            }
        }

    }

    void PlaceStage(Vector2 touchPosition)
    {
        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
#if UNITY_EDITOR
        isExists = true;
        FindObjectOfType<EditStage>().instantiateStage(Instantiate(spawnObject, Vector3.zero, Quaternion.identity));
#else
        if (!m_pointCloud.FindPlane(cam, touchPosition, out planeCenter, out plane))
        {
            Debug.Log("cannot find plane.");
            return;
        }

        // Place kitten on the surface, and make it always face the camera.
        if (Vector3.Angle(plane.normal, Vector3.up) < 30.0f)
        {
            Vector3 up = plane.normal;
            Vector3 right = Vector3.Cross(plane.normal, cam.transform.forward).normalized;
            Vector3 forward = Vector3.Cross(right, plane.normal).normalized;
            //PhotonNetwork.Instantiate("Kitty", planeCenter, Quaternion.LookRotation(forward, up), 0);
            //PhotonManager.playerExists = true;
            isExists = true;
            FindObjectOfType<EditStage>().instantiateStage(Instantiate(spawnObject, planeCenter, Quaternion.identity));
        }
        else
        {
            Debug.Log("surface is too steep for Stage to be placed.");
        }
#endif
    }
}
