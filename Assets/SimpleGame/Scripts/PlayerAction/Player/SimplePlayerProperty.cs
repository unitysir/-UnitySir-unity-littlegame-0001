using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerProperty
{
    private int hp;

    /// <summary>
    /// 血量
    /// </summary>
    public int Hp
    {
        get => hp < 0 ? 0 : hp;
        set => hp = value;
    }

    private int damage;

    /// <summary>
    /// 伤害
    /// </summary>
    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    private int def;

    /// <summary>
    /// 防御
    /// </summary>
    public int Def
    {
        get => def;
        set => def = value;
    }
}