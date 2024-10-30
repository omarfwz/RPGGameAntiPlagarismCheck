using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public long lastUpdated;

    public string playerName;
    public string[] attributes;
    public int[] sceneEvents;
    public int relation;
    public string savePoint;
    public string sceneName;

    public List<int> inv;
    public List<int> consInv;
    public List<int> consCounts;
    public List<int> spellInv;
    public List<int> weaponInv;
    public List<int> charmInv;
    

    public List<int> spellInv_eq;
    public List<int> charmInv_eq;
    public int equippedWeapon;

    public int lvl;
    public int xp;
    public List<int> statChangers;

    public PlayerData()
    {
        playerName = "";
        attributes = new string[0];
        sceneEvents = new int[0];
        relation = 0;
        savePoint = "Beginning";
        sceneName = "TempleLandingSpawn";
        
        inv = new List<int>();
        consInv = new List<int>();
        consCounts = new List<int>();
        spellInv = new List<int>();
        weaponInv = new List<int>();
        charmInv = new List<int>();

        spellInv_eq = new List<int>();
        charmInv_eq = new List<int>();
        equippedWeapon = 1;

        lvl = 1;
        xp = 0;
        statChangers = new List<int>();
    }

}
