using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImageStorage : MonoBehaviour
{
    
    [SerializeField] Sprite[] ItemPics;
    [SerializeField] Sprite[] ConsItemPics;
    [SerializeField] Sprite[] SpellPics;
    [SerializeField] Sprite[] WeaponPics;



    private void Start()
    {
        for(int i = 0; i < ItemPics.Length; i++)
        {
            ItemList.ItemKeys[i].ItemSprite = ItemPics[i];
        }
        for (int i = 0; i < ConsItemPics.Length; i++)
        {
            ItemList.ConsItemKeys[i].ItemSprite = ConsItemPics[i];
        }
        for (int i = 0; i < SpellPics.Length; i++)
        {
            Spell.SpellKeys[i].icon = SpellPics[i];
        }
        for (int i = 0; i < WeaponPics.Length; i++)
        {
            ItemList.WeaponKeys[i].ItemSprite = WeaponPics[i];
        }
    }
}
