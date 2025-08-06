using UnityEngine;
using System.Collections;

public class CreatSoldier : MonoBehaviour {
    [SerializeField]
    private GameObject soldierPrefab;
    [SerializeField]
    private Transform soldierParent;
    private  bool isCreatSoldier=true;

    [SerializeField]
    private Transform[] middleTowers;
    [SerializeField]
    private Transform[] LeftTowers;
    [SerializeField]
    private Transform[] RightTowers;
    [SerializeField]
    private Transform[] middleEnemyTowers;
    [SerializeField]
    private Transform[] LeftEnemyTowers;
    [SerializeField]
    private Transform[] RightEnemyTowers;

    [SerializeField]
    private Transform[] Start1;
    [SerializeField]
    private Transform[] Start2;

    public int soldierCount=2;

    void Start()
    {
        StartCoroutine(Creat(2,1,10));
    }

    void CreatSmartSoldier(SoldierType soldierType,Transform startTran, Transform[] towers,int road)
    {
        GameObject obj = Instantiate(soldierPrefab, startTran.position, Quaternion.identity) as GameObject;
        obj.transform.parent = soldierParent;

        SmartSoldier soldier = obj.GetComponent<SmartSoldier>();
        soldier.towers = towers;//指定目标塔
        soldier.SetRoad(road);
        soldier.type = (int)soldierType;
    }

    private IEnumerator Creat(float time, float delyTime, float spwanTime)
    {
        yield return new WaitForSeconds(time);//几秒后开始生成士兵
        while (isCreatSoldier)
        {
            for (int i = 0; i < soldierCount; i++)
            {              
                CreatSmartSoldier(SoldierType.soldier1, Start1[0], middleEnemyTowers,1<<3);//中路我方小兵
                CreatSmartSoldier(SoldierType.soldier2, Start2[0], middleTowers, 1 << 3);//中路敌方小兵

                CreatSmartSoldier(SoldierType.soldier1, Start1[1], LeftEnemyTowers, 1 << 4);//上（左）路我方小兵
                CreatSmartSoldier(SoldierType.soldier2, Start2[1], LeftTowers, 1 << 4);//上（左）路敌方小兵

                CreatSmartSoldier(SoldierType.soldier1, Start1[2], RightEnemyTowers, 1 << 5);//下（右）路我方小兵
                CreatSmartSoldier(SoldierType.soldier2, Start2[2], RightTowers, 1 << 5);//下（右）路敌方小兵

                yield return new WaitForSeconds(delyTime); //生成下一个小兵的时间间隔
            }
            yield return new WaitForSeconds(spwanTime); //生成下一波的时间间隔
        }


    }

  
}