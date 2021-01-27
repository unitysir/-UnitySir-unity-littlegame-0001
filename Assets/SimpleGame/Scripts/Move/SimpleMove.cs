using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("角色方向控制变量")] [SerializeField] private Vector2 moveDir = Vector2.zero; //移动方向
    [SerializeField] private Vector2 moveDirNormalized = Vector2.zero;
    [SerializeField] private float targetDir = 0f; //角色目标方向
    [SerializeField] private float walkSpeed = 2f; //行走速度
    private float _smoothTime = 0.1f;
    private float _currentVelocity = 0f;
    private float _vMove = 0f;
    private float _hMove = 0f;

    private void Awake()
    {
        _rigidbody = GameObject.Find("MainPlayer").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");
        SetMoveDir();
        SimpleMsgMechanism.SendMsg("playerani", Mathf.Sqrt(_hMove * _hMove + _vMove * _vMove));
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    /// <summary>
    /// 设置朝向
    /// </summary>
    void SetMoveDir()
    {
        moveDir = new Vector2(_hMove, _vMove); //获取到输入的值
        moveDirNormalized = moveDir.normalized; //将值归一化
        targetDir = Mathf.Atan2(moveDirNormalized.x, moveDirNormalized.y) * Mathf.Rad2Deg; //计算朝向目标的角度
        if (moveDirNormalized != Vector2.zero) //当输入的值不为0时，转向目标角度
        {
            _rigidbody.transform.eulerAngles =
                Vector3.up * Mathf.SmoothDampAngle(_rigidbody.transform.eulerAngles.y, targetDir,
                    ref _currentVelocity, _smoothTime);
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    void PlayerMove()
    {
        _rigidbody.velocity = new Vector3(walkSpeed * _hMove, 0, walkSpeed * _vMove);
    }
}