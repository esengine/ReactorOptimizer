namespace ReactorOptimizer.Core;

public class ComponentDefinition
{
    public string Name { get; set; }
    public string IconPath { get; set; } // 图标路径，用于绑定 Image
    public Func<ComponentBase> Factory { get; set; } // 创建实例的方法

    public ComponentDefinition(string name, string iconPath, Func<ComponentBase> factory)
    {
        Name = name;
        IconPath = iconPath;
        Factory = factory;
    }
}
