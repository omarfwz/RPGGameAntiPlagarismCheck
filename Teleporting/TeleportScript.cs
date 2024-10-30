using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D otherteleporter;
    [SerializeField] Vector2 offset;

    private playerControl managerScript;
    private TeleportManager tpScript;
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<playerControl>();
        tpScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<TeleportManager>();

    }
    void goToObject()
    {
        tpScript.TeleportPlayer(otherteleporter, offset);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey += goToObject;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey -= goToObject;

        }

    }
} 

