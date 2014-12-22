using System.ComponentModel;

namespace ShockAbsorber.Enumerations
{
    /// <summary>
    /// Параметры модели.
    /// </summary>
    public enum Parameter
    {
        [Description("1. Длина стойки")]
        BodyLength,

        [Description("2. Диаметр стойки")]
        BodyDiameter,

        [Description("3. Диаметр стержня")]
        RodDiameter,

        [Description("4. Длина резьбы")]
        CarvingLength,

        [Description("5. Диаметр резьбы")]
        CarvingDiameter,

        [Description("6. Направление скручивания")]
        ScrewDirection,

        [Description("7. Диаметр пружины")]
        SpringDiameter,

        [Description("8. Диаметр отверстия 1")]
        FirstHoleDiameter,

        [Description("9. Диаметр отверстия 2")]
        SecondHoleDiameter,

        [Description("10. Толщина окружности")]
        CircleThickness,

        [Description("11. Положение гайки")]
        GearPosition,
    }
}