using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˺���������
/// </summary>
public class DamageCalcuCenter
{
    /// <summary>
    /// ��������(��������������X��1+����/250)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetADAtck(LogicActor attacker)
    {
        return attacker.AD * (1 + attacker.STR / 250);
    }

    /// <summary>
    /// ħ��������(��������������X��1+����/250)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetAPAtck(LogicActor attacker)
    {
        return attacker.AP * (1 + attacker.INT / 250);
    }

    /// <summary>
    /// ��ȡ�����˺����� ���˰ٷֱ�=�������/���������ȼ�X200+������������˷ⶥ75%
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetADReduction(LogicActor attacker, LogicActor attackTarget)
    {
        FixInt damageReductionRate = attackTarget.ADDef / (attacker.Level * 200 + attackTarget.ADDef);
        return damageReductionRate > 0.75 ? 0.75 : damageReductionRate;
    }

    /// <summary>
    /// ��ȡħ���˺����� ���˰ٷֱ�=�������/���������ȼ�X200+������������˷ⶥ75%
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetAPReduction(LogicActor attacker, LogicActor attackTarget)
    {
        FixInt damageReductionRate = attackTarget.APdef / (attacker.Level * 200 + attackTarget.APdef);
        return damageReductionRate > 0.75 ? 0.75 : damageReductionRate;
    }

    /// <summary>
    /// ��ȡ�������˺� �����˺�=���˺�X��100%+50%��
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetADPCTDamage(FixInt totalDamage, LogicActor target)
    {
        return totalDamage * (1 + target.PCT);
    }
    /// <summary>
    /// ��ȡħ�������˺� �����˺�=���˺�X��100%+50%��
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetAPMCTDamage(FixInt totalDamage, LogicActor target)
    {
        return totalDamage * (1 + target.MCT);
    }
    /// <summary>
    /// �������˺�
    /// </summary>
    /// <returns></returns>
    public static FixInt CaclulateDamage(SkillDamageConfig damageCfg, LogicActor attacker, LogicActor attackTarget)
    {
        FixInt finalDamage = FixInt.Zero;
        switch (damageCfg.damageType)
        {
            case DamageType.None:
            case DamageType.ADDamage:
                finalDamage = GetADReduction(attacker, attackTarget) * GetADAtck(attacker);
                break;
            case DamageType.APDamage:
                finalDamage = GetAPReduction(attacker, attackTarget) * GetAPAtck(attacker);
                break;

        }
        return finalDamage * (damageCfg.damageRate / 100);
    }

    /// <summary>
    /// �������˺�
    /// </summary>
    /// <returns></returns>
    public static FixInt CaclulateDamage(BuffConfig buffCfg, LogicActor attacker, LogicActor attackTarget)
    {
        FixInt finalDamage = FixInt.Zero;
        DamageType damageType = buffCfg.targetConfig.isOpen ? buffCfg.targetConfig.damageCfg.damageType : buffCfg.damageType;
        switch (damageType)
        {
            case DamageType.None:
            case DamageType.ADDamage:
                finalDamage = GetADReduction(attacker, attackTarget) * GetADAtck(attacker);
                break;
            case DamageType.APDamage:
                finalDamage = GetAPReduction(attacker, attackTarget) * GetAPAtck(attacker);
                break;

        }
        return finalDamage * (buffCfg.targetConfig.isOpen ? (buffCfg.targetConfig.damageCfg.damageRate / 100) : finalDamage * (buffCfg.damageRate / 100));
    }

}
