namespace ReactorOptimizer.Core;

public class HeatVent : ComponentBase, IHeatStorage
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
    /// 每秒自动从自身释放6点热量
    /// </summary>
    public override void Dissipate()
    {
        RemoveHeat(6);
    }

    public override string ToString()
    {
        return $"散热片 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
