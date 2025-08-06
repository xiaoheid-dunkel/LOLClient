using UnityEngine;
using System.Collections;
using GameProtocol;

public class FightHandler : MonoBehaviour,Ihandler {




    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case FightProtocol.START_BRO:
                //start(model.GetMessage<FightRoomModel>());//生成角色和箭塔初始化
                break;
            case FightProtocol.MOVE_BRO:
               //  move(model.GetMessage<MoveDTO>());//移动
                break;
            case FightProtocol.ATTACK_BRO:
               //  Attack(model.GetMessage<AttackDTO>());//攻击
                break;
            case FightProtocol.DAMAGE_BRO:
                //damage(model.GetMessage<DamageDTO>());//伤害
                break;
            case FightProtocol.SKILL_UP_SRES:
               // skillLevelUp(model.GetMessage<FightSkill>());//技能升级
                break;
            case FightProtocol.SKILL_BRO:
               // skill(model.GetMessage<SkillAtkModel>());//技能
                break;
        }
    }
}
