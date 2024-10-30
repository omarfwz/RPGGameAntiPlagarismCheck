using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] NPCController noora;
    bool active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !active)
        {
            StartCoroutine(NooraCutscene());
            active = true;
        }
    }

    IEnumerator NooraCutscene()
    {
        playerControl.instance.DisableControls();
        yield return StartCoroutine(playerControl.instance.MoveCamera(new Vector2(playerControl.instance.cam.position.x, playerControl.instance.cam.position.y - 0.5F), true));
        yield return StartCoroutine(noora.MoveToPoint(new Vector2(-1.656F, -6.506F)));
        DialogueManager.instance.CallDialogue("hi");
        noora.SetDirection(1);
        yield return StartCoroutine(playerControl.instance.MoveCamera(new Vector2(playerControl.instance.cam.position.x, playerControl.instance.cam.position.y + 0.5F), false));

    }
}
