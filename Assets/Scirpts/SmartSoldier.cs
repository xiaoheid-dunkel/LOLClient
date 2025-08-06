using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SmartSoldier : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    private Animation ani;
    private float distance;
    private List<Transform> enemyList = new List<Transform>();

    public Transform target;
    public Transform[] towers;
    private Health HP;

    
    public int type=0;
	// Use this for initialization
	void Start () {
        HP=GetComponent<Health>();
	    agent =GetComponent<UnityEngine.AI.NavMeshAgent>();
        ani=GetComponent<Animation>();
        target = GetTarget();
	}
	
	// Update is called once per frame
	void Update () {
        SoldierMove();
	}
    /// <summary>
    /// 士兵移动
    /// </summary>
    void SoldierMove()
    {
        if (target == null)
        {
            target = GetTarget();
            return;
        }
        ani.CrossFade("Run");
        agent.SetDestination(target.position);

        distance = Vector3.Distance(transform.position, target.position);
        if (distance>5)
        {
            agent.speed = 3.5f;
        }
        else
        {
            agent.speed = 0;
            Vector3 tarPos = target.position;
            Vector3 lookPos = new Vector3(tarPos.x,transform.position.y,tarPos.z);
            transform.LookAt(lookPos);
            ani.CrossFade("Attack1");

        }
      
    }
    /// <summary>
    ///进入小兵攻击范围
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter(Collider col)
    {
        //判断是否为英雄，否则就是小兵
        if (col.gameObject.tag=="Player"&&this.type==1)
        {
            print("遇见英雄");
            this.enemyList.Add(col.transform);
            Transform temp = this.enemyList[0];

            if (target == null || temp != target)//判断target是不是空或者相同，不相同和 为空情况下对target赋值
            {
                target = temp; 
            }

        }else{
            //获取碰到的物体，看这个物体上有没有SmartSoldier脚本，有就说明是士兵
            SmartSoldier soldier = col.GetComponent<SmartSoldier>();
            if (soldier != null  //如果是空就说明不是士兵
                && soldier.type != this.type //判断碰到的这个士兵是不是与自身相同，不相同就说明是敌人
                && !this.enemyList.Contains(col.transform))//检查敌人List数组是不是包含这个敌人，不包含就添加进去，避免重复添加
            {
                this.enemyList.Add(col.transform);
                Transform temp = this.enemyList[0];
                if (target == null || temp != target) //判断target是不是空或者相同，不相同和 为空情况下对target赋值
                {
                    target = temp;
                }
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
      
        if (this.enemyList.Contains(col.transform))
        {
            this.enemyList.Remove(col.transform);
            target = GetTarget();        
        }

    }

    /// <summary>
    /// 士兵的伤害
    /// </summary>
    public  void TakeDamage(float damage)
    {
        HP.hp.Value -= damage;
        if (HP.hp.Value <= 0)
        {
            Destroy(gameObject);
        } 

    }
   
    /// <summary>
    /// 小兵攻击，此事件在动画中调用
    /// </summary>

    public  void  Attack()
    {
      
        if (target == null)
        {
            target = GetTarget();
            return;
        }
        Health  heath=target.GetComponent<Health>();
        float damage = Random.Range(0.1f,0.6f);
        heath.TakeDamage(damage);

        if (heath.hp.Value<=0)
        {
            Destroy(target.gameObject);      
            //移除目标对象
            if (this.enemyList.Contains(target)) //判断这个target是否在数组中 因为塔在另一个数组里
            {
                this.enemyList.Remove(target);
            }
            else //士兵如果没有包含  那么就是塔了
            {
                DestroyTowerInList(target);
            }
            //获取下一个目标
            target = GetTarget();
            if (target)
            {
                agent.SetDestination(target.transform.position);
                agent.speed = 3.5f;
                ani.CrossFade("Run");
            }

        }

    }
    /// <summary>
    /// 当塔消失时，塔从列表中清除
    /// </summary>
    /// <param name="destroyTower"></param>
    void DestroyTowerInList(Transform detroyTower)
    {
        for (int i = 0; i <towers.Length; i++)
        {
            if (towers[i]==detroyTower)
            {
                towers[i] = null;
                return;
            }
        }
    }
  
    /// <summary>
    /// 获取攻击目标
    /// </summary>
    /// <returns></returns>

    Transform GetTarget()
    {
        this.enemyList.RemoveAll(t=>t==null);

        if (enemyList.Count > 0) //判断数组里是否有值
        {
            //直接拿第一个
            return enemyList[0];
        }

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null)//判断哪个不是空
            {
                return towers[i];//不为空直接返回这个塔
            }
        }
        return null;
    }
    //设置小兵行走的路
    public void SetRoad(int road)
    {
       agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.areaMask=road;
    }


  
}
