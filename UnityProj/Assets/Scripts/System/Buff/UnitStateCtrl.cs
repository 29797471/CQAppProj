using System;

public interface UnitStateCtrl
{
    UInt64 GetId();

    /// <summary>
    /// 应用于那些只由BUFF变更的属性,战斗过程中属性不会发生变化.(防御),
    /// Buff过程中只更新由基础值到最终值的计算公式,实际属性的变更发生在外部获取(战斗计算)
    /// 加成效果可以是临时的,这些属性加成在Buff退出时取消
    /// </summary>
    void Set(uint unitType, uint attrKey, Func<float, float> GetVal);

    /// <summary>
    /// 应用于那些不只由Buff变更的属性,战斗过程中属性也会发生变化.(生命值,能量值)
    /// Buff过程中直接获取/修改属性,加成效果是永久的, 这些加成或者变化不应该在Buff退出时取消
    /// </summary>
    void Set(uint unitType, uint attrKey, float attrValue);
    float Get(uint unitType, uint attrKey);

    /// <summary>
    /// 当战斗计算时属性更新的回调
    /// </summary>
    event Action<int, int, float> OnSetAttritube;

    uint CurrentState { get; set; }

    Action SetEffect(string resEff, uint bindPoint);

    bool IsUnitDead();

    event Action OnDestroyAction;
}