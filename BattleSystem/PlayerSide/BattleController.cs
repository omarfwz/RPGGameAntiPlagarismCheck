using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public string phase;
    public event Action<string> ChangePhase;
    //player variables
    public Character cur;
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple battle controller scripts");
        }
        instance = this;
        
        
    }
    void Start()
    {
        cur = Fundamental.instance.main_Character;
        ResetPosition();

    }

    public void InvokePhase(string invokedPhase)
    {
        ChangePhase.Invoke(invokedPhase);
    }
    //for weapon
    public void AttackBoss()
    {
        int dis = CalculateDistance();
        float attackMultiplier;
        if(dis > cur.Weapon.BestDis + cur.Weapon.Range || dis < cur.Weapon.BestDis - cur.Weapon.Range)
        {
            attackMultiplier = 0;
        } else if (dis == cur.Weapon.BestDis)
        {
            attackMultiplier = 1.5F;
        } else
        {
            attackMultiplier = 1 - (Math.Abs(cur.Weapon.BestDis - dis) / (float)cur.Weapon.Range);
        }

        attackMultiplier += Random.Range(-0.1F, 0.1F);
        int dmg = (int)MathF.Round((cur.Attack + cur.Weapon.Attack) * attackMultiplier);
        AttackBoss(dmg);
    }

    
    public void AttackBoss(int damage)
    {
        
        BossManager.instance.TakeDamage(damage);
    }


    public void HealPlayer(int amount)
    {
        if (cur.HP + amount > cur.MaxHP)
        {
            cur.HP = cur.MaxHP;
        }
        else
        {
            cur.HP += amount;
        }
    }
    public void UseAbility(Spell ability)
    {
        if(ability.Type == "Healing")
        {
            if(ability.Subtype == "Scaler")
            {
                HealPlayer((int)Math.Round(ability.Multiplier * 100));
            } else if (ability.Subtype == "Direct")
            {
                HealPlayer(ability.RawValue);
            }
            
        } else if(ability.Type == "Damaging")
        {
            if (ability.Subtype == "Scaler")
            {
                AttackBoss((int)Math.Round(ability.Multiplier * 10));
            }
            else if (ability.Subtype == "Direct")
            {
                AttackBoss(ability.RawValue);
            }

            
        } else if(ability.Type == "Weaken")
        {
            if (ability.Subtype == "Scaler")
            {
                BossManager.instance.curAttack = (int)Math.Round(BossManager.instance.curAttack*ability.Multiplier);
            }
            else if (ability.Subtype == "Direct")
            {
                if(BossManager.instance.curAttack > ability.RawValue + 2)
                {
                    BossManager.instance.curAttack -= ability.RawValue;
                }
                else
                {
                    BossManager.instance.curAttack = 2;
                }
                
                
            }
        }
        cur.MP -= ability.Cost;
    }

    public void UseItem(ConsumableItem item)
    {
        if(item.Type == "Healing")
        {
            if(item.SubType == "Direct")
            {
                HealPlayer(item.RawValue);
            }
        }
        cur.Inventory.Remove(item);
   
    }
    public void TakeDamage(int num)
    {
        if(num < cur.HP)
        {
            cur.HP -= num;
        }
        else
        {
            cur.HP = 0;
            PlayDeath();
        }
        
        GuiManager.instance.UpdateHealthBar(cur.HP);
        
    }

    public int CalculateDistance()
    {
        Transform bossPos = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        return (int)Math.Round(Math.Sqrt(2*Math.Pow(bossPos.position.x - playerPos.position.x,2 ) + Math.Pow(bossPos.position.y - playerPos.position.y,2)));
        
    }

    public void ResetPosition()
    {
        Rigidbody2D playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Vector2 spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint").GetComponent<Transform>().position;
        playerPos.MovePosition(spawnPoint);
    }

    public void PlayDeath()
    {
        DataPersistenceManager.instance.StartLoadedGame();
    }
}
