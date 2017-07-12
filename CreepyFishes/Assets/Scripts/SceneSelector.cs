using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelector : MonoBehaviour {

    public void LoadArea()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadArea");
    }
    public void LearnArea()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreateArea");
    }


	
}
