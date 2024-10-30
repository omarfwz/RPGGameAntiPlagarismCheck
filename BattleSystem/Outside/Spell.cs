using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    public int ID;
    public string Name;
    public string Description;
    public string Type;
    public string Subtype;
    public float Multiplier;
    public int RawValue;
    public int Cost;
    public Sprite icon;

    public static List<Spell> SpellKeys = new List<Spell>()
    {
        {new Spell { ID = 0 , Name="Test Healing Spell", Description="A testing healing spell. Used for development.", Type = "Healing",Subtype="Scaler", Multiplier=0.2F, Cost=10 }},
        {new Spell { ID = 1 , Name="Test Attack Spell", Description="A testing attacking spell. Used for development.", Type = "Damaging", Subtype="Scaler", Multiplier=2F, Cost=10 }},
        {new Spell { ID = 2 , Name="Test Weakening Spell", Description="A testing attacking spell. Used for development.", Type = "Weaken", Subtype="Scaler", Multiplier=.8F, Cost=10 }}
    };
}



public class Charm
{
    
    public static List<int> charmSlots = new List<int>();
    public int ID;
    public string Name;
    public string Description;
    public int[] DirectAdditions;
    public float[] Multipliers;

    /*
        0 = hp
        1 = attack
        2 = defense
        3 = mp
     */

    public int SlotCost;
    public Sprite Icon;

    public static List<Charm> charmList = new List<Charm>()
    {


    };
}

public class StatChanger
{
    
    public int ID;
    public string Name;
    public int[] DirectAdditions;
    public float[] Multipliers;

    public static List<StatChanger> changerList = new List<StatChanger>()
    {


    };
} 