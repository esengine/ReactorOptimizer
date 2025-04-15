namespace ReactorOptimizer.Core;

public static class ComponentLibraryFactory
{
    public static List<ComponentDefinition> AllComponents => new()
    {
        new("反应堆散热片", "pack://application:,,,/Icons/reactor_heat_vent.png", () => new ReactorHeatVent()),
        new("元件散热片", "pack://application:,,,/Icons/component_heat_vent.png", () => new ComponentHeatVent()),
        new("超频散热片", "pack://application:,,,/Icons/overclocked_heat_vent.png", () => new OverclockedHeatVent()),
        new("反应堆热交换器", "pack://application:,,,/Icons/reactor_heat_exchanger.png", () => new ReactorHeatExchanger()),
        new("元件热交换器", "pack://application:,,,/Icons/component_heat_exchanger.png", () => new ComponentHeatExchanger()),
        new("高级热交换器", "pack://application:,,,/Icons/advanced_heat_exchanger.png", () => new AdvancedHeatExchanger()),
        new("热交换器", "pack://application:,,,/Icons/heat_exchanger.png", () => new HeatExchanger()),
        new("散热片", "pack://application:,,,/Icons/heat_vent.png", () => new HeatVent()),
        new("燃料棒（铀）", "pack://application:,,,/Icons/uranium_fuel_rod.png", () => new UraniumFuelRod()),
        new("双联燃料棒（铀）", "pack://application:,,,/Icons/dual_uranium_fuel_rod.png", () => new DualUraniumFuelRod()),
        new("四联燃料棒（铀）", "pack://application:,,,/Icons/quad_uranium_fuel_rod.png", () => new QuadUraniumFuelRod()),
    };
}
