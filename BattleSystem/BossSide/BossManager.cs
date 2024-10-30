using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BossManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] bool debug;
    [SerializeField] Transform center;
    public TextAsset dialogue;
    public bool usePosition;

    public static BossManager instance;

    [Header("Transfer Details")]
    [SerializeField] int bossID;
    [SerializeField] int respawnID;
    [SerializeField] string scenerespawnName;
    [SerializeField] int[] sceneEvents;

    [Header("Boss Details")]
    public int curPhase;
    public int numOfPhases;
    public int[] healthRanges;
    public int maxbosshealth;
    public int bosshealth;
    public int def;
    public int curDef;
    public int attack;
    public int curAttack;
    public int xp;

    private GameObject boss_Attack;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple battle controller scripts");
        }
        instance = this;
    }
    private void Start()
    {

        if (debug)
        {
            Invoke(nameof(InitializeBoss), 0.2f);
        }
        else
        {
            InitializeBoss();
        }
        

    }
    private void InitializeBoss()
    {
        // Puts in boss dialogue and starts the boss
        
        if (Fundamental.instance.overrideText != null)
        {
            dialogue = Fundamental.instance.overrideText;
        }
        
        BattleController.instance.ResetPosition();
        BattleController.instance.ChangePhase += StartAttack;
        BattleController.instance.InvokePhase("Dialogue");

    }
    private void StartAttack(string phase)
    {
        if (phase != "BossTurn")
        {
            Destroy(boss_Attack);
            return;
        }
        ChoosePhase();
        SpawnAttacks();
    }
    private void ChoosePhase()
    {
        for(int i = 0; i < healthRanges.Length; i++)
        {
            if(i < healthRanges.Length - 1)
            {
                if(healthRanges[i] > bosshealth && healthRanges[i+1] < bosshealth)
                {
                    curPhase = i + 1;
                    if(curPhase > numOfPhases)
                    {
                        Debug.LogWarning("Num of Phases messed up.");
                    }
                }
            }
        }

        gameObject.GetComponentsInChildren<PhaseScript>();

    }

    private void SpawnAttacks()
    {
        PhaseScript[] PhaseKeeper = GetComponentsInChildren<PhaseScript>();
        PhaseScript attack_Phase = PhaseKeeper[0].GetComponent<PhaseScript>();
        foreach(PhaseScript obj in PhaseKeeper)
        {
            
            if (obj.phase == curPhase)
            {
                attack_Phase = obj;
                break;
            }
          
        }
        int variation = Random.Range(0, attack_Phase.variations.Length);
        boss_Attack = Instantiate(attack_Phase.variations[variation], center);
        attack_Phase.isActive = true;
        attack_Phase.varnum = variation;
        boss_Attack.SetActive(true);

    }

    public void TakeDamage(int damage)
    {
        GuiManager.instance.IndicateDamage(damage);
        if (bosshealth > damage / (curDef / 10 + 1))
        {
            bosshealth -= damage / (curDef / 10 + 1);
        }
        else
        {
            bosshealth = 0;
            PlayBossDeath();
        }
    }

    void PlayBossDeath()
    {
        Fundamental.instance.AddBossBeaten(bossID, xp);
        if (usePosition)
        {
            Fundamental.instance.LoadOutsideScene(scenerespawnName);
        }
        else
        {
            Fundamental.instance.LoadOutsideScene(scenerespawnName, respawnID);
        }
        
    }
}
