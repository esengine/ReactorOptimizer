namespace ReactorOptimizer.Core;

public abstract class FuelRodBase : ComponentBase, INeutronSource
{
    public abstract string Name { get; }
    public abstract int BaseDurationTicks { get; }
    public abstract int NeutronFluxPerDirection { get; }

    public abstract int CalculateEnergyOutput(int externalNeutronFlux);
    public abstract int CalculateHeatPerSecond(int adjacentReactors);
    
    protected void DistributeHeat(ReactorCore core, List<IHeatStorage> neighbors, int totalHeat)
    {
        var targets = neighbors.Where(n => !n.IsDestroyed).ToList();

        if (targets.Count > 0)
        {
            int share = totalHeat / targets.Count;
            int remainder = totalHeat % targets.Count;

            for (int i = 0; i < targets.Count; i++)
                targets[i].AddHeat(share + (i == 0 ? remainder : 0));
        }
        else
        {
            core.AddHeat(totalHeat);
        }
    }
    
    /// <summary>
    /// 计算当前 Tick 的 EU 输出（按核脉冲数模拟）
    /// </summary>
    public virtual int CalculateEUPerTick(int pulseCount)
    {
        return CalculateEnergyOutput(pulseCount);
    }
}
