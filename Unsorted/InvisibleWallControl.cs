using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvisibleWallControl : MonoBehaviour
{
    GameObject[] invWallList;
    [SerializeField] bool debugMode;
    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!debugMode)
        {
            invWallList = GameObject.FindGameObjectsWithTag("InvWall");
            foreach(GameObject obj in invWallList)
            {
                SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
                spr.sprite = null;
            }
        }
    }

    
}
