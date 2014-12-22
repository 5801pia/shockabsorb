using System.Linq;
using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using ShockAbsorber.Interfaces;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber.ModelParts
{
    /// <summary>
    /// Гайка.
    /// </summary>
    public class Screw : IModelPart
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

            bodyLength += circleThickness;

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
                    OperationColor = Color.FromArgb(255, 104, 32)
                };

                sketchProperty.PointsList.Add(new PointF(0, bodyLength + 3.14f));
                sketchProperty.PointsList.Add(new PointF(1.8f, bodyLength + 3.14f));
                sketchProperty.PointsList.Add(new PointF(2.14f, bodyLength + 3));
                sketchProperty.PointsList.Add(new PointF(2.14f, bodyLength + 2.76f));
                sketchProperty.PointsList.Add(new PointF(1.8f, bodyLength + 2.63f));
                sketchProperty.PointsList.Add(new PointF(1.8f, bodyLength + 2.54f));
                sketchProperty.PointsList.Add(new PointF(1.85f, bodyLength + 2.54f));
                sketchProperty.PointsList.Add(new PointF(1.85f, bodyLength + 2.42f));
                sketchProperty.PointsList.Add(new PointF(1.8f, bodyLength + 2.38f));
                sketchProperty.PointsList.Add(new PointF(1.8f, bodyLength + 1.56f));
                sketchProperty.PointsList.Add(new PointF(1.85f, bodyLength + 1.54f));
                sketchProperty.PointsList.Add(new PointF(1.85f, bodyLength + 1.46f));
                sketchProperty.PointsList.Add(new PointF(0.75f, bodyLength + 0.85f));
                sketchProperty.PointsList.Add(new PointF(0, bodyLength + 0.85f));

                sketchProperty.SketchName = "Гайка 1";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.Plane = PlaneType.PlaneXOZ;
                sketchProperty.IsOffsetPlane = true;
                sketchProperty.PlaneDirectionUp = true;
                sketchProperty.OffsetPlaneValue = bodyLength + 3.2f;
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-0.16f, 1.92f));
                sketchProperty.PointsList.Add(new PointF(-0.32f, 2.15f));
                sketchProperty.PointsList.Add(new PointF(0.32f, 2.15f));
                sketchProperty.PointsList.Add(new PointF(0.16f, 1.92f));
                sketchProperty.WithArc = true;
                sketchProperty.ArcPoints.Add(new PointF(0.16f, 1.92f));
                sketchProperty.ArcPoints.Add(new PointF(0, 1.8f));
                sketchProperty.ArcPoints.Add(new PointF(-0.16f, 1.92f));
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.DirectionType = Direction_Type.dtNormal;
                sketchProperty.NormalValue = 1.1f;
                sketchProperty.SketchName = "Гайка 2";
                sketchProperty.CreateNewSketch(part);

                var operation = sketchProperty.OperationsDictionary.Values.Last();
                sketchProperty.CopiesCount = 15;
                sketchProperty.CircularCopy(part, (ksEntity) part.NewEntity((short) Obj3dType.o3d_axisOY),
                                            new List<ksEntity> {operation});
            }
        }
    }
}