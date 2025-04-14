using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactorOptimizer.Core;
using ReactorOptimizer.UI;

namespace ReactorOptimizer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ReactorViewModel ViewModel { get; }
    
    private ReactorGridManager _grid;
    
    public MainWindow()
    {
        InitializeComponent();
        
        _grid = new ReactorGridManager(3); // 默认3仓，9x6
        var simulator = new ReactorSimulator(_grid);
        ViewModel = new ReactorViewModel(_grid, simulator);
        
        DataContext = ViewModel;
    }
    
    private void ComponentItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed &&
            sender is FrameworkElement fe &&
            fe.DataContext is ComponentDefinition def)
        {
            DragDrop.DoDragDrop(fe, def, DragDropEffects.Copy);
        }
    }

    private void Cell_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(ComponentDefinition)))
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }

    private void Cell_Drop(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data.GetData(typeof(ComponentDefinition)) is ComponentDefinition def &&
                ((FrameworkElement)sender).DataContext is ReactorCellViewModel cell)
            {
                var newComponent = def.Factory();
                newComponent.DisplayName = def.Name;

                cell.Component = newComponent;

                _grid.Cells[cell.X, cell.Y].Component = newComponent;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"组件创建失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    
    private void Start_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Start();
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Pause();
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Reset();
    }
}