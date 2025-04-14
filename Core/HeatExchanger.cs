namespace ReactorOptimizer.Core;

public class HeatExchanger : ComponentBase, IHeatStorage
{
    public int MaxHeat => 2500;
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
    /// 每 Tick 与堆交换最多 4 点热量，与邻居双向交换最多 12 点热量
    /// </summary>
    public override void Tick(ReactorCore reactor, List<IHeatStorage> neighbors)
    {
        const int reactorDelta = 4;

        // 与堆热交换（最多 4）
        if (reactor.Temperature > CurrentHeat)
        {
            AddHeat(reactorDelta);
            reactor.RemoveHeat(reactorDelta);
        }
        else
        {
            RemoveHeat(reactorDelta);
            reactor.AddHeat(reactorDelta);
        }

        // 与邻居热交换（最多 12 / 每个组件）
        foreach (var neighbor in neighbors)
        {
            if (neighbor == null || neighbor == this || neighbor is ReactorCore) continue;

            int diff = this.CurrentHeat - neighbor.CurrentHeat;

            if (Math.Abs(diff) >= 2)
            {
                int transfer = Math.Min(12, Math.Abs(diff) / 2);

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
        return $"热交换器 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
