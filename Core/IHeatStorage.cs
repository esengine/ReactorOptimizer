namespace ReactorOptimizer.Core;

public interface IHeatStorage
{
    int CurrentHeat { get; }
    int MaxHeat { get; }
    bool IsDestroyed { get; }

    void AddHeat(int amount);
    void RemoveHeat(int amount);
}
