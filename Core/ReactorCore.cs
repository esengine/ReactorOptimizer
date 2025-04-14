namespace ReactorOptimizer.Core;

public class ReactorCore
{
    public int Temperature { get; private set; }
    public const int MaxTemperature = 10000;

    public void AddHeat(int amount)
    {
        Temperature += amount;
    }

    public void RemoveHeat(int amount)
    {
        Temperature = Math.Max(0, Temperature - amount);
    }
    
    public void Reset()
    {
        Temperature = 0;
    }
    
    public bool IsExploded => Temperature >= MaxTemperature;

    public override string ToString()
    {
        return $"堆温: {Temperature} / {MaxTemperature} | {(IsExploded ? "💥 爆炸!" : "🟢 正常")}";
    }
    
    public static ReactorCore Instance { get; } = new ReactorCore();
    
    
}
