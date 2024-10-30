using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeTriggerScript : MonoBehaviour
{
    [SerializeField] string requiredAttribute;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Logic.instance.HasAttribute(requiredAttribute))
            {
                Destroy(gameObject);
            }
        }
    }
}
