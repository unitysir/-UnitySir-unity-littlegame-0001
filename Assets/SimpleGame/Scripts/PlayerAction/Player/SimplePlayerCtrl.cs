using UnityEngine;

public class SimplePlayerCtrl : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("角色方向控制变量")] [SerializeField] private Vector2 moveDir = Vector2.zero; //移动方向
    [SerializeField] private Vector2 moveDirNormalized = Vector2.zero;
    [SerializeField] private float targetDir = 0f; //角色目标方向
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float walkSpeed = 2f; //行走速度
    [SerializeField] private float runSpeed = 6f;
    [Header("是否正在奔跑")][SerializeField] public bool isRunning = false;
    [Header("是否正在进行普通攻击")][SerializeField] public bool isNormalAttack = false;
    [Header("角色是否死亡")] [SerializeField] public bool isDeath;

    private float _smoothTime = 0.1f; //过渡时间
    private float _currentVelocity = 0f;
    private float _vMove = 0f; //垂直输入
    private float _hMove = 0f; //水平输入

    /// <summary>
    /// 方向大小
    /// </summary>
    [HideInInspector] public float dirMag = 0f;

    /// <summary>
    /// 方形转圆形
    /// </summary>
    private Vector2 toRound = Vector2.zero;

    [Header("键盘输入控制")] [Header("奔跑")] [SerializeField]
    private KeyCode keyA = KeyCode.LeftShift;

    [Header("普通攻击")] [SerializeField] private KeyCode keyB = KeyCode.J;
    [SerializeField] private KeyCode keyC = KeyCode.None;
    [SerializeField] private KeyCode keyD = KeyCode.None;

    [Header("角色攻击的武器")] [SerializeField] private GameObject weapon;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (weapon != null)
        {
            weapon.AddComponent<SimplePlayerCalcDamage>();
        }
    }

    private void Update()
    {
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");
        SetMoveDir();
        SetRun();
        toRound = SimpleTools.V2Rect2Round(new Vector2(_hMove, _vMove));
        dirMag = Mathf.Sqrt(toRound.x * toRound.x + toRound.y * toRound.y);
        SimpleMsgMechanism.SendMsg("PlayerMove", dirMag, isRunning);
        SetNormalAttack();
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
        moveDirNormalized = moveDir.normalized; //将值归一化(获取方向)
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
        moveSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 output = SimpleTools.V3Rect2Round(new Vector3(_hMove, 0, _vMove));
        _rigidbody.velocity = new Vector3(moveSpeed * output.x, _rigidbody.velocity.y, moveSpeed * output.z);
    }

    /// <summary>
    /// 设置角色奔跑
    /// </summary>
    void SetRun()
    {
        isRunning = Input.GetKey(keyA);
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    void SetNormalAttack()
    {
        isNormalAttack = Input.GetKeyUp(keyB);
    }
}