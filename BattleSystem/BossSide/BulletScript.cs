using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifespan;
    public float damageMultiplier;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifespan && lifespan != 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleController.instance.TakeDamage((int)Mathf.Round(damageMultiplier * BossManager.instance.attack));
            Destroy(gameObject);
        }
    }
}
