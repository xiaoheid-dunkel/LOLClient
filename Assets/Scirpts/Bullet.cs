using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private GameObject target;
    public float speed=20f;
    public Tower tower;
     
	void Start () {
        tower = GetComponentInParent<Tower>();
        Destroy(this.gameObject, 1f);
	}

    //子弹撞到士兵或者英雄
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "soldier")
        {
            Health hp=col.GetComponent<Health>();
            if (hp)
            {
                hp.TakeDamage(0.5f);
                if (hp.hp.Value <= 0)
                {
                    tower.listSoldier.Remove(col.gameObject);
                    Destroy(col.gameObject);
                }
                Destroy(this.gameObject);
            }
            
        }
       
        else if (col.gameObject.tag == "Player")
        {
            //销毁英雄

            Health hp = col.GetComponent<Health>();
            if (hp)
            {
                hp.TakeDamage(0.2f);
                if (hp.hp.Value <= 0)
                {
                    tower.listHero.Remove(col.gameObject);
                    Destroy(col.gameObject);
                }
                Destroy(this.gameObject);
            }
           
        }
    }  
	void Update () {
        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        }
        else
        {

            Destroy(this.gameObject);
        }

	}
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

}
