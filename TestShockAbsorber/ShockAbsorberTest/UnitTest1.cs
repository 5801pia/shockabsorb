using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShockAbsorber;
using ShockAbsorber.ModelParts;
using ShockAbsorber.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using ShockAbsorber.Enumerations;


namespace ShockAbsorberTest
{
    [TestClass]
    public class ShockAbsorberTest
    {
        /// <summary>
        /// Тестирование параметра позиции гайки на корректность
        /// </summary>
        [TestMethod]
        public void TestGearPositionValid()
        {
            var parametrData = new ModelParameters();

            var parametrs = new Dictionary<Parameter, ParameterData>
            {
                {
                    Parameter.GearPosition, new ParameterData(Parameter.GearPosition.ToString(), 16, new PointF(0, 16))
                },
            };

            List<string> error = parametrData.CheckData(parametrs);
            if (error.Count != 0)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Тестирование параметра длины резьбы на корректность
        /// </summary>
        [TestMethod]
        public void TestCarvingLengthValid()
        {
            var parametrData = new ModelParameters();

            var parametrs = new Dictionary<Parameter, ParameterData>
            {
                {
                    Parameter.CarvingLength, new ParameterData(Parameter.CarvingLength.ToString(), 15, new PointF(5, 15))
                },
            };

            List<string> error = parametrData.CheckData(parametrs);
            if (error.Count != 0)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Тестирование конструктора класса точки в пространстве
        /// </summary>
        [TestMethod]
        public void TestPointConstructor()
        {
            Point3D point = new Point3D(1, 2, 3);
            Assert.AreEqual(1, point.X);
            Assert.AreEqual(2, point.Y);
            Assert.AreEqual(3, point.Z);
        }
    }
}
