using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInteractScript : MonoBehaviour
{
    [SerializeField] TextAsset twineText;
    [SerializeField] string basicText;
    [SerializeField] string[] textArray;

    [SerializeField] bool destroyAfter;
    [SerializeField] int type;
    private DialogueManager dialogue;

    [SerializeField] int eventID;


    void Start()
    {
        dialogue = GameObject.FindGameObjectWithTag("Manager").GetComponent<DialogueManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        if (type == 1)
        {
            dialogue.CallDialogue(basicText);
        }
        else if(type == 2)
        {
            dialogue.CallDialogue(twineText);
        } else if (type == 3)
        {
            dialogue.CallDialogue(textArray);
        }
        
        if (destroyAfter)
        {
            Destroy(gameObject);
            Fundamental.instance.AddSceneEvent(eventID);
        }

    }
}
