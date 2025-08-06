using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour {


    public List<GameObject> listSoldier = new List<GameObject>();//小兵攻击箭塔队列
    public List<GameObject> listHero = new List<GameObject>();//英雄攻击箭塔队列
    public int towerType;
    [SerializeField]
    private  GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletStart;
    [SerializeField]
    private Transform parent;
    void Start()
    {
        if (this.gameObject.tag.Equals("Tower"))
        {
            towerType = 0;
        }
        else
        {
            towerType = 1;
        }
        InvokeRepeating("CreatBullet", 0.1f, 1.3f);
    }

    public void CreatBullet()
    {
        if (listHero.Count == 0 && listSoldier.Count == 0) return;

        GameObject bullet=(GameObject) Instantiate(bulletPrefab, bulletStart.position,Quaternion.identity);
        bullet.transform.parent = parent;
        BulletTarget(bullet);//设置子弹攻击目标
    }
   /// <summary>
   /// 设置子弹攻击目标，先从小兵集合中取目标，再从英雄集合中取
   /// </summary>
   /// <param name="bullet"></param>
    public void BulletTarget(GameObject bullet)
    {
        if (listSoldier.Count > 0)
        {
            bullet.GetComponent<Bullet>().SetTarget(listSoldier[0]);
        }
        else
        {
            bullet.GetComponent<Bullet>().SetTarget(listHero[0]);
        }

    }


    void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.tag=="Player"){
            listHero.Add(col.gameObject);
        }
        else
        {
             SmartSoldier soldier = col.GetComponent<SmartSoldier>();
            if (soldier&&soldier.type!=towerType)
            {
               
                listSoldier.Add(col.gameObject);
            }
        }    
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            listHero.Remove(col.gameObject);
        }else        
        {
            listSoldier.Remove(col.gameObject);
        }
    }
}
