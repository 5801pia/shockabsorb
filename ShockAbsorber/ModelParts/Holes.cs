using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using ShockAbsorber.Interfaces;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber.ModelParts
{
    /// <summary>
    /// Отверстия.
    /// </summary>
    public class Holes : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var bodyLength = parameters[Parameter.BodyLength].Value - 25;
            var circleThickness = parameters[Parameter.CircleThickness].Value - 0.1f;
            var firstHoleDiameter = parameters[Parameter.FirstHoleDiameter].Value;
            var secondHoleDiameter = parameters[Parameter.SecondHoleDiameter].Value;

            bodyLength += circleThickness;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Circle,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = 2.2f,
                    ReverseValue = 2.2f,
                    Radius = firstHoleDiameter,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtBoth,
                    OperationColor = Color.FromArgb(192, 192, 192)
                };

                sketchProperty.PointsList.Add(new PointF(0, -26.8f));

                sketchProperty.SketchName = "Отверстие 1";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.Chamfer(part, firstHoleDiameter, -26.8f, 2.2f, 0.2f);
                sketchProperty.Chamfer(part, firstHoleDiameter, -26.8f, -2.2f, 0.2f);

                sketchProperty.Radius = firstHoleDiameter - 0.55f;
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.CreateNewSketch(part);

                sketchProperty.NormalValue = 1.4f;
                sketchProperty.ReverseValue = 1.4f;
                sketchProperty.PointsList.Clear();
                sketchProperty.Radius = secondHoleDiameter;
                sketchProperty.Operation = OperationType.BaseExtrusion;
                sketchProperty.PointsList.Add(new PointF(0, bodyLength + 5.1f));
                sketchProperty.SketchName = "Отверстие 2";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.Chamfer(part, secondHoleDiameter, bodyLength + 5.1f, 1.4f, 0.2f);
                sketchProperty.Chamfer(part, secondHoleDiameter, bodyLength + 5.1f, -1.4f, 0.2f);

                sketchProperty.Radius = secondHoleDiameter - 0.55f;
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.CreateNewSketch(part);
            }
        }
    }
}