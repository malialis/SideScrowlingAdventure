using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public string sceneToLoad;
    public string exitName;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player has entered the pipe");
            PlayerPrefs.SetString("LastExitName", exitName);
            SceneManager.LoadScene(sceneToLoad);
        }
    }


}
