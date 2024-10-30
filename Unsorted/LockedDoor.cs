using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{

    [SerializeField] string itemText;
    [SerializeField] int itemID;
    [SerializeField] int eventID;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openedSprite;
    void Start()
    {
    }

    void ActivateDialogue()
    {
        if (ItemManager.instance.hasItem(itemID))
        {
            OpenDoor();
            Fundamental.instance.AddSceneEvent(eventID);
            ItemManager.instance.removeItem(itemID);
        }
        else
        {
            DialogueManager.instance.CallDialogue(itemText);
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
    public void OpenDoor()
    {
        spriteRenderer.sprite = openedSprite;
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            Destroy(col);
        }
    }
}
