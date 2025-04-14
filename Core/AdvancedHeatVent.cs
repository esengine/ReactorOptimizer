namespace ReactorOptimizer.Core;

public class AdvancedHeatVent : IHeatStorage
{
    public int MaxHeat => 1000;
    public int CurrentHeat { get; private set; }

    public bool IsDestroyed => CurrentHeat > MaxHeat;

    public void AddHeat(int amount)
    {
        CurrentHeat += amount;
    }

    public void RemoveHeat(int amount)
    {
        CurrentHeat = Math.Max(0, CurrentHeat - amount);
    }

    /// <summary>
    /// 主动散发自身热量（12 HU/s）
    /// </summary>
    public void Dissipate()
    {
        RemoveHeat(12);
    }

    public override string ToString()
    {
        return $"高级散热片 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
