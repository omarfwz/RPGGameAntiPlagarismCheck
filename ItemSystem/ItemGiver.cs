using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] int itemID;
    [SerializeField] bool canGetMultipleTimes;
    [SerializeField] bool basic = true;
    [SerializeField] TextAsset complextext;
    [SerializeField] string actionText;
    [SerializeField] int eventID;

    private void Start()
    {
        if (!canGetMultipleTimes && Storage.inst.sceneEvents.Contains(eventID))
        {
            Destroy(gameObject);
        }
    }
    void ActivateDialogue()
    {
        if (basic)
        {
            DialogueManager.instance.CallDialogue(actionText);
        }
        else
        {
            DialogueManager.instance.CallDialogue(complextext);
        }
        ItemManager.instance.addToInventory(itemID);
        if (!canGetMultipleTimes)
        {
            Destroy(gameObject);
            Fundamental.instance.AddSceneEvent(eventID);
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
