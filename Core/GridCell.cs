namespace ReactorOptimizer.Core;

public class GridCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public IHeatStorage? Component { get; set; }
}