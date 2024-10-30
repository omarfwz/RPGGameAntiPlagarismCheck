using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BoundingBoxSwitcher : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;

    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("BoundingBox").GetComponent<Collider2D>();
    }
}
