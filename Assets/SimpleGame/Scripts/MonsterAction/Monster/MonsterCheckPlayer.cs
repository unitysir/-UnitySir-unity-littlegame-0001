using UnityEngine;

/// <summary>
/// 怪物检测玩家脚本
/// </summary>
public class MonsterCheckPlayer : MonoBehaviour
{
    //检测条件：
    //1.需要实时计算与主角的距离
    //2.需要用主角的位置 - 怪物的位置 ，此时怪物才能看向主角

    /// <summary>
    /// 主角实时位置
    /// </summary>
    [Header("主角位置")] [SerializeField] private Vector3 playerPos = Vector3.zero;

    [Header("怪物位置")] [SerializeField] private Vector3 monsterPos = Vector3.zero;

    [Header("主角与怪物的距离")] [SerializeField] private float distance;

    [Header("角色是否进入怪物检测区")] [SerializeField]
    private bool isInCheck;

    private void Awake()
    {
        monsterPos = transform.position;
    }

    private void Start()
    {
        SimpleMsgMechanism.ReceiveMsg("PlayerCurrentPos", objects =>
        {
            //实时获取主角位置
            playerPos = (Vector3) objects[0];
        });
    }

    private void Update()
    {
        //获取怪物的实时位置
        monsterPos = transform.position;
        //如果 distance 小于25 ，则进入检测区
        distance = Vector3.Distance(playerPos, monsterPos);
        isInCheck = distance < 25f;

        //是否进入检测区
        SimpleMsgMechanism.SendMsg("PlayerIsInMonsterCheck", isInCheck, playerPos,monsterPos);
    }
}