using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactScript : MonoBehaviour
{
    
    [SerializeField] TextAsset twineText;
    [SerializeField] string basicText;
    [SerializeField] string[] textArray;
    [SerializeField] int type;
   


    void ActivateDialogue()
    {
        if(type == 1)
        {
            DialogueManager.instance.CallDialogue(basicText);
        } else if (type == 2)
        {
            DialogueManager.instance.CallDialogue(twineText);
        } else if (type == 3)
        {
            DialogueManager.instance.CallDialogue(textArray);
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.instance.OnConfirmKey += ActivateDialogue;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.instance.OnConfirmKey -= ActivateDialogue;
        }

    }

}
