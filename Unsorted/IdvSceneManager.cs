using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdvSceneManager : MonoBehaviour
{
    public static IdvSceneManager instance;
    
    [SerializeField] string sceneName;
    [SerializeField] int[] sceneEvents;
    [SerializeField] string[] runningEvent;
    [SerializeField] GameObject[] relevantObjects;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogAssertion("Replacing scene manager.");
        }
        instance = this;
    }
    void Start()
    {
        //this is just for testing!
        if(Storage.inst.spellInventory.Count < 1)
        {
            Storage.inst.spellInventory.Add(Spell.SpellKeys[0]);
            Storage.inst.spellInventory.Add(Spell.SpellKeys[1]);
            Storage.inst.spellInventory.Add(Spell.SpellKeys[2]);
        }



        foreach(int i in Storage.inst.sceneEvents)
        {
            Debug.Log(i);
        }
        Storage.inst.sceneName = sceneName;
        for(int i = 0; i<sceneEvents.Length; i++)
        {
            if (!Storage.inst.sceneEvents.Contains(sceneEvents[i]))
            {
                continue;
            }
            if (runningEvent[i] == "Destroy")
            {
                Destroy(relevantObjects[i]);
            } else if(runningEvent[i] == "Open")
            {
                relevantObjects[i].GetComponent<LockedDoor>().OpenDoor();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
}
