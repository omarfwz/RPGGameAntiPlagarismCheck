using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePointScript : MonoBehaviour
{
    [SerializeField] string savePointID;
    playerControl managerScript;
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<playerControl>();
    }

    void SaveData()
    {
        DataPersistenceManager.instance.SetSavePoint(savePointID);
        DataPersistenceManager.instance.SaveGame();
        DialogueManager.instance.CallDialogue("[Game Saved]");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey += SaveData;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey -= SaveData;
        }

    }

   
    }


