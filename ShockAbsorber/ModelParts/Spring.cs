using System;
using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using ShockAbsorber.Interfaces;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber.ModelParts
{
    /// <summary>
    /// Пружина.
    /// </summary>
    public class Spring : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var bodyLength = parameters[Parameter.BodyLength].Value - 25;
            var carvingLength = parameters[Parameter.CarvingLength].Value;
            var bodyLength2 = parameters[Parameter.BodyLength].Value;
            var springDiameter = parameters[Parameter.SpringDiameter].Value;
            var screwDirection = (int) parameters[Parameter.ScrewDirection].Value;
            var gearLength = parameters[Parameter.GearPosition].Value;
            var carvingDiameter = parameters[Parameter.CarvingDiameter].Value / 4 - 1;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Circle,
                    Radius = springDiameter,
                    Plane = PlaneType.PlaneYOZ,
                    Operation = OperationType.BaseEvolution,
                    OperationColor = Color.FromArgb(255, 104, 32)
                };

                sketchProperty.PointsList.Add(new PointF(-3.44f - carvingDiameter, 25 - gearLength - 5 - 0.4f));

                float heightStep = -(25 - gearLength - 5 - 0.4f);
                int angleStep = screwDirection == 0 ? 20 : 110;

                sketchProperty.Spline3DPoints.Add(new Point3D(0, -(25 - gearLength - 5 - 0.4f), 3.44f + carvingDiameter));

                while (heightStep < bodyLength - springDiameter - 0.4f)
                {
                    var point = GetPoint(new PointF(0, 0), angleStep, 4.2f + carvingDiameter);

                    sketchProperty.Spline3DPoints.Add(screwDirection == 0
                                                          ? new Point3D(point.Y, heightStep, point.X)
                                                          : new Point3D(point.X, heightStep, point.Y));

                    angleStep += 60;

                    // Создаем завиток в начале (снизу).
                    if (angleStep < 250) continue;

                    heightStep += 0.65f;
                }

                // Создаем завиток в конце (сверху).
                for (int i = 0; i < 5; i++)
                {
                    var point = GetPoint(new PointF(0, 0), angleStep, 4f + carvingDiameter);

                    sketchProperty.Spline3DPoints.Add(screwDirection == 0
                                                          ? new Point3D(point.Y, heightStep, point.X)
                                                          : new Point3D(point.X, heightStep, point.Y));

                    angleStep += 50;
                }

                var lastPoint = GetPoint(new PointF(0, 0), angleStep + 5, 3.65f + carvingDiameter);
                sketchProperty.Spline3DPoints.Add(screwDirection == 0
                                                          ? new Point3D(lastPoint.Y, heightStep, lastPoint.X)
                                                          : new Point3D(lastPoint.X, heightStep, lastPoint.Y));

                sketchProperty.SketchName = "Пружина";
                sketchProperty.CreateNewSketch(part);
            }
        }

        /// <summary>
        /// Возвращает координату точки на окружности.
        /// </summary>
        /// <param name="center">Координата центра окружности.</param>
        /// <param name="angel">Угол.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <returns>Координата точки на окружности.</returns>
        public static PointF GetPoint(PointF center, double angel, double radius)
        {
            if (angel > 360)
                angel -= ((int)(angel / 360)) * 360;

            var x = (float)(Math.Cos((angel) * Math.PI / 180) * radius + center.X);
            var y = (float)(Math.Sin((angel) * Math.PI / 180) * radius + center.Y);

            return new PointF(x, y);
        }
    }
}