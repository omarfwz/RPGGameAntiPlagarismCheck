using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractScript : MonoBehaviour
{
    
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextAsset itemText;
    [SerializeField] TextAsset nonItemText;
    [SerializeField] int itemID;
    private playerControl managerScript;
    private DialogueManager dialogue;
    private ItemManager item;
    void Start()
    {
        dialogue = GameObject.FindGameObjectWithTag("Manager").GetComponent<DialogueManager>();
        managerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<playerControl>();
        item = GameObject.FindGameObjectWithTag("Manager").GetComponent<ItemManager>();
    }

    void ActivateDialogue()
    {
       if (item.hasItem(itemID))
       {
            dialogue.CallDialogue(itemText);
       }
       else
       {
            dialogue.CallDialogue(nonItemText);
       }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey += ActivateDialogue;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            managerScript.OnConfirmKey -= ActivateDialogue;
            
        }

    }
}
    
