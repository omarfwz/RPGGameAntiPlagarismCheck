using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStorage : MonoBehaviour
{
    CharacterBox[] characters;
    private void Awake()
    {

        characters = gameObject.GetComponentsInChildren<CharacterBox>();
        foreach(CharacterBox box in characters)
        {
            CharacterObject.characters.Add(new CharacterObject { Name = box.Name, Images = box.Images });
            Destroy(box.gameObject);
        }
    }
}
