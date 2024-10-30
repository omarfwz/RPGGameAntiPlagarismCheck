using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;


    List<Item> combinationSlots = new List<Item>();


    [Header("GUI")]
    [SerializeField] GameObject invGUI;
    [SerializeField] GameObject charGUI;
    [SerializeField] GameObject descGUI;
    [SerializeField] TextMeshProUGUI descriptionTextArea;

    [Header("Slots")]
    [SerializeField] GameObject[] itemSlots;
    [SerializeField] GameObject[] consItemSlots;
    [SerializeField] GameObject[] craftingSlots;
    [SerializeField] GameObject[] resultsSlots;
    [SerializeField] GameObject[] spellSlots;
    [SerializeField] GameObject[] weaponSlots;
    [SerializeField] GameObject SpellParent;
    [SerializeField] GameObject WeaponParent;
    [SerializeField] GameObject ConsumableParent;
    [SerializeField] GameObject[] equippedSpellSlots;
    [SerializeField] GameObject equippedWeapon;
    [SerializeField] GameObject[] charmSlots;
    [SerializeField] GameObject[] equippedCharmSlots;

    [Header("CharacterStats")]



    playerControl inputScript;

    bool open = false;
    float timer;

    
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("multiple item managers");
        }
        instance = this;
    }
    private void Start()
    {
        inputScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<playerControl>();
        inputScript.OnInvKey += OpenInventory;

    }
    public void Update()
    {
        if (open)
        {
            timer += Time.deltaTime;
            if(timer > .2 && EventSystem.current.currentSelectedGameObject != null)
            {
                
                UI_Slot slot = EventSystem.current.currentSelectedGameObject.GetComponent<UI_Slot>();
                if (slot != null)
                {
                    if (slot.slotItem != null)
                    {
                        descriptionTextArea.text = slot.slotItem.Description;
                    }
                    else if (slot.slotSpell != null)
                    {
                        descriptionTextArea.text = slot.slotSpell.Description;
                    }
                    else if (slot.slotCharm != null)
                    {
                        descriptionTextArea.text = slot.slotCharm.Description;
                    }
                    else if (slot.slotWeapon != null)
                    {
                        descriptionTextArea.text = slot.slotWeapon.Description;
                    }
                    else
                    {
                        descriptionTextArea.text = "";
                    }
                }
                else
                {
                    descriptionTextArea.text = "";
                }
                
            }
        }
    }

 
    
    private void CloseInventory(InputAction.CallbackContext context)
    {
        open = false;
        descGUI.SetActive(false);
        invGUI.SetActive(false);
        charGUI.SetActive(false);
        inputScript.playerController.UI.Disable();
        inputScript.setUpControls();
    }
    public void OpenInventory()
    {
        open = true;
        inputScript.DisableControls();
        inputScript.playerController.UI.Enable();
        invGUI.SetActive(true);
        descGUI.SetActive(true);
        charGUI.SetActive(false);
        inputScript.playerController.UI.Inventory.performed += CloseInventory;
        OpenItemsInventory();
        OpenCraftingArea();

    }

    public void OpenCharacter()
    {
        open = true;
        charGUI.SetActive(true);
        invGUI.SetActive(false);
        OpenConsumablesInventory();
        OpenEquippedSpells();
        OpenEquippedWeapon();
    }

    void OpenItemsInventory()
    {
        for (int i = 0; i < 15; i++)
        {
            itemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (Storage.inst.inventory.Count - 1 < i)
            {
                SetButtonEmpty(itemSlots[i]);
            }
            else
            {
                Item item = Storage.inst.inventory[i];
                GameObject button = itemSlots[i];
                SetButtonItem(itemSlots[i], item);
                itemSlots[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { AddItemToCrafting(item, button); });
            }
        }
    }

    void OpenCraftingArea()
    {
        combinationSlots.Clear();
        for(int i = 0; i < 3; i++)
        {
            SetButtonEmpty(craftingSlots[i]);
        }
        for(int i = 0; i < 2; i++)
        {
            SetButtonEmpty(resultsSlots[i]);
            resultsSlots[i].SetActive(false);
        }
    }

    public void OpenConsumablesInventory()
    {
        CloseAllStorageMenus();
        ConsumableParent.SetActive(true);
        for (int i = 0; i < 12; i++)
        {
            consItemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (Storage.inst.consInventory.Count - 1 < i)
            {
                SetButtonEmpty(consItemSlots[i]);
            }
            else
            {
                ConsumableItem item = Storage.inst.consInventory[i];
                SetButtonItem(consItemSlots[i], item);

                /*consItemSlots[i].GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
                consItemSlots[i].GetComponentsInChildren<Image>()[1].sprite = Storage.inst.consInventory[i].ItemSprite;
                consItemSlots[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = Storage.inst.consInventory[i].Name;
                consItemSlots[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "x" + Storage.inst.consInventory[i].Count;
                *///consItemSlots[i].GetComponent<Button>().onClick.AddListener(delegate { ReadDescription(consInventory[i].Description); });
            }
        }
    }

    public void OpenSpellsInventory()
    {
        CloseAllStorageMenus();
        SpellParent.SetActive(true);
        for (int i = 0; i < 12; i++)
        {
            spellSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (Storage.inst.spellInventory.Count - 1 < i)
            {
                spellSlots[i].GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
                spellSlots[i].GetComponentsInChildren<Image>()[1].sprite = null;
                spellSlots[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = null;
            }
            else
            {
                Spell spell = Storage.inst.spellInventory[i];
                GameObject button = spellSlots[i];
                SetButtonSpell(button, spell);   
                spellSlots[i].GetComponent<Button>().onClick.AddListener(delegate { EquipSpell(spell, button); });
            }
        }
    }

    public void OpenEquippedSpells()
    {  
        for (int i = 0; i < 4; i++)
        {
            equippedSpellSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (Storage.inst.equippedSpells.Count - 1 < i)
            {
                equippedSpellSlots[i].GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
                equippedSpellSlots[i].GetComponentsInChildren<Image>()[1].sprite = null;
                equippedSpellSlots[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = null;
            }
            else
            {
                Spell spell = Storage.inst.equippedSpells[i];
                GameObject button = equippedSpellSlots[i];
                SetButtonSpell(button, spell);
                button.GetComponent<Button>().onClick.AddListener(delegate { UnequipSpell(spell, button); });
            }
        }
    }

    public void OpenEquippedWeapon()
    {  
        if(equippedWeapon != null)
        {
            SetButtonWeapon(equippedWeapon, Storage.inst.equippedWeapon);
        }
        else
        {
            equippedWeapon.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
            equippedWeapon.GetComponentsInChildren<Image>()[1].sprite = null;
            equippedWeapon.GetComponentsInChildren<TextMeshProUGUI>()[0].text = null;
        }   
    }

    public void OpenWeaponsInventory()
    {
        CloseAllStorageMenus();
        WeaponParent.SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            consItemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (Storage.inst.weaponInventory.Count - 1 < i)
            {
                SetButtonEmpty(weaponSlots[i]);
            }
            else
            {
                WeaponItem weapon = Storage.inst.weaponInventory[i];
                GameObject button = weaponSlots[i];
                SetButtonWeapon(button, weapon);
                weaponSlots[i].GetComponent<Button>().onClick.AddListener(delegate { EquipWeapon(weapon, button); });
            }
        }
    }

    void CloseAllStorageMenus()
    {
        SpellParent.SetActive(false);
        ConsumableParent.SetActive(false);
        WeaponParent.SetActive(false);
    }

    void AddItemToCrafting(Item item, GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        bool added = AddItemToCombinationSlots(item);
        if (added)
        {
            SetButtonEmpty(button);
            int i = combinationSlots.IndexOf(item);
            GameObject new_button = craftingSlots[i];
            SetButtonItem(new_button, item);
            
            craftingSlots[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { combinationSlots.Remove(item); RemoveItemFromCrafting(item, new_button); });
        }
        else
        {
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate { AddItemToCrafting(item, button); });
        }
    }

    void RemoveItemFromCrafting(Item item, GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        SetButtonEmpty(button);        
        for(int i = 0; i < 15; i++)
        {
            GameObject new_button = itemSlots[i];
            if (new_button.GetComponent<UI_Slot>().slotItem == null)
            {
                SetButtonItem(new_button, item);
                new_button.GetComponentInChildren<Button>().onClick.AddListener(delegate { AddItemToCrafting(item, new_button); });
                return;
            }
        }

    }

    public void CraftItems()
    {
        Item[] comboItems = combinationSlots.ToArray();
        if(comboItems.Length < 2)
        {
            DisplayError("Not enough items to craft");
            return;
        }
        foreach(ItemCombinations combination in ItemCombinations.AllCombonations)
        {
            if(CheckEquivalence(combination.Requirements, comboItems))
            {
                combinationSlots.Clear();
                foreach(Item item in comboItems)
                {
                    Storage.inst.inventory.Remove(item);
                }
                foreach(Item item in combination.Outputs)
                {
                    Storage.inst.inventory.Add(item);
                }
                foreach(GameObject button in craftingSlots)
                {
                    SetButtonEmpty(button);
                }
                for(int i = 0; i < combination.Outputs.Length; i++)
                {
                    GameObject button = resultsSlots[i];
                    Item item = combination.Outputs[i];
                    SetButtonItem(button, item);
                    button.SetActive(true);
                    button.GetComponentInChildren<Button>().onClick.AddListener(delegate { RemoveItemFromCrafting(item, button); button.SetActive(false); });

                }
                
            }
        }
        DisplayError("No combination avaliable");
    }

    bool CheckEquivalence(Item[] first, Item[] second)
    {
        if(first.Length != second.Length)
        {
            return false;
        }
        for(int i = 0; i < second.Length; i++)
        {
            if(Array.IndexOf(first, second[i]) == -1)
            {
                return false;
            }
        }
        return true;
    }
    
    void DisplayError(string s)
    {

    }

    bool AddItemToCombinationSlots(Item item)
    {
        if(combinationSlots.Count >= 3 || combinationSlots.Contains(item))
        {
            return false;
        }
        else
        {
            combinationSlots.Add(item);
            return true;
        }
    }

    bool TryEquipSpell(Spell spell)
    {
        if(Storage.inst.equippedSpells.Count > 4 || Storage.inst.equippedSpells.Contains(spell))
        {
            return false;
        }
        else
        {
            Storage.inst.equippedSpells.Add(spell);
            Storage.inst.spellInventory.Remove(spell);
            return true;
        }
    }

    void EquipSpell(Spell spell, GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        bool added = TryEquipSpell(spell);
        if (added)
        {
            SetButtonEmpty(button);
            int i = Storage.inst.equippedSpells.IndexOf(spell);
            GameObject new_button = equippedSpellSlots[i];
            SetButtonSpell(new_button, spell);

            new_button.GetComponentInChildren<Button>().onClick.AddListener(delegate {  UnequipSpell(spell, new_button); });
        }
        else
        {
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate { EquipSpell(spell, button); });
        }
    }

    void UnequipSpell(Spell spell, GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        if(!(Storage.inst.spellInventory.Count > 12 || Storage.inst.spellInventory.Contains(spell)))
        {
            Storage.inst.spellInventory.Add(spell);
            Storage.inst.equippedSpells.Remove(spell);
            SetButtonEmpty(button);
            for (int i = 0; i < 12; i++)
            {
                GameObject new_button = spellSlots[i];
                if (new_button.GetComponent<UI_Slot>().slotSpell == null)
                {
                    SetButtonSpell(new_button, spell);
                    button.GetComponentInChildren<Button>().onClick.AddListener(delegate { UnequipSpell(spell, new_button); });
                    return;
                }
            }
        }
        else
        {
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate { UnequipSpell(spell, button); });
        }
    }

    void EquipWeapon(WeaponItem weapon, GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        SetButtonEmpty(button);
        GameObject new_button = equippedWeapon;
        SetButtonWeapon(new_button, weapon);
        Storage.inst.weaponInventory.Add(Storage.inst.equippedWeapon);
        Storage.inst.equippedWeapon = weapon;
        
        
    }

    public bool hasItem(int s)
    {
       
        Item tool = ItemList.ItemKeys[s];
        return Storage.inst.inventory.Contains(tool);
    }
    public void removeItem(int s)
    {
        Item tool = ItemList.ItemKeys[s];
        if (!Storage.inst.inventory.Contains(tool))
        {
            return;
        }
        Storage.inst.inventory.Remove(tool);
        
    }
    public void addToInventory(int s)
    {
        if (Storage.inst.inventory.Count > 15)
        {
            return;
        }
        Item tool = ItemList.ItemKeys[s];
        Storage.inst.inventory.Add(tool);

    }
    public void AddToSpells(int s)
    {
        if (Storage.inst.spellInventory.Count >= 12)
        {
            return;
        }
        Spell spell = Spell.SpellKeys[s];
        Storage.inst.spellInventory.Add(spell);
    }
    public List<ConsumableItem> GetConsInventory()
    {
        return Storage.inst.consInventory;
    }

    void SetButtonEmpty(GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
        button.GetComponentsInChildren<Image>()[1].sprite = null;
        TextMeshProUGUI[] textMeshProUGUIs = button.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < textMeshProUGUIs.Length; i++)
        {
            textMeshProUGUIs[i].text = null;
        }
        UI_Slot ui_slot = button.GetComponent<UI_Slot>();
        ui_slot.slotItem = null;
        ui_slot.slotSpell = null;
        ui_slot.slotCharm = null;
        ui_slot.slotWeapon = null;
    }

    void SetButtonItem(GameObject button, Item item)
    {
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        button.GetComponentsInChildren<Image>()[1].sprite = item.ItemSprite;
        button.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
        button.GetComponent<UI_Slot>().slotItem = item;
        
    }


    void SetButtonItem(GameObject button, ConsumableItem item)
    {
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        button.GetComponentsInChildren<Image>()[1].sprite = item.ItemSprite;
        button.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.Name;
        button.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "x" + item.Count;
        button.GetComponent<UI_Slot>().slotItem = item;
    }

    void SetButtonSpell(GameObject button, Spell spell)
    {
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        button.GetComponentsInChildren<Image>()[1].sprite = spell.icon;
        button.GetComponentInChildren<TextMeshProUGUI>().text = spell.Name;
        button.GetComponent<UI_Slot>().slotSpell = spell;

    }

    void SetButtonCharm(GameObject button, Charm charm)
    {
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        button.GetComponentsInChildren<Image>()[1].sprite = charm.Icon;
        button.GetComponentInChildren<TextMeshProUGUI>().text = charm.Name;
        button.GetComponent<UI_Slot>().slotCharm = charm;

    }
    void SetButtonWeapon(GameObject button, WeaponItem weapon)
    {
        button.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
        button.GetComponentsInChildren<Image>()[1].sprite = weapon.ItemSprite;
        button.GetComponentInChildren<TextMeshProUGUI>().text = weapon.Name;
        button.GetComponent<UI_Slot>().slotWeapon = weapon;

    }
}
