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
        SimpleMsgMechanism.ReceiveMsg("PlayerMove", objects =>
        {
            float move = (float) objects[0];
            bool isRun = (bool) objects[1];
            _animator.SetFloat("ForwardMove",
                move * (isRun
                    ? (Mathf.Lerp(_animator.GetFloat("ForwardMove"), 2f, 0.1f))
                    : Mathf.Lerp(_animator.GetFloat("ForwardMove"), 1f, 0.1f)));
        });
    }
}