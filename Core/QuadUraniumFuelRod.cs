namespace ReactorOptimizer.Core;

public class QuadUraniumFuelRod : FuelRodBase
{
    public override string Name => "四联燃料棒（铀）";
    public override int BaseDurationTicks => 400000;
    public override int NeutronFluxPerDirection => 4;
    
    private int _ticksRemaining = 20000;
    private double _heatBuffer = 0;

    public override int CalculateEnergyOutput(int externalNeutronFlux)
    {
        return 5 * (12 + externalNeutronFlux);
    }

    public override int CalculateHeatPerSecond(int adjacentReactors)
    {
        int n = adjacentReactors;
        return 8 * (n + 3) * (n + 4);
    }
    
    public override void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        int adjacent = neighbors.Count(n =>
            n is FuelRodBase || n.GetType().Name.Contains("Reflector", StringComparison.OrdinalIgnoreCase));

        _heatBuffer += CalculateHeatPerSecond(adjacent) / 20.0;

        int heatToApply = (int)Math.Floor(_heatBuffer);
        _heatBuffer -= heatToApply;

        if (heatToApply > 0)
            DistributeHeat(core, neighbors, heatToApply);

        _ticksRemaining = Math.Max(0, _ticksRemaining - 1);
    }

    public override void Dissipate() { }

    public override string ToString()
    {
        return $"{Name} | 剩余Tick: {_ticksRemaining} / {BaseDurationTicks}";
    }
}
