using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using ShockAbsorber.Interfaces;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber.ModelParts
{
    /// <summary>
    /// Резьба.
    /// </summary>
    public class Carving : IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var carvingLength = parameters[Parameter.CarvingLength].Value;
            var carvingDiameter = parameters[Parameter.CarvingDiameter].Value/4;
            
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
                    OperationColor = Color.FromArgb(40, 40, 40)
                };

                float step = 0;
                sketchProperty.PointsList.Add(new PointF(0, -22));
                sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -22));
                sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -21.56f));

                while (step < carvingLength)
                {
                    sketchProperty.PointsList.Add(new PointF(1.6f + carvingDiameter, -21.56f + step));
                    sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -21.38f + step));
                    sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -21.32f + step));
                    sketchProperty.PointsList.Add(new PointF(1.6f + carvingDiameter, -21.14f + step));

                    step += 0.42f;
                }

                sketchProperty.PointsList.Add(new PointF(1.6f + carvingDiameter, -21.56f + step));
                sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -21.38f + step));
                sketchProperty.PointsList.Add(new PointF(1.725f + carvingDiameter, -21.32f + step));
                sketchProperty.PointsList.Add(new PointF(1.63f + carvingDiameter, -21.14f + step + 0.2f));
                sketchProperty.PointsList.Add(new PointF(0, -21.14f + step + 0.2f));

                sketchProperty.SketchName = "Резьба";
                sketchProperty.CreateNewSketch(part);

                //sketchProperty.
            }
        }
    }
}