using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using ReactorOptimizer.Core;

namespace ReactorOptimizer.UI;

public class ReactorViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ReactorCellViewModel> ReactorCells { get; }
    public ObservableCollection<ComponentDefinition> ComponentLibrary { get; }

    private readonly ReactorGridManager _grid;
    private readonly ReactorSimulator _simulator;
    public ReactorViewModel(ReactorGridManager grid, ReactorSimulator simulator)
    {
        _grid = grid;
        _simulator = simulator;
        _simulator.TickUpdated += () =>
        {
            OnPropertyChanged(nameof(ReactorTemperature));
            OnPropertyChanged(nameof(TotalEUText));
            
            foreach (var cellVM in ReactorCells)
            {
                cellVM.OnTickVisualUpdate();
            }
        };

        ReactorCells = new ObservableCollection<ReactorCellViewModel>();
        for (int y = 0; y < _grid.Height; y++)
        for (int x = 0; x < _grid.Width; x++)
            ReactorCells.Add(new ReactorCellViewModel(x, y, _grid));

        ComponentLibrary = new ObservableCollection<ComponentDefinition>(ComponentLibraryFactory.AllComponents);
    }

    public string ReactorTemperature =>
        $"堆温: {_simulator.Core.Temperature} / {ReactorCore.MaxTemperature} HU";
    
    public string TotalEUText => $"总输出: {CalculateTotalEU()} EU/t";

    private int CalculateTotalEU()
    {
        int total = 0;

        foreach (var cell in ReactorCells)
        {
            if (cell.Component is FuelRodBase fuel)
            {
                int pulses = _grid.GetPulseAt(cell.X, cell.Y);
                total += fuel.CalculateEnergyOutput(pulses);
            }
        }

        return total;
    }
    
    public void Start() => _simulator.Start();
    public void Pause() => _simulator.Pause();
    public void Reset() => _simulator.Reset();

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
