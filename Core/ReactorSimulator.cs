using System.Windows;

namespace ReactorOptimizer.Core;

using System.Windows.Threading;

public class ReactorSimulator
{
    private readonly ReactorGridManager _grid;
    private readonly DispatcherTimer _timer;

    public bool IsRunning => _timer.IsEnabled;
    public ReactorCore Core => _grid.Core;

    public event Action? TickUpdated;

    public ReactorSimulator(ReactorGridManager grid)
    {
        _grid = grid;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50) // 20 tick/s
        };
        _timer.Tick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_grid.Core.IsExploded)
        {
            _timer.Stop();
            TickUpdated?.Invoke();
            MessageBox.Show("💥 反应堆爆炸！温度超过 10,000 HU", "爆炸警告", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _grid.Tick();
        TickUpdated?.Invoke(); // 通知 UI 刷新
    }

    public void Start() => _timer.Start();
    public void Pause() => _timer.Stop();
    public void Reset()
    {
        _timer.Stop();
        _grid.Core.Reset(); // 你可以添加 Reset 方法清零温度
        // 重置所有组件热量
        foreach (var cell in _grid.Cells)
        {
            if (cell.Component is IHeatStorage hs)
                hs.RemoveHeat(hs.CurrentHeat); // 清零热量
        }
        TickUpdated?.Invoke();
    }
}
