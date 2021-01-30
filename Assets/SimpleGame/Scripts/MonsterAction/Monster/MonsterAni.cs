using UnityEngine;

public class MonsterAni : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GameObject.Find("Monster1/Monster1").GetComponent<Animator>();
    }

    private void Start()
    {
        SimpleMsgMechanism.ReceiveMsg("MonsterIsMove", objects =>
        {
            bool isMove = (bool) objects[0];
            _animator.SetInteger("move", isMove ? 1 : 0);
        });
    }
}