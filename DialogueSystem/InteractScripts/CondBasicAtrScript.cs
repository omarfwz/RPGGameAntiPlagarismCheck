using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondBasicAtrScript : MonoBehaviour
{

    [SerializeField] string atrText;
    [SerializeField] TextAsset nonAtrText;
    [SerializeField] string atrID;

    private playerControl inputScript;
    void Start()
    {

        inputScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<playerControl>();
    }

    void ActivateDialogue()
    {
        if (Logic.instance.HasAttribute(atrID))
        {
            DialogueManager.instance.CallDialogue(atrText);
        }
        else
        {
            DialogueManager.instance.CallDialogue(nonAtrText);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inputScript.OnConfirmKey += ActivateDialogue;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inputScript.OnConfirmKey -= ActivateDialogue;

        }

    }
}

