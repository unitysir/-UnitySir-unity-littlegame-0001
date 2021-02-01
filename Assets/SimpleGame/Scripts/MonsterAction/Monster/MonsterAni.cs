using System;
using UnityEngine;

public class MonsterAni : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private MonsterCtrl _monsterCtrl;

    [Header("是否移动")] [SerializeField] private bool isMove;
    [Header("是否攻击")] [SerializeField] private bool isAttack;

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _monsterCtrl = GetComponent<MonsterCtrl>();
    }

    private void Update()
    {
        isMove = _monsterCtrl.isMove;
        _animator.SetInteger("move", isMove ? 1 : 0);

        isAttack = _monsterCtrl.isAttack;
        if (isAttack) _animator.SetTrigger("attack");
    }
}