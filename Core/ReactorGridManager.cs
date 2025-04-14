namespace ReactorOptimizer.Core;

using System;
using System.Collections.Generic;

public class ReactorGridManager
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public ReactorCell[,] Cells { get; private set; }
    
    public Dictionary<(int x, int y), int> NeutronPulsesReceived = new();
    public ReactorCore Core { get; } = ReactorCore.Instance;

    public ReactorGridManager(int reactorChambers = 1)
    {
        Width = reactorChambers * 3;
        Height = 6;
        Cells = new ReactorCell[Width, Height];

        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Cells[x, y] = new ReactorCell(x, y);
    }

    public void Tick()
    {
        UpdateNeutronPulseMap();
        
        foreach (var cell in Cells)
        {
            cell.Component?.Tick(Core, GetNeighbors(cell.X, cell.Y));
        }

        // 自散热
        foreach (var cell in Cells)
        {
            cell.Component?.Dissipate();
        }
    }

    private void UpdateNeutronPulseMap()
    {
        NeutronPulsesReceived.Clear();
        
        foreach (var cell in Cells)
        {
            if (cell.Component is INeutronSource source)
            {
                var directions = new List<(int dx, int dy)>
                {
                    (0, -1), (-1, 0), (1, 0), (0, 1)
                };

                foreach (var (dx, dy) in directions)
                {
                    int tx = cell.X + dx;
                    int ty = cell.Y + dy;

                    if (tx >= 0 && tx < Width && ty >= 0 && ty < Height)
                    {
                        var key = (tx, ty);
                        if (!NeutronPulsesReceived.ContainsKey(key))
                            NeutronPulsesReceived[key] = 0;

                        NeutronPulsesReceived[key] += source.NeutronFluxPerDirection;
                    }
                }
            }
        }
    }
    
    public int GetPulseAt(int x, int y)
    {
        return NeutronPulsesReceived.TryGetValue((x, y), out int val) ? val : 0;
    }

    public List<IHeatStorage> GetNeighbors(int x, int y)
    {
        var list = new List<IHeatStorage>();

        var offsets = new List<(int dx, int dy)>
        {
            (0, -1), // 上
            (-1, 0), // 左
            (1, 0),  // 右
            (0, 1)   // 下
        };

        foreach (var (dx, dy) in offsets)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
            {
                var neighbor = Cells[nx, ny].Component;
                if (neighbor is IHeatStorage hs && !hs.IsDestroyed)
                    list.Add(hs);
            }
        }

        return list;
    }
    
    public List<ComponentBase> GetAllAdjacentComponents(int x, int y)
    {
        var list = new List<ComponentBase>();

        var offsets = new List<(int dx, int dy)>
        {
            (0, -1), (-1, 0), (1, 0), (0, 1)
        };

        foreach (var (dx, dy) in offsets)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
            {
                var neighbor = Cells[nx, ny].Component;
                if (neighbor != null)
                    list.Add(neighbor);
            }
        }

        return list;
    }
    
    /// <summary>
    /// 返回从某位置收到的中子束数（用于计算核脉冲）
    /// </summary>
    public int CountNeutronPulses(int x, int y)
    {
        int count = 0;

        // 方向向量 + 反向映射
        var directions = new List<(int dx, int dy)>
        {
            (0, -1), // 上 → neighbor 向下
            (-1, 0), // 左 → neighbor 向右
            (1, 0),  // 右 → neighbor 向左
            (0, 1)   // 下 → neighbor 向上
        };

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
            {
                var neighbor = Cells[nx, ny].Component;

                if (neighbor is INeutronSource neutron)
                {
                    // 简化逻辑：假设所有 INeutronSource 向四周都发射
                    count += neutron.NeutronFluxPerDirection;
                }
            }
        }

        return count;
    }
}
