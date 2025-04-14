namespace ReactorOptimizer.Core;

public class ComponentHeatVent : ComponentBase, IHeatStorage
{
    public int CurrentHeat => 0;
    public int MaxHeat => 0;
    public bool IsDestroyed => false;
    public string Name => "元件散热片";
    
    public void AddHeat(int amount)
    {
        // 不接受热量，忽略
    }
    
    public void RemoveHeat(int amount)
    {
        // 不储热，也不减少热量
    }

    /// <summary>
    /// 被动散热逻辑：为周围 4 个组件各减 1 点热量（总最多 4 点）
    /// </summary>
    public override void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        int count = 0;
        foreach (var neighbor in neighbors)
        {
            if (count >= 4) break;

            neighbor.RemoveHeat(1);
            count++;
        }
    }

    public override string ToString()
    {
        return $"{Name} | 被动为周围元件散热";
    }
}
