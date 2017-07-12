using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageComponents : MonoBehaviour {

    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject redArea;
    public GameObject blueArea;

	public GameObject GetLeftWall()
    {
        return leftWall;
    }
    public GameObject GetRightWall()
    {
        return rightWall;
    }
    public GameObject GetRedArea()
    {
        return redArea;
    }
    public GameObject GetBlueArea()
    {
        return blueArea;
    }

}
