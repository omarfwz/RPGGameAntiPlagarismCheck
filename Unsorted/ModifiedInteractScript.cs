using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedInteractScript : MonoBehaviour
{
    [SerializeField] TextAsset twineText;
    [SerializeField] string afterCalled;
    [SerializeField] GameObject barrier;
    [SerializeField] bool destroyBarrier;
    [SerializeField] int eventID;

    private bool called;

    private void Start()
    {
        if (Storage.inst.sceneEvents.Contains(eventID))
        {
            called = true;
            if (destroyBarrier)
            {
                Destroy(barrier);
            }
        }
    }
    void ActivateDialogue()
    {
        if (!called)
        {
            DialogueManager.instance.CallDialogue(twineText);
            called = true;
            if (destroyBarrier)
            {
                Destroy(barrier);
            }
            Storage.inst.sceneEvents.Add(eventID);
            Debug.Log(Storage.inst.sceneEvents[0]);
        }
        else
        {
            DialogueManager.instance.CallDialogue(afterCalled);
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
