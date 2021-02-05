using System;
using UnityEngine;

/// <summary>
/// 角色攻击怪物的伤害计算
/// </summary>
public class SimplePlayerCalcDamage : MonoBehaviour
{
    [SerializeField] private SimplePlayerCtrl _simplePlayerCtrl;
    private SimplePlayerProperty _simplePlayerProperty;

    private void Awake()
    {
        _simplePlayerCtrl = GetComponentInParent<SimplePlayerCtrl>();
        _simplePlayerProperty = new SimplePlayerProperty
        {
            Hp = 100,
            Damage = 10,
            Def = 3,
        };
        print("layer = " + gameObject.layer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_simplePlayerCtrl.isNormalAttack && other.gameObject.layer == 7)
        {
            Debug.Log("打击怪物！！");
        }
    }
}