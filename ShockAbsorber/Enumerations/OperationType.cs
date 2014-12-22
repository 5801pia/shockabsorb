namespace ShockAbsorber.Enumerations
{
    /// <summary>
    /// Операция.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// Вырезать выдавливанием.
        /// </summary>
        CutExtrusion,

        /// <summary>
        /// Базовая операция вращения.
        /// </summary>
        BaseRotated,

        /// <summary>
        /// Базовая операция выдавливания.
        /// </summary>
        BaseExtrusion,

        /// <summary>
        /// Кинематическая операция.
        /// </summary>
        BaseEvolution
    }
}