using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerAattack : MonoBehaviour {
    [SerializeField]
    private ParticleSystem fire1;
    [SerializeField]
    private ParticleSystem fire2;
    private Animator ani;
    private int type;
    private List<GameObject> enemyList = new List<GameObject>();
    void Start()
    {
        ani = GetComponent<Animator>();
        if (this.tag=="Player")
        {
            type = 0;  
        }
        else
        {
            type = 1;
        }
    }
    void OnTriggerExit(Collider col)
    {      
        enemyList.Remove(col.gameObject);
        enemyList.RemoveAll(t=>t==null);
        print(enemyList.Count);
    }
    void OnTriggerEnter(Collider col)
    {
        if (!this.enemyList.Contains(col.gameObject))
        {
            enemyList.Add(col.gameObject);  
        }
       
    }  


   public void Atk1()
    {
        ani.SetInteger("state",AnimState.ATTACK1);
        if (enemyList.Count <= 0) return;    
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].name.Contains("Tower"))
            {
                SmartSoldier soldier = enemyList[i].GetComponent<SmartSoldier>();
                if (soldier&&soldier.type!=this.type)
                {                   
                  Health hp=soldier.GetComponent<Health>();
                  hp.TakeDamage(0.5f);
                     if (hp.hp.Value<=0)
	                {
                        enemyList.Remove(soldier.gameObject);
                        Destroy(soldier.gameObject);
	                }
                }               
            }         
        }      
    }
   public void Atk2()
    {
        ani.SetInteger("state", AnimState.ATTACK2);
        if (enemyList.Count <= 0) return;   
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].name.Contains("Tower"))
            {
                SmartSoldier soldier = enemyList[i].GetComponent<SmartSoldier>();
                if (soldier && soldier.type != this.type)
                {
                    soldier.TakeDamage(1f);
                }
              
            }
           
        }
    }
   public  void Dance()
    {
        ani.SetInteger("state", AnimState.DANCE);
    }
   public void EffectPlay1()
   {
       fire1.Play();
   }
   public void EffectPlay2()
   {
       fire2.Play();
   }
   public void ResetIdle()
   {
       ani.SetInteger("state", AnimState.IDLE);
   }
}
