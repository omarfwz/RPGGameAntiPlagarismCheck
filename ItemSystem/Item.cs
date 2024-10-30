using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Alt_Description { get; set; }
    public string MainType { get; set; }
    public Sprite ItemSprite { get; set; }
}

public class ConsumableItem : Item
{
    public int Count;
    public string Type { get; set; }
    public string SubType { get; set; }
    public string Stat { get; set; }
    public int RawValue { get; set; }
    public float Multiplier { get; set; }
    public bool Permenant { get; set; }


}

public class WeaponItem: Item
{
    public int BestDis;
    public int Range;
    public int Attack;
    public int DoT;
    public int Turns;
}

public class ItemList
{

    public static List<Item> ItemKeys = new List<Item>()
    {
        { new Item { ID = 0 , Name="Key?", Description="An object with multiple grooves. Might be a key of some sort.", MainType = "basic"}},
        { new Item { ID = 1 , Name="Large Broken Relic", Description="A broken stone with multiple grooves. It looks like something could fit into this.", MainType = "basic"}},
        { new Item { ID = 2 , Name="Small Chunk", Description="A small broken stone with a groove. ", MainType = "basic"}}

    };
    public static List<ConsumableItem> ConsItemKeys = new List<ConsumableItem>()
    {
        { new ConsumableItem { ID = 0 , Name="TestHeal", Description="Testheal. Heals 3 health.", MainType = "Consumable", Type="Healing", SubType="Direct", RawValue=3}}
        
    };
    public static List<WeaponItem> WeaponKeys = new List<WeaponItem>()
    {
        {new WeaponItem { ID = 0 , Name="Fists", Description="Used when nothing else is equipped.", MainType = "Weapon", BestDis=1, Range=10, Attack=1 }},
        {new WeaponItem { ID = 1 , Name="Worn Shrine Hammer", Description="An old hammer made from the stone near the EZQ shrine.", MainType = "Weapon", BestDis=2, Range=10, Attack=4 }}

    };
}

public class ItemCombinations
{
    public static List<ItemCombinations> AllCombonations = new List<ItemCombinations>()
    {
        {new ItemCombinations{ID = 0, Requirements = new Item[2]{ItemList.ItemKeys[1], ItemList.ItemKeys[2]}, Outputs = new Item[1]{ ItemList.ItemKeys[0] } } }
    };

    public int ID;
    public Item[] Requirements;
    public Item[] Outputs;
}