namespace ReactorOptimizer.Core;

public class ComponentHeatExchanger : ComponentBase, IHeatStorage
{
    public int MaxHeat => 5000;
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
    /// 每秒与周围组件进行热交换（每个方向最多36点，根据热差决定方向）
    /// </summary>
    public override void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        foreach (var neighbor in neighbors)
        {
            if (neighbor == null || neighbor == this || neighbor is ReactorCore) continue;

            int diff = this.CurrentHeat - neighbor.CurrentHeat;

            if (Math.Abs(diff) >= 2)
            {
                int transfer = Math.Min(36, Math.Abs(diff) / 2);

                if (diff > 0)
                {
                    RemoveHeat(transfer);
                    neighbor.AddHeat(transfer);
                }
                else
                {
                    AddHeat(transfer);
                    neighbor.RemoveHeat(transfer);
                }
            }
        }
    }

    public override string ToString()
    {
        return $"元件热交换器 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
