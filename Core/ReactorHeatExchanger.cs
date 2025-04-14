namespace ReactorOptimizer.Core;

public class ReactorHeatExchanger : ComponentBase, IHeatStorage
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
    /// 每秒与反应堆本体交换最多 72 HU（视堆温或组件温差而定）
    /// 方向策略：当反应堆温度高时吸热，否则排热
    /// </summary>
    public override void Tick(ReactorCore reactor, List<IHeatStorage> neighbors)
    {
        const int delta = 72;

        if (reactor.Temperature > CurrentHeat)
        {
            // 从堆吸热
            AddHeat(delta);
            reactor.RemoveHeat(delta);
        }
        else
        {
            // 向堆释放热量
            RemoveHeat(delta);
            reactor.AddHeat(delta);
        }
    }

    public override string ToString()
    {
        return $"反应堆热交换器 | 当前热量: {CurrentHeat}/{MaxHeat} | 状态: {(IsDestroyed ? "失效" : "正常")}";
    }
}
