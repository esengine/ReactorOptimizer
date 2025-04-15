using System.ComponentModel;
using System.Windows.Media;

namespace ReactorOptimizer.Core;

public class ReactorCellViewModel : INotifyPropertyChanged
{
    public int X { get; }
    public int Y { get; }
    public int LastPulseReceived { get; set; }
    
    private readonly ReactorGridManager _grid;
    public ReactorCellViewModel(int x, int y, ReactorGridManager grid)
    {
        X = x;
        Y = y;
        _grid = grid;
    }

    private ComponentBase? _component;
    public ComponentBase? Component
    {
        get => _component;
        set
        {
            _component = value;
            OnPropertyChanged(nameof(Component));
            OnPropertyChanged(nameof(HeatColor));
            OnPropertyChanged(nameof(HeatText));
            OnPropertyChanged(nameof(ComponentName));
            OnPropertyChanged(nameof(IconPath));
        }
    }

    public string IconPath => Component?.IconPath ?? "";
    public string ComponentName => Component?.DisplayName ?? "空格";
    public string HeatText => Component is IHeatStorage hs ? $"{hs.CurrentHeat} HU" : "";

    public Brush HeatColor
    {
        get
        {
            if (Component is IHeatStorage hs)
            {
                double percent = hs.CurrentHeat / (double)hs.MaxHeat;
                return percent switch
                {
                    < 0.3 => Brushes.LightGreen,
                    < 0.7 => Brushes.Orange,
                    < 0.9 => Brushes.Red,
                    _ => Brushes.DarkRed
                };
            }
            return Brushes.Transparent;
        }
    }
    
    public string EnergyOutputText =>
        Component is FuelRodBase fuel
            ? $"{fuel.CalculateEnergyOutput(_grid.GetPulseAt(X, Y))} EU/t"
            : "";

    public void OnTickVisualUpdate()
    {
        OnPropertyChanged(nameof(HeatText));
        OnPropertyChanged(nameof(HeatColor));
        OnPropertyChanged(nameof(ComponentName));
        OnPropertyChanged(nameof(EnergyOutputText));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
