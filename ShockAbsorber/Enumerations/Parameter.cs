using System.ComponentModel;

namespace ShockAbsorber.Enumerations
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    public enum Parameter
    {
        [Description("1. ����� ������")]
        BodyLength,

        [Description("2. ������� ������")]
        BodyDiameter,

        [Description("3. ������� �������")]
        RodDiameter,

        [Description("4. ����� ������")]
        CarvingLength,

        [Description("5. ������� ������")]
        CarvingDiameter,

        [Description("6. ����������� �����������")]
        ScrewDirection,

        [Description("7. ������� �������")]
        SpringDiameter,

        [Description("8. ������� ��������� 1")]
        FirstHoleDiameter,

        [Description("9. ������� ��������� 2")]
        SecondHoleDiameter,

        [Description("10. ������� ����������")]
        CircleThickness,

        [Description("11. ��������� �����")]
        GearPosition,
    }
}