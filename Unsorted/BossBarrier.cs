using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour
{
    [SerializeField] int bossID;
    // Start is called before the first frame update
    void Start()
    {
        if (Storage.inst.BossesBeaten.Contains(bossID)){
            Destroy(gameObject);
        }
    }

    
}
