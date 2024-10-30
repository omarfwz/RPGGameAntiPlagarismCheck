using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] Rigidbody2D cam;
    [SerializeField] Rigidbody2D player;

    public void TeleportPlayer(Rigidbody2D goal, Vector2 offset)
    {
        player.position = goal.position + offset;
        cam.position = player.position;
    }
    
}
