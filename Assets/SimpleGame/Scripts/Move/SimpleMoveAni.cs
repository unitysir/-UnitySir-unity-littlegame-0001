using UnityEngine;

public class SimpleMoveAni : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GameObject.Find("MainPlayer/Player").GetComponent<Animator>();
    }

    void Update()
    {
        SetMoveAni();
    }

    /// <summary>
    /// 设置移动动画
    /// </summary>
    void SetMoveAni()
    {
        SimpleMsgMechanism.Instance.ReceiveMsg("playerani", msg =>
        {
            float ani = (float) msg;
            _animator.SetFloat("ForwardMove", ani);
        });
    }
}