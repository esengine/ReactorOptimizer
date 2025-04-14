namespace ReactorOptimizer.Core;

public class OverclockedHeatVent : ComponentBase, IHeatStorage
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
    /// 在 Tick 中吸收堆热（堆温 > 0 时吸收 36 HU）
    /// </summary>
    public override void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        if (core.Temperature > 0)
        {
            AddHeat(36);
        }
    }

    /// <summary>
    /// 每秒自动从自身散发 20 HU
    /// </summary>
    public override void Dissipate()
    {
        RemoveHeat(20);
    }

    public override string ToString()
    {
        return $"超频散热片 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
