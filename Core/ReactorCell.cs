namespace ReactorOptimizer.Core;

public class ReactorCell
{
    public int X { get; }
    public int Y { get; }
    public ComponentBase? Component { get; set; }

    public ReactorCell(int x, int y)
    {
        X = x;
        Y = y;
        Component = null;
    }
}
