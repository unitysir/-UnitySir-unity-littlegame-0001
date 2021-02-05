using System;
using UnityEngine;

/// <summary>
/// 怪物攻击角色的伤害计算
/// </summary>
public class MonsterCalcDamage : MonoBehaviour
{
    [SerializeField] private MonsterCtrl _monsterCtrl;

    /// <summary>
    /// 怪物当前的一些属性
    /// </summary>
    private MonsterProperty _monsterProperty;

    private void Awake()
    {
        _monsterCtrl = GetComponentInParent<MonsterCtrl>();
        _monsterProperty = new MonsterProperty
        {
            Hp = 100,
            Damage = 5,
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_monsterCtrl.isAttack && other.gameObject.layer == 8)
        {
            Debug.Log("攻击了角色！！");
        }
    }

    private void Update()
    {
        Debug.Log(transform.position);
    }
}