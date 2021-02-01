using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerate : MonoBehaviour
{
    [Header("生成怪物的最大数量")] [SerializeField] private int monsterNumMax = 12;
    [Header("当前怪物的数量")] [SerializeField] private int monsterCurrNum = 0;

    [Header("加载出的怪物对象")] [SerializeField] private GameObject monsterPrefabObj;
    [Header("怪物列表")] [SerializeField] private List<GameObject> monsterList = new List<GameObject>();

    private void Start()
    {
        //加载怪物对象
        monsterPrefabObj = Resources.Load<GameObject>("Prefabs/Models/Monster1");
        //启用协程
        StartCoroutine(GenerateMonster());
    }

    /// <summary>
    /// 利用协程生成怪物
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateMonster()
    {
        while (true)
        {
            if (monsterCurrNum < monsterNumMax)
            {
                //随机生成一个本地坐标
                var localPos = new Vector3(GetRndF(), GetRndF(), GetRndF());
                //将本地坐标转为世界坐标,TransformPoint()会根据当前对象的坐标和缩放来转换
                var pos = transform.TransformPoint(localPos);
                //获取随机的方向
                var rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                //实例化一个怪物对象
                var monsterInstance = Instantiate(monsterPrefabObj, pos, rotation, null);

                monsterList.Add(monsterInstance);
                monsterCurrNum++;

                yield return new WaitForSeconds(5f); //每隔五秒
            }

            yield return null;
        }
    }

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <returns></returns>
    private float GetRndF()
    {
        return Random.Range(-0.5f, 0.5f);
    }
}