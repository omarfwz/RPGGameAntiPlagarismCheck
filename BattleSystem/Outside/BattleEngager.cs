using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEngager : MonoBehaviour
{
    [SerializeField] string battleName;
    [SerializeField] TextAsset overrideDialogue;
    Vector3 pos;

    void BattleScene()
    {
        Fundamental.instance.LoadBattleScene(battleName, overrideDialogue, pos);
        playerControl.instance.OnConfirmKey -= BattleScene;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pos = collision.GetComponent<Transform>().position;
            playerControl.instance.OnConfirmKey += BattleScene;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.instance.OnConfirmKey -= BattleScene;
        }

    }
}
