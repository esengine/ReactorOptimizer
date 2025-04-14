namespace ReactorOptimizer.Core;

public abstract class ComponentBase
{
    public string DisplayName { get; set; } = "";
    
    public virtual void Tick(ReactorCore core, List<IHeatStorage> neighbors)
    {
        // 热交换器/吸热器等可覆盖
    }

    public virtual void Dissipate()
    {
        // 散热器可覆盖
    }
    
    public override string ToString() => DisplayName;
}
