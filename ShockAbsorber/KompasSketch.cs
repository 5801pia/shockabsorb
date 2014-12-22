using System.Linq;
using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber
{
    /// <summary>
    /// Свойства эскиза.
    /// </summary>
    public class KompasSketch
    {
        #region - Переменные -

        /// <summary>
        /// Счетчик операций.
        /// </summary>
        private int _operationsCounter;

        /// <summary>
        /// Список разделителей.
        /// </summary>
        private List<int> _breakPointsList;

        #endregion // Переменные.

        #region - Конструктор -

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public KompasSketch()
        {
            Initialize();
        }

        #endregion // Конструктор.
        
        #region - Инициализация -

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            SketchName = string.Empty;
            ArcPoints = new List<PointF>();
            PointsList = new List<PointF>();
            Spline3DPoints = new List<Point3D>();
            OperationsDictionary = new Dictionary<int, ksEntity>();

            _operationsCounter = 0;
            _breakPointsList = new List<int>();
        }

        #endregion // Инициализация.
        
        #region - Свойства -

        /// <summary>
        /// Название эскиза.
        /// </summary>
        public string SketchName { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public double NormalValue { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public double ReverseValue { get; set; }

        /// <summary>
        /// Количество копий в массиве.
        /// </summary>
        public int CopiesCount { get; set; }

        /// <summary>
        /// Примитив.
        /// </summary>
        public ShapeType Shape { get; set; }

        /// <summary>
        /// Плоскость для рисования.
        /// </summary>
        public PlaneType Plane { get; set; }

        /// <summary>
        /// Радиус окружности.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool WithArc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PointF> ArcPoints { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Point3D> Spline3DPoints { get; set; }

        /// <summary>
        /// True - если необходимо сместить плоскость.
        /// </summary>
        public bool IsOffsetPlane { get; set; }

        /// <summary>
        /// Величина смещения плоскости.
        /// </summary>
        public double OffsetPlaneValue { get; set; }

        /// <summary>
        /// Направление спещения плоскости.
        /// </summary>
        public bool PlaneDirectionUp { get; set; }

        /// <summary>
        /// True - если необходимо использовать осевую линию.
        /// </summary>
        public bool UseCenterLine { get; set; }

        /// <summary>
        /// Начальное положение центральной линии.
        /// </summary>
        public PointF CenterLineStartPos { get; set; }
        /// <summary>
        /// Конечное положение центральной линии.
        /// </summary>
        public PointF CenterLineEndPos { get; set; }

        /// <summary>
        /// Цвет операции.
        /// </summary>
        public Color OperationColor { get; set; }

        /// <summary>
        /// Направление каманды.
        /// </summary>
        public Direction_Type DirectionType { get; set; }

        /// <summary>
        /// Список координат фигуры.
        /// </summary>
        public List<PointF> PointsList { get; set; }

        /// <summary>
        /// Список операций.
        /// </summary>
        public OperationType Operation { get; set; }

        /// <summary>
        /// Словарь операций.
        /// </summary>
        public Dictionary<int, ksEntity> OperationsDictionary;

        #endregion // Свойства.

        #region - Public методы -

        /// <summary>
        /// Добавляет разделитель линий.
        /// </summary>
        public void AddBreakPoint()
        {
            _breakPointsList.Add(PointsList.Count - 1);
        }

        /// <summary>
        /// Очищает список разделителей линий.
        /// </summary>
        public void ClearBreakPoint()
        {
            _breakPointsList.Clear();
        }

        /// <summary>
        /// Создает новый эскиз.
        /// </summary>
        /// <param name="part">Новая деталь.</param>
        public void CreateNewSketch(ksPart part)
        {
            var entitySketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);

            if (entitySketch == null) return;

            // Интерфейс свойств эскиза.
            var sketchDef = (ksSketchDefinition)entitySketch.GetDefinition();
            if (sketchDef != null)
            {
                var plane = CreateDefaultPlane(part);
                var basePlane = IsOffsetPlane ? CreateOffsetPlane(part, plane) : plane;

                // Установим плоскость базовой для эскиза.
                sketchDef.SetPlane(basePlane);

                // 
                if (!string.IsNullOrEmpty(SketchName))
                    entitySketch.name = SketchName;

                // Создадим эскиз.
                entitySketch.Create();

                // Интерфейс редактора эскиза.
                var sketchEdit = (ksDocument2D)sketchDef.BeginEdit();

                switch (Shape)
                {
                    case ShapeType.Line:
                        {
                            DrawLine(sketchEdit);
                        }
                        break;

                    case ShapeType.Circle:
                        {
                            DrawCircle(sketchEdit);
                        }
                        break;
                }

                if (UseCenterLine)
                {
                    sketchEdit.ksLineSeg(CenterLineStartPos.X, CenterLineStartPos.Y, CenterLineEndPos.X,
                                         CenterLineEndPos.Y, 3);
                }

                // Завершение редактирования эскиза.
                sketchDef.EndEdit();

                switch (Operation)
                {
                    case OperationType.BaseExtrusion:
                        {
                            BaseExtrusion(part, entitySketch);
                        }
                        break;

                    case OperationType.BaseRotated:
                        {
                            BaseRotated(part, entitySketch);
                        }
                        break;

                    case OperationType.CutExtrusion:
                        {
                            CutExtrusion(part, entitySketch);
                        }
                        break;

                    case OperationType.BaseEvolution:
                        {
                            BaseEvolution(part, entitySketch);
                        }
                        break;
                }
            }
        }

        #endregion // Public методы.

        #region - Операции 2D -

        /// <summary>
        /// Рисует линию.
        /// </summary>
        /// <param name="sketchEdit">Эскиз для рисования.</param>
        private void DrawLine(ksDocument2D sketchEdit)
        {
            if (PointsList.Count == 0) return;

            var pointList = new List<PointF>();

            for (int i = 0; i < PointsList.Count; i++)
            {
                if (_breakPointsList.Contains(i))
                {
                    pointList.Add(PointsList[i]);

                    DrawLine(sketchEdit, pointList);

                    pointList.Clear();

                    if (i != PointsList.Count - 1)
                    {
                        i++;
                    }
                }

                pointList.Add(PointsList[i]);
            }

            DrawLine(sketchEdit, pointList);
        }

        /// <summary>
        /// Рисует линию.
        /// </summary>
        /// <param name="sketchEdit">Эскиз для рисования.</param>
        /// <param name="pointsList">Координаты линии.</param>
        private void DrawLine(ksDocument2D sketchEdit, List<PointF> pointsList)
        {
            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                sketchEdit.ksLineSeg(pointsList[i].X, pointsList[i].Y, pointsList[i + 1].X, pointsList[i + 1].Y, 1);
            }

            if (WithArc)
            {
                sketchEdit.ksNurbs(3, false, 1);

                foreach (PointF point in ArcPoints)
                {
                    sketchEdit.ksPoint(point.X, point.Y, 0);
                }

                sketchEdit.ksEndObj();
            }
            else
            {
                int index = pointsList.Count - 1;
                sketchEdit.ksLineSeg(pointsList[index].X, pointsList[index].Y, pointsList[0].X, pointsList[0].Y, 1);
            }
        }

        /// <summary>
        /// Рисует окружность.
        /// </summary>
        /// <param name="sketchEdit">Эскиз для рисования.</param>
        public void DrawCircle(ksDocument2D sketchEdit)
        {
            foreach (PointF center in PointsList)
            {
                sketchEdit.ksCircle(center.X, center.Y, Radius, 1);
            }
        }

        #endregion // Операции 2D.

        #region - Операции 3D -

        /// <summary>
        /// Базовая операция выдавливания.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="entitySketch">Эскиз.</param>
        private void BaseExtrusion(ksPart part, ksEntity entitySketch)
        {
            // Построим выдавливанием.
            var entityExtr = (ksEntity)part.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            if (entityExtr != null)
            {
                // Интерфейс свойств базовой операции выдавливания.
                var extrusionDef = (ksBaseExtrusionDefinition)entityExtr.GetDefinition();

                // Интерфейс базовой операции выдавливания.
                if (extrusionDef != null)
                {
                    // Направление выдавливания.
                    extrusionDef.directionType = (short)DirectionType;

                    // 1 - прямое направление,
                    // 2 - строго на глубину,
                    // 3 - расстояние.
                    switch (DirectionType)
                    {
                        case Direction_Type.dtNormal:
                            {
                                extrusionDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                            }
                            break;

                        case Direction_Type.dtReverse:
                            {
                                extrusionDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;

                        case Direction_Type.dtBoth:
                            {
                                extrusionDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                                extrusionDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;
                    }

                    // Задаем цвет операции.
                    var colorParam = (ksColorParam)entityExtr.ColorParam();
                    colorParam.color = GetKompasColor(OperationColor);

                    // Эскиз операции выдавливания.
                    extrusionDef.SetSketch(entitySketch);

                    // Создать операцию.
                    entityExtr.Create();

                    // Обновить параметры эскиза.
                    entitySketch.Update();

                    // Обновить параметры операции выдавливания.
                    entityExtr.Update();
                }
            }

            AddOperation(entityExtr);
        }

        /// <summary>
        /// Вырезать выдавливанием.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="entitySketch">Эскиз.</param>
        public void CutExtrusion(ksPart part, ksEntity entitySketch)
        {
            // Вырежим выдавливанием.
            var entityCutExtr = (ksEntity)part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            if (entityCutExtr != null)
            {
                var cutExtrDef = (ksCutExtrusionDefinition)entityCutExtr.GetDefinition();
                if (cutExtrDef != null)
                {
                    // Прямое направление.
                    cutExtrDef.directionType = (short)DirectionType;

                    // 1 - прямое направление,
                    // 2 - строго на глубину,
                    // 3 - расстояние.
                    switch (DirectionType)
                    {
                        case Direction_Type.dtNormal:
                            {
                                cutExtrDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                            }
                            break;

                        case Direction_Type.dtReverse:
                            {
                                cutExtrDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;

                        case Direction_Type.dtBoth:
                            {
                                cutExtrDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                                cutExtrDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;
                    }

                    // Задаем цвет операции.
                    var colorParam = (ksColorParam)entityCutExtr.ColorParam();
                    colorParam.color = GetKompasColor(OperationColor);

                    // Эскиз операции вырезания.
                    cutExtrDef.SetSketch(entitySketch);

                    // Создадим операцию вырезание выдавливанием.
                    entityCutExtr.Create();

                    // Обновить параметры эскиза.
                    entitySketch.Update();

                    // Обновить параметры операции вырезание выдавливанием.
                    entityCutExtr.Update();
                }
            }

            AddOperation(entityCutExtr);
        }

        /// <summary>
        /// Базовая операция вращения.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="entitySketch">Эскиз.</param>
        private void BaseRotated(ksPart part, ksEntity entitySketch)
        {
            var entityRotate = (ksEntity)part.NewEntity((short)Obj3dType.o3d_baseRotated);
            if (entityRotate != null)
            {
                var rotateDef = (ksBaseRotatedDefinition)entityRotate.GetDefinition();

                // Интерфейс базовой операции вращения.
                if (rotateDef != null)
                {
                    // Прямое направление.
                    rotateDef.directionType = (short)DirectionType;
                    rotateDef.toroidShapeType = false;
                    rotateDef.SetSideParam(true, NormalValue);

                    // Обратное направление.
                    if (DirectionType == Direction_Type.dtBoth)
                    {
                        rotateDef.SetSideParam(false, ReverseValue);
                    }

                    // Задаем цвет операции.
                    var colorParam = (ksColorParam)entityRotate.ColorParam();
                    colorParam.color = GetKompasColor(OperationColor);

                    // Эскиз операции вырезания.
                    rotateDef.SetSketch(entitySketch);

                    // Создадим операцию вырезание выдавливанием.
                    entityRotate.Create();

                    // Обновить параметры эскиза.
                    entitySketch.Update();

                    // Обновить параметры операции вырезание выдавливанием.
                    entityRotate.Update();
                }
            }

            AddOperation(entityRotate);
        }

        /// <summary>
        /// Создает операцию.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="entitySketch">Эскиз.</param>
        public void BaseEvolution(ksPart part, ksEntity entitySketch)
        {
            var spline = Spline3D(part, Spline3DPoints);

            var entityEvolution = (ksEntity)part.NewEntity((short)Obj3dType.o3d_baseEvolution);
            if (entityEvolution == null) return;

            var evolutionDefinition = (ksBaseEvolutionDefinition)entityEvolution.GetDefinition();
            if (evolutionDefinition != null)
            {
                evolutionDefinition.SetSketch(entitySketch);

                // Интерфейс массивов элементов модели.
                var pathPartArray = (ksEntityCollection)evolutionDefinition.PathPartArray();
                // Массив содержащий параметры.
                pathPartArray.Add(spline);

                // Получаем параметры цвета и задаем цвет операции.
                var colorParam = (ksColorParam)entityEvolution.ColorParam();
                colorParam.color = GetKompasColor(OperationColor);

                entityEvolution.Create();
                entityEvolution.Update();
            }

        }

        /// <summary>
        /// Массив по концентрической сетке.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        public void CircularCopy(ksPart part)
        {
            CircularCopy(part, null, OperationsDictionary.Values.ToList());
        }

        /// <summary>
        /// Массив по концентрической сетке.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="x">X - координата поверхности,
        ///                     вокруг которой будет происходить копирование.</param>
        /// <param name="y">Y - координата поверхности,
        ///                     вокруг которой будет происходить копирование.</param>
        /// <param name="z">Z - координата поверхности,
        ///                     вокруг которой будет происходить копирование.</param>
        /// <param name="operations">Операции которые необходимо скопировать.</param>
        public void CircularCopy(ksPart part, double x, double y, double z, List<ksEntity> operations)
        {
            var coll = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            if (coll != null)
            {
                coll.SelectByPoint(x, y, z);
                CircularCopy(part, coll.GetByIndex(0), operations);
            }
        }

        /// <summary>
        /// Массив по концентрической сетке.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="axis">Ось.</param>
        /// <param name="operations">Операции которые необходимо скопировать.</param>
        public void CircularCopy(ksPart part, ksEntity axis, List<ksEntity> operations)
        {
            CircularCopy(part, axis, operations, 360);
        }

        /// <summary>
        /// Массив по концентрической сетке.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="axis">Ось.</param>
        /// <param name="operations">Операции которые необходимо скопировать.</param>
        /// <param name="angle"></param>
        public void CircularCopy(ksPart part, ksEntity axis, List<ksEntity> operations, int angle)
        {
            var cutCopy = (ksEntity)part.NewEntity((short)Obj3dType.o3d_circularCopy);
            var cutCopyDefinition = (ksCircularCopyDefinition)cutCopy.GetDefinition();

            if (axis == null)
            {
                cutCopyDefinition.SetAxis((ksEntity)part.NewEntity((short)Obj3dType.o3d_axisOX));
            }
            else
            {
                cutCopyDefinition.SetAxis(axis);
            }

            cutCopyDefinition.step2 = angle;
            cutCopyDefinition.count2 = CopiesCount;
            var collectionCutCopy = (ksEntityCollection)cutCopyDefinition.GetOperationArray();

            foreach (ksEntity operation in operations)
            {
                collectionCutCopy.Add(operation);
            }

            ksColorParam colors = cutCopy.ColorParam();
            colors.color = GetKompasColor(OperationColor);

            cutCopy.Create();
            cutCopy.Update();
        }

        /// <summary>
        /// Операция фаска.
        /// </summary>
        /// <param name="part">Компонент (интерфейс детали)</param>
        /// <param name="x">Координата x точки на ребре</param>
        /// <param name="y">Координата y точки на ребре</param>
        /// <param name="z">Координата z точки на ребре</param>
        /// <param name="r">Дистанция</param>
        public void Chamfer(ksPart part, double x, double y, double z, double r)
        {
            var entityChamfer = (ksEntity)part.NewEntity((short)Obj3dType.o3d_chamfer);
            if (entityChamfer != null)
            {
                var chamferDef = (ksChamferDefinition)entityChamfer.GetDefinition();

                //chamferDef.radius = r;
                chamferDef.tangent = false;
                chamferDef.SetChamferParam(true, r, r);

                var array = (ksEntityCollection)chamferDef.array();
                if (array != null)
                {
                    var coll = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_edge);
                    if (coll != null)
                    {
                        coll.SelectByPoint(x, y, z);
                        array.Add(coll.GetByIndex(0));
                    }
                }

                var colorParam = (ksColorParam)entityChamfer.ColorParam();

                // Задаем цвет операции.
                colorParam.color = GetKompasColor(OperationColor);

                entityChamfer.Create();
                entityChamfer.Update();
            }
        }

        /// <summary>
        /// Сплайн 3D.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="point3DList">Список точек.</param>
        public ksEntity Spline3D(ksPart part, List<Point3D> point3DList)
        {
            var entitySpline = (ksEntity)part.NewEntity((short)Obj3dType.o3d_spline);
            if (entitySpline != null)
            {
                var splineDefinition = (ksSplineDefinition)entitySpline.GetDefinition();
                if (splineDefinition != null)
                {
                    splineDefinition.splineOnPoles = true;

                    foreach (Point3D point3D in point3DList)
                    {
                        splineDefinition.AddVertex(point3D.X, point3D.Y, point3D.Z, 1);
                    }

                    entitySpline.Create();

                    entitySpline.Update();
                }
            }

            return entitySpline;
        }

        #endregion // Операции 3D.

        #region - Private методы -

        /// <summary>
        /// Добавляет операцию в словарь.
        /// </summary>
        /// <param name="operation">Операция.</param>
        private void AddOperation(ksEntity operation)
        {
            OperationsDictionary.Add(_operationsCounter, operation);
            _operationsCounter++;
        }

        /// <summary>
        /// Преобразует цвет модели в понятный для Компаса.
        /// </summary>
        /// <param name="color">Цвет модели.</param>
        /// <returns>Значение цвета.</returns>
        private int GetKompasColor(Color color)
        {
            return Color.FromArgb(color.B, color.G, color.R).ToArgb();
        }

        /// <summary>
        /// Возвращает текущую плоскость для рисования.
        /// </summary>
        private short ActivePlane
        {
            get
            {
                var plane = (short)Obj3dType.o3d_planeXOY;

                switch (Plane)
                {
                    case PlaneType.PlaneXOY:
                        plane = (short)Obj3dType.o3d_planeXOY;
                        break;

                    case PlaneType.PlaneXOZ:
                        plane = (short)Obj3dType.o3d_planeXOZ;
                        break;

                    case PlaneType.PlaneYOZ:
                        plane = (short)Obj3dType.o3d_planeYOZ;
                        break;
                }

                return plane;
            }
        }

        /// <summary>
        /// Создает плоскость.
        /// </summary>
        /// <param name="part">Деталь.</param>
        /// <returns>Плоскость.</returns>
        public ksEntity CreateDefaultPlane(ksPart part)
        {
            return (ksEntity)part.GetDefaultEntity(ActivePlane);
        }

        /// <summary>
        /// Создает смещенную плоскость.
        /// </summary>
        /// <param name="part">Деталь.</param>
        /// <param name="basePlane"></param>
        /// <returns>Смещенная плоскость.</returns>
        public ksEntity CreateOffsetPlane(ksPart part, ksEntity basePlane)
        {
            var offsetPlane = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);

            // Интерфейс смещенной плоскости.
            var offsetDefinition = (ksPlaneOffsetDefinition)offsetPlane.GetDefinition();

            offsetDefinition.SetPlane(basePlane);
            offsetDefinition.offset = OffsetPlaneValue;
            offsetDefinition.direction = PlaneDirectionUp;

            offsetPlane.name = "Смещенная плоскость";
            offsetPlane.Create();

            return offsetPlane;
        }

        #endregion // Private методы.
    }
}