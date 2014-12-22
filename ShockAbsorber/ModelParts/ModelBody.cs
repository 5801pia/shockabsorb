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
    /// Тело амортизатора.
    /// </summary>
    public class ModelBody : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var bodyLength = parameters[Parameter.BodyLength].Value - 25;
            var rodDiameter = parameters[Parameter.RodDiameter].Value;
            var bodyDiameter = parameters[Parameter.BodyDiameter].Value/2 - 3;
            var circleThickness = parameters[Parameter.CircleThickness].Value - 0.1f;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new KompasSketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = 360,
                    UseCenterLine = true,
                    CenterLineStartPos = new PointF(0, 0),
                    CenterLineEndPos = new PointF(0, 1),
                    Operation = OperationType.BaseRotated,
                    DirectionType = Direction_Type.dtNormal,
                    OperationColor = Color.FromArgb(192, 192, 192)
                };

                // Тело амортизатора 1.
                sketchProperty.PointsList.Add(new PointF(0, 4.68f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.32f, 4.68f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.56f, 4.35f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.56f, 1.36f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.1f, 1.36f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.1f, 1.025f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.1f, 1.025f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(2.25f, 1 + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(2.4f + bodyDiameter, 1 + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(4 + bodyDiameter, 0.22f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(4 + bodyDiameter, 0.02f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(4.15f + bodyDiameter, 0.02f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(4.15f + bodyDiameter, -0.1f + bodyLength));
                sketchProperty.PointsList.Add(new PointF(rodDiameter, -0.1f + bodyLength));
                sketchProperty.PointsList.Add(new PointF(rodDiameter, -21.8f));
                sketchProperty.PointsList.Add(new PointF(2.78f + bodyDiameter, -21.8f));
                sketchProperty.PointsList.Add(new PointF(3 + bodyDiameter, -22.25f));
                sketchProperty.PointsList.Add(new PointF(3 + bodyDiameter, -23f));
                sketchProperty.PointsList.Add(new PointF(3.2f + bodyDiameter, -23.5f));
                sketchProperty.PointsList.Add(new PointF(3.2f + bodyDiameter, -24.75f));
                sketchProperty.PointsList.Add(new PointF(3 + bodyDiameter, -25));
                sketchProperty.PointsList.Add(new PointF(0, -25));
                sketchProperty.SketchName = "Тело амортизатора 1";
                sketchProperty.CreateNewSketch(part);

                // Тело амортизатора 2.
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(0, 1.4f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.26f, 1.4f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(1.26f, 1.14f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(2.26f, 1.025f + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(2.26f, 1 + bodyLength + circleThickness));
                sketchProperty.PointsList.Add(new PointF(0, 1 + bodyLength + circleThickness));
                sketchProperty.OperationColor = Color.FromArgb(40, 40, 40);
                sketchProperty.SketchName = "Тело амортизатора 2";
                sketchProperty.CreateNewSketch(part);

                // Тело амортизатора 3.
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-1.78f - bodyDiameter, -28));
                sketchProperty.PointsList.Add(new PointF(-3.2f - bodyDiameter, -24.75f));
                sketchProperty.PointsList.Add(new PointF(3.2f + bodyDiameter, -24.75f));
                sketchProperty.PointsList.Add(new PointF(1.78f + bodyDiameter, -28f));
                sketchProperty.WithArc = true;
                sketchProperty.ArcPoints.Add(new PointF(1.78f + bodyDiameter, -28));
                sketchProperty.ArcPoints.Add(new PointF(0, -30));
                sketchProperty.ArcPoints.Add(new PointF(-1.78f - bodyDiameter, -28));
                sketchProperty.Operation = OperationType.BaseExtrusion;
                sketchProperty.DirectionType = Direction_Type.dtBoth;
                sketchProperty.NormalValue = 1.1f;
                sketchProperty.ReverseValue = 1.1f;
                sketchProperty.OperationColor = Color.FromArgb(192, 192, 192);
                sketchProperty.SketchName = "Тело амортизатора 3";
                sketchProperty.CreateNewSketch(part);

                // Тело амортизатора 4.
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-4 - bodyDiameter, -22));
                sketchProperty.PointsList.Add(new PointF(-3 - bodyDiameter, -22));
                sketchProperty.PointsList.Add(new PointF(-3 - bodyDiameter, -26));
                sketchProperty.PointsList.Add(new PointF(-4 - bodyDiameter, -26));
                sketchProperty.AddBreakPoint();
                sketchProperty.PointsList.Add(new PointF(3 + bodyDiameter, -22));
                sketchProperty.PointsList.Add(new PointF(4 + bodyDiameter, -22));
                sketchProperty.PointsList.Add(new PointF(4 + bodyDiameter, -26));
                sketchProperty.PointsList.Add(new PointF(3 + bodyDiameter, -26));
                sketchProperty.WithArc = false;
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.DirectionType = Direction_Type.dtBoth;
                sketchProperty.NormalValue = 2;
                sketchProperty.ReverseValue = 2;
                sketchProperty.SketchName = "Тело амортизатора 4";
                sketchProperty.CreateNewSketch(part);

                // Тело амортизатора 5.
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(0, 5.1f + bodyLength + circleThickness));
                sketchProperty.Shape = ShapeType.Circle;
                sketchProperty.Radius = 1.7f;
                sketchProperty.SketchName = "Тело амортизатора 5";
                sketchProperty.CreateNewSketch(part);

                // Тело амортизатора 6.
                sketchProperty.Operation = OperationType.BaseExtrusion;
                sketchProperty.NormalValue = 0.7f;
                sketchProperty.ReverseValue = 0.7f;
                sketchProperty.SketchName = "Тело амортизатора 6";
                sketchProperty.CreateNewSketch(part);
            }
        }
    }
}