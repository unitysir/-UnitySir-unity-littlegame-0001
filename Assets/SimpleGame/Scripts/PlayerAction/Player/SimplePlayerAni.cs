using UnityEditor;
using UnityEngine;

public class SimplePlayerAni : MonoBehaviour
{
    private Animator _animator;
    private SimplePlayerCtrl _playerCtrl;

    void Awake()
    {
        _animator = GameObject.Find("MainPlayer/Player").GetComponent<Animator>();
        _playerCtrl = GetComponent<SimplePlayerCtrl>();
    }

    void Update()
    {
        SetMoveAni();
        SetNormalAttack();
    }

    /// <summary>
    /// 设置移动动画
    /// </summary>
    void SetMoveAni()
    {
        float dirMag = _playerCtrl.dirMag;
        bool isRun = _playerCtrl.isRunning;
        _animator.SetFloat("ForwardMove",
            dirMag * (isRun
                ? (Mathf.Lerp(_animator.GetFloat("ForwardMove"), 2f, 0.1f))
                : Mathf.Lerp(_animator.GetFloat("ForwardMove"), 1f, 0.1f)));
    }

    void SetNormalAttack()
    {
        bool isNormalAttack = _playerCtrl.isNormalAttack;
        if (isNormalAttack && !_playerCtrl.isRunning)
        {
            _animator.SetTrigger("NormalAttack");
        }
    }
}