namespace ReactorOptimizer.Core;

public class ReactorHeatVent : ComponentBase, IHeatStorage
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
    
    public int GetHeatPercent() => (int)((float)CurrentHeat / MaxHeat * 100);
    
    /// <summary>
    /// 每 Tick 被调度调用（每秒 20 次）
    /// </summary>
    public override void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        // 每秒吸收堆热 5 点，每 Tick 5 / 20 = 0.25 → 1 Tick 吸收 1 点，每 4 Tick 吸收一次 1（可模拟）
        // 简化为：每 Tick 吸 1，持续 5 次，模拟“吸5散5”效果
        AddHeat(5); // 每秒吸收 5 点热量（写成直接 Tick 吸收，模拟器统一 1 Tick = 1 秒）
    }
    
    public override void Dissipate()
    {
        RemoveHeat(5); // 每秒散发 5 点热量（在模拟器统一调用 Dissipate）
    }

    public override string ToString()
    {
        return $"Heat Vent | 当前热量: {CurrentHeat} / {MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
