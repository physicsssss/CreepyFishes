using UnityEngine;
using System.Collections;

public class SpawnObjectAR : MonoBehaviour
{
    public GameObject spawnObject;
    private TangoPointCloud m_pointCloud;

    void Start()
    {
        m_pointCloud = FindObjectOfType<TangoPointCloud>();
    }
    public static bool touchEnabled=false;
    void Update()
    {
        if (touchEnabled && !PhotonManager.playerExists)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceKitten(Input.mousePosition);
            }
            if (Input.touchCount == 1)
            {
                // Trigger place kitten function when single touch ended.
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Ended)
                {
                    PlaceKitten(t.position);
                }
            }
        }

    }

    void PlaceKitten(Vector2 touchPosition)
    {
        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
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
            PhotonNetwork.Instantiate("Kitty", planeCenter, Quaternion.LookRotation(forward, up),0);
            PhotonManager.playerExists = true;
            //Instantiate(spawnObject, planeCenter, Quaternion.LookRotation(forward, up));
        }
        else
        {
            Debug.Log("surface is too steep for kitten to stand on.");
        }
    }
}