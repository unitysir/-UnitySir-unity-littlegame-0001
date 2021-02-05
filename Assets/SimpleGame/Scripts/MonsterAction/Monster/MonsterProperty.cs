/// <summary>
/// 怪物的一些属性
/// </summary>
public class MonsterProperty
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
}