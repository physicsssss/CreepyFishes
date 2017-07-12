using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStage : MonoBehaviour {
    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject redArea;
    private GameObject blueArea;

    [HideInInspector]
    public GameObject stage;
    // Use this for initialization

    public void instantiateStage(GameObject s)
    {
        stage = s;
        StageComponents stageComponents = s.GetComponent<StageComponents>();
        leftWall = stageComponents.GetLeftWall();
        rightWall = stageComponents.GetRightWall();
        redArea = stageComponents.GetRedArea();
        blueArea = stageComponents.GetBlueArea();
        initiated = true;

    }

    public static bool initiated = false;
    private int changeHeightValue=1;
    private int changeWidthValue=1;
    private int rotateValue=1;
    private bool isChangeAbleHeight;
    private bool isChangeAbleWidth;
    private bool isRotateable;
    
    public void StartAdjustHeight(int val)
    {
        changeHeightValue = val;   
        isChangeAbleHeight = true;
    }
    public void StartAdjustWidth(int val)
    {
        changeWidthValue = val;
        isChangeAbleWidth = true;
    }

    public void StopAdjustHeight()
    {
        isChangeAbleHeight = false;
    }
    public void StopAdjustWidth()
    {
        isChangeAbleWidth = false;
    }

    public void StartRotate(int val)
    {
        rotateValue = val;
        isRotateable = true;
    }
    public void StopRotate()
    {
        isRotateable = false;
    }

    // Update is called once per frame
    void Update () {
        
        if (initiated)
        {
            if (isChangeAbleHeight)
            {
                leftWall.transform.localScale = new Vector3(leftWall.transform.localScale.x, leftWall.transform.localScale.y, leftWall.transform.localScale.z + changeHeightValue);
                rightWall.transform.localScale = new Vector3(rightWall.transform.localScale.x, rightWall.transform.localScale.y, rightWall.transform.localScale.z + changeHeightValue);

                redArea.transform.Translate(changeHeightValue * Vector3.forward/2.0f);
                blueArea.transform.Translate(changeHeightValue * Vector3.back /2.0f);
            }
            if (isChangeAbleWidth)
            {
                redArea.transform.localScale = new Vector3(redArea.transform.localScale.x + changeWidthValue, redArea.transform.localScale.y, redArea.transform.localScale.z);
                blueArea.transform.localScale = new Vector3(blueArea.transform.localScale.x + changeWidthValue, blueArea.transform.localScale.y, blueArea.transform.localScale.z );

                rightWall.transform.Translate(changeWidthValue * Vector3.right / 2.0f);
                leftWall.transform.Translate(changeWidthValue * Vector3.left / 2.0f);
            }
            if (isRotateable)
            {
                stage.transform.Rotate(Vector3.up,rotateValue* 0.1f);
            }
        }
	}
}
