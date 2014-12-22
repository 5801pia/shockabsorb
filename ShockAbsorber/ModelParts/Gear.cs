using System;
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
    /// Шестеренка.
    /// </summary>
    public class Gear : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var springLength = parameters[Parameter.GearPosition].Value;
            var carvingDiameter = parameters[Parameter.CarvingDiameter].Value / 4 - 1;

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

                sketchProperty.PointsList.Add(new PointF(0, -20.15f + springLength));
                sketchProperty.PointsList.Add(new PointF(3.65f + carvingDiameter, -20.15f + springLength));
                sketchProperty.PointsList.Add(new PointF(3.65f + carvingDiameter, -20.55f + springLength));
                sketchProperty.PointsList.Add(new PointF(3.36f + carvingDiameter, -20.55f + springLength));
                sketchProperty.PointsList.Add(new PointF(3.36f + carvingDiameter, -21 + springLength));
                sketchProperty.PointsList.Add(new PointF(0, -21 + springLength));

                sketchProperty.SketchName = "Шестеренка 1";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.Plane = PlaneType.PlaneXOZ;
                sketchProperty.IsOffsetPlane = true;
                sketchProperty.OffsetPlaneValue = Math.Abs(-21 + springLength);
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-0.35f, -3.4f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(0.35f, -3.4f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(0.35f, -3.12f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(-0.35f, -3.12f - carvingDiameter));
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.DirectionType = Direction_Type.dtReverse;
                sketchProperty.ReverseValue = 0.45f;
                sketchProperty.SketchName = "Шестеренка 2";
                sketchProperty.CreateNewSketch(part);

                var operation = sketchProperty.OperationsDictionary.Values.Last();
                sketchProperty.CopiesCount = 7;
                sketchProperty.CircularCopy(part, (ksEntity)part.NewEntity((short)Obj3dType.o3d_axisOY),
                                            new List<ksEntity> { operation });

                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-0.3f, -3.65f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(0.3f, -3.65f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(0.3f, -3.43f - carvingDiameter));
                sketchProperty.PointsList.Add(new PointF(-0.3f, -3.43f - carvingDiameter));
                sketchProperty.Operation = OperationType.CutExtrusion;
                sketchProperty.DirectionType = Direction_Type.dtReverse;
                sketchProperty.ReverseValue = 0.65f;
                sketchProperty.SketchName = "Шестеренка 3";
                sketchProperty.CreateNewSketch(part);

                operation = sketchProperty.OperationsDictionary.Values.Last();
                sketchProperty.CopiesCount = 8;
                sketchProperty.CircularCopy(part, (ksEntity)part.NewEntity((short)Obj3dType.o3d_axisOY),
                                            new List<ksEntity> { operation });
            }
        }
    }
}