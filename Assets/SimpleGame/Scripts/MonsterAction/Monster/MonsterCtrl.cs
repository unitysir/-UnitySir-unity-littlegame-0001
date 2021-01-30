using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    [SerializeField] private CharacterController monsterController;

    [Header("是否进入怪物检测区")] [SerializeField] private bool isInCheck;

    [Header("怪物的出生点位置")] [SerializeField] private Vector3 monsterBronPointPos = Vector3.zero;

    [Header("怪物的位置")] [SerializeField] private Vector3 monsterPos = Vector3.zero;

    [Header("怪物是否移动")] [SerializeField] private bool isMove;
    [Header("怪物是否开始攻击")] [SerializeField] private bool isAttack;
    [Header("怪物是否死亡")] [SerializeField] private bool isDeath;

    [Header("主角当前的位置")] [SerializeField] private Vector3 playerPos = Vector3.zero;

    //怪物控制必须要有以下几个条件 :
    //怪物需要实时计算与主角的距离：
    //1.主角进入了怪物的检测区 (范围 = (25))
    //此时怪物会向主角移动，播放移动动画
    //2.怪物的攻击区 (范围 = (1.8))
    //当角色进入怪物的攻击区，怪物才会攻击，此时播放攻击动画
    //3.怪兽死亡
    //当怪物血量低于0时，播放死亡动画
    //4.主角退出了怪物的检测区 (范围 = (25))
    //此时怪物会回到生成区
    private void Awake()
    {
        monsterController = GetComponent<CharacterController>();
        monsterPos = transform.position;
    }

    private void Start()
    {
        monsterBronPointPos = transform.position; //获取出生点
        SimpleMsgMechanism.ReceiveMsg("PlayerIsInMonsterCheck", objects =>
        {
            isInCheck = (bool) objects[0]; //是否进入检测区
            playerPos = (Vector3) objects[1]; //主角实时位置
            monsterPos = (Vector3) objects[2]; //怪物的实时位置

            Debug.DrawLine(playerPos, monsterBronPointPos);
        });
    }

    private void Update()
    {
        if (isInCheck)
        {
            //如果角色进入检测区,怪物实时看着主角并向主角移动
            //实时看着主角
            transform.LookAt(playerPos);
            //获取主角与怪物之间的距离
            var distance = Vector3.Distance(playerPos, monsterPos);
            //获取主角和怪物之间的偏移
            var offset = (playerPos - monsterPos);
            //如果怪物没有接触地面
            if (!monsterController.isGrounded)
            {
                //应用重力。
                offset.y -= 20f * Time.deltaTime;
            }

            if (distance > 1.8f)
            {
                //当主角和怪物之间的的距离大于 0.1 时 ，怪物就向主角移动
                //获取到目标方向
                offset = offset.normalized * 1.2f;
                //移动到目标点
                monsterController.Move(offset * Time.deltaTime);
                isMove = true; //移动
            }
            else
            {
                //当检车到主角时，如果和主角的距离小于等于2，则开始攻击
                isMove = false;
            }
        }
        else
        {
            //如果主角退出检测区,怪物就回到出生点
            isAttack = false;
            //怪物要实时看着出生点
            transform.LookAt(monsterBronPointPos);
            //获取出生点与怪物之间的距离
            var distance = Vector3.Distance(monsterBronPointPos, monsterPos);
            //获取出生点和怪物之间的偏移
            var offset = (monsterBronPointPos - monsterPos);

            if (!monsterController.isGrounded)
            {
                //应用重力。
                offset.y -= 20f * Time.deltaTime;
            }

            if (distance > 1f)
            {
                //获取到目标方向
                offset = offset.normalized * 1.5f;
                //移动到目标点
                monsterController.Move(offset * Time.deltaTime);
                isMove = true; //移动
            }
            else
            {
                isMove = false;
            }
        }

        //发送当前怪物的移动状态，从而播放对应的动画
        SimpleMsgMechanism.SendMsg("MonsterIsMove", isMove);
    }
}