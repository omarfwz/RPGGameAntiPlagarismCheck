using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GuiManager : MonoBehaviour
{
    public static GuiManager instance;
    Character cur;
    private string curPhase;
    private float clock;
    private float codeTimer = 0.1F;
    private bool dialogueInst; //whether the dialogue has been instantiated
    private bool doAdvancing; // whether dialogue should advance

    [Header("For Player Battle GUI")]
    [SerializeField] GameObject battleGUI;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI mptext;
    [SerializeField] TextMeshProUGUI distance;
    [SerializeField] GameObject damageIndicator;
    [SerializeField] Button[] buttons;
    
    [SerializeField] Button prefab_btnresponse;
    [SerializeField] GameObject warningGUI;

    [Header("For Boss")]
    [SerializeField] GameObject bossGUI;
    [SerializeField] TextMeshProUGUI bossHealth;

    [Header("Healthbar")]
    [SerializeField] GameObject healthbar;
    [SerializeField] Slider healthbarSlider;
    [SerializeField] TextMeshProUGUI maxHealth;
    [SerializeField] TextMeshProUGUI curHealth;

    [Header("Player Healthbar")]
    [SerializeField] GameObject pHealthBar;
    [SerializeField] Slider pHealthbarSlider;
    [SerializeField] TextMeshProUGUI pmaxHealth;
    [SerializeField] TextMeshProUGUI pcurHealth;

    [Header("Distance")]
    [SerializeField] GameObject distanceShower;
    TextMeshProUGUI distanceCounter;

    [Header("Items Menu")]
    [SerializeField] GameObject itemsMenu;
    [SerializeField] Transform itemsGrid;
    [SerializeField] GameObject forwardButton;
    [SerializeField] GameObject backButton;

    [Header("Abilities Menu")]
    [SerializeField] GameObject abilitiesMenu;
    [SerializeField] Transform abilitiesGrid;

    [Header("Description Shower")]
    [SerializeField] GameObject desc_container;
    [SerializeField] TextMeshProUGUI desc_title;
    [SerializeField] TextMeshProUGUI desc_description;
    [SerializeField] Image desc_image;

    [Header("Weapon Details")]
    [SerializeField] TextMeshProUGUI wp_damage;
    [SerializeField] TextMeshProUGUI wp_idealdistance;
    [SerializeField] TextMeshProUGUI wp_range;
    [SerializeField] TextMeshProUGUI wp_desc;
    [SerializeField] Image wp_icon;

    [Header("Timer")]
    [SerializeField] GameObject gui_timer;
    [SerializeField] TextMeshProUGUI text_timer;

    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple battle controller scripts");
        }
        instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {


        BattleController.instance.ChangePhase += Activate;
        cur = Fundamental.instance.main_Character;
        distanceCounter = distanceShower.GetComponentInChildren<TextMeshProUGUI>();

        //weapon details
        wp_damage.text = cur.Weapon.Attack + "";
        if(cur.Weapon.BestDis - cur.Weapon.Range < 0)
        {
            wp_range.text = 0 + "-" + (cur.Weapon.BestDis + cur.Weapon.Range);
        }
        else
        {
            wp_range.text = cur.Weapon.BestDis - cur.Weapon.Range + "-" + (cur.Weapon.BestDis + cur.Weapon.Range);
        }
        
        wp_icon.sprite = cur.Weapon.ItemSprite;
        wp_idealdistance.text = cur.Weapon.BestDis + "";
        wp_desc.text = cur.Weapon.Description;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        if(clock > codeTimer)
        {
            if (curPhase == "BossTurn")
            {
                distanceCounter.text = BattleController.instance.CalculateDistance() + "";
            }
            clock = 0;
        }
        
        
    }
    
    void Activate(string phase)
    {

        if(phase != "PlayerTurn")
        {   
            if(phase == "Dialogue")
            {
                DialoguePhase();
            }
            return;
        }
        playerControl.instance.DisableControls();
        playerControl.instance.playerController.UI.Enable();
      
        EnableButtons(true);

        

    }
    
    public void Deactivate()
    {
        CloseAllMenus();
        battleGUI.SetActive(false);
        bossGUI.SetActive(false);
        dialogueBox.SetActive(false);
        distanceShower.SetActive(true);
        DialogueManager.instance.PauseDialogue();

        //for boss
        healthbar.SetActive(true);
        healthbarSlider.maxValue = BossManager.instance.maxbosshealth;
        healthbarSlider.value = BossManager.instance.bosshealth;
        maxHealth.text = BossManager.instance.maxbosshealth + "";
        curHealth.text = BossManager.instance.bosshealth + "";

        //for player
        pHealthBar.SetActive(true);
        pHealthbarSlider.maxValue = cur.MaxHP;
        pHealthbarSlider.value = cur.HP;
        pmaxHealth.text = cur.MaxHP + "";
        pcurHealth.text = cur.HP + "";

        BattleController.instance.ResetPosition();
        playerControl.instance.playerController.UI.Disable();
        playerControl.instance.setUpControls();

        BattleController.instance.InvokePhase("BossTurn");
        curPhase = "BossTurn";
    }

    public void AttackBoss()
    {
        EnableButtons(false);
        BattleController.instance.AttackBoss();
        bossHealth.text = BossManager.instance.bosshealth + "";
        

        Invoke(nameof(Deactivate), 1.0f);
    }

    public void UpdateHealthBar(int health)
    {
        pcurHealth.text = health+"";
        pHealthbarSlider.value = health;
    }

    public void EnableButtons(bool state)
    {
        Debug.Log(state);
        Button[] buttons = battleGUI.GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            button.interactable = state;
            
        }
        if (state)
        {
            buttons[0].Select();
        }
    }



    public void CloseAllMenus()
    {
        abilitiesMenu.SetActive(false);
        desc_container.SetActive(false);
        itemsMenu.SetActive(false);

    }
    public static void KillAllChildren(UnityEngine.Transform parent)
    {
        UnityEngine.Assertions.Assert.IsNotNull(parent);
        for (int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--)
        {
            Destroy(parent.GetChild(childIndex).gameObject);
        }
    }
    public void OpenAbilitiesMenu()
    {
        if(cur.PlayerSpells.Count == 0)
        {
            Warn("No Spells Equipped!");
            return;
        }
        CloseAllMenus();
        desc_container.SetActive(true);
        abilitiesMenu.SetActive(true);
        KillAllChildren(abilitiesGrid);

        for(int i = 0; i< cur.PlayerSpells.Count; i++)
        {
            Spell spell = cur.PlayerSpells[i];
            var responseButton = Instantiate(prefab_btnresponse, abilitiesGrid);
            ButtonScript values = responseButton.GetComponent<ButtonScript>();
            responseButton.GetComponentInChildren<TextMeshProUGUI>().text = (spell.Name);

            values.description = spell.Description;
            values.title = spell.Name;
            values.icon = spell.icon;
            values.descriptionText = desc_description;
            values.titleText = desc_title;
            values.imageD = desc_image;

            responseButton.onClick.AddListener(delegate { UseAbility(spell); });
            if (i == 0)
            {
                responseButton.Select();
                values.first = true;
            }
        };
    }
    public void OpenItemsMenu(int index)
    {

        if(index < 0 || index > 2 || cur.Inventory.Count == 0)
        {
            Warn("No Items Left!");
            return;
        }
        CloseAllMenus();
        desc_container.SetActive(true);
        itemsMenu.SetActive(true);
        KillAllChildren(itemsGrid);

        Button forwardButton_B = forwardButton.GetComponent<Button>();
        Button backButton_B = backButton.GetComponent<Button>();
        forwardButton_B.onClick.RemoveAllListeners();
        backButton_B.onClick.RemoveAllListeners();
        forwardButton_B.onClick.AddListener(delegate { OpenItemsMenu(index + 1); });
        backButton_B.onClick.AddListener(delegate { OpenItemsMenu(index - 1); });
        forwardButton.SetActive(index < 2 && (index+1)*4<cur.Inventory.Count);
        backButton.SetActive(index != 0);


        for (int i = index*4; i < index*4+4; i++)
        {
            if(cur.Inventory.Count <= i)
            {
                continue;
            }

            ConsumableItem item = cur.Inventory[i];
            var responseButton = Instantiate(prefab_btnresponse, itemsGrid);
            ButtonScript values = responseButton.GetComponent<ButtonScript>();
            responseButton.GetComponentInChildren<TextMeshProUGUI>().text = (item.Name);

            values.description = item.Description;
            values.title = item.Name;
            values.descriptionText = desc_description;
            values.titleText = desc_title;
            values.imageD = desc_image;
            
            
            responseButton.onClick.AddListener(delegate { UseItem(item); });
            if (i == index * 4)
            {
                responseButton.Select();
                values.first = true;

            }
        };
        
    }
    public void UseAbility(Spell spell)
    {
       if (cur.MP < spell.Cost)
        {
            Warn("Not Enough MP");
            return;
        }   
        EnableButtons(false);
        desc_container.SetActive(false);
        BattleController.instance.UseAbility(spell);
        bossHealth.text = BossManager.instance.bosshealth + "";
        health.text = cur.HP + "";
        mptext.text = cur.MP + "";

        Invoke(nameof(Deactivate), 1.0f);
    }

    public void UseItem(ConsumableItem item)
    {
        EnableButtons(false);
        desc_container.SetActive(false);
        BattleController.instance.UseItem(item);
        bossHealth.text = BossManager.instance.bosshealth + "";
        health.text = cur.HP + "";
        mptext.text = cur.MP + "";

        Invoke(nameof(Deactivate), 1.0f);
    }
    void Warn(string message)
    {
        TextMeshProUGUI warning_text = warningGUI.GetComponentInChildren<TextMeshProUGUI>();
        warningGUI.SetActive(true);
        warning_text.text = message;
        EnableButtons(false);
        
        Invoke(nameof(DeactivatePopUp), 1.0f);
    }
    public void DeactivatePopUp()
    {
        warningGUI.SetActive(false);
        EnableButtons(true);
    }

   public void UpdateTimer(int time_parameter)
    {
        if (!gui_timer.activeInHierarchy)
        {
            gui_timer.SetActive(true);
        }
        text_timer.text = time_parameter + "";
    }

    public void IndicateDamage(int dmg)
    {
        damageIndicator.SetActive(true);
        damageIndicator.GetComponentInChildren<TextMeshProUGUI>().text = dmg + "";
        Invoke(nameof(DeactivateIndicator), 1.0f);

    }

    void DeactivateIndicator()
    {
        damageIndicator.SetActive(false);
    }

    //for dialogue phase, goes into player turn which has controls
    void DialoguePhase()
    {
        playerControl.instance.DisableControls();
        playerControl.instance.playerController.UI.Enable();
        
        //gui stuff
        EnableButtons(false);
        battleGUI.SetActive(true);
        health.text = cur.HP + "/" + cur.MaxHP;
        mptext.text = cur.MP + "/" + cur.MaxMP;
        distance.text = BattleController.instance.CalculateDistance()+"";
        bossGUI.SetActive(true);
        bossHealth.text = BossManager.instance.bosshealth + "";

        healthbar.SetActive(false);
        pHealthBar.SetActive(false);
        gui_timer.SetActive(false);
        
        DialogueManager.instance.ResumeDialogueCtrl();
        if (!dialogueInst)
        {
            if (BossManager.instance.dialogue != null)
            {
                DialogueManager.instance.InstantiateBattleDialogue(BossManager.instance.dialogue);
                dialogueInst = true;
            }
        }
        if (doAdvancing)
        {
            int req = DialogueManager.instance.GetRequiredHealth();
            if (BossManager.instance.bosshealth <= req || req == -1)
            {
                DialogueManager.instance.AdvanceDialogue();
            }
            else
            {
                FinishDialogue();
            }

        }
        else if(!dialogueInst)
        {
            FinishDialogue();
        }

        if (dialogueInst)
        {
            doAdvancing = true;
        }
        
    }

    public void FinishDialogue()
    {
        DialogueManager.instance.PauseDialogueCtrl();
        BattleController.instance.InvokePhase("PlayerTurn");
    }
}
