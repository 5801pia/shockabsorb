namespace ShockAbsorber
{
    /// <summary>
    /// Точка в пространстве.
    /// </summary>
    public class Point3D
    {
        #region - Конструкторы -

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Point3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="x">X - координата точки.</param>
        /// <param name="y">Y - координата точки.</param>
        public Point3D(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="x">X - координата точки.</param>
        /// <param name="y">Y - координата точки.</param>
        /// <param name="z">Z - координата точки.</param>
        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion // Конструкторы.

        /// <summary>
        /// X - координата точки.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y - координата точки.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Z - координата точки.
        /// </summary>
        public float Z { get; set; }
    }
}