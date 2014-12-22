using System.Drawing;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber
{
    /// <summary>
    /// Содержит параметры модели.
    /// </summary>
    public class ModelParameters
    {
        /// <summary>
        /// Словарь параметров.
        /// </summary>
        public Dictionary<Parameter, ParameterData> Parameters { get; private set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ModelParameters()
        {
            Initialize();
        }

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            Parameters = new Dictionary<Parameter, ParameterData>
                {
                    {
                        Parameter.BodyLength,
                        new ParameterData(Parameter.BodyLength.ToString(), 25, new PointF(20, 30))
                    },
                    {
                        Parameter.BodyDiameter,
                        new ParameterData(Parameter.BodyDiameter.ToString(), 6, new PointF(5, 10))
                    },
                    {
                        Parameter.RodDiameter,
                        new ParameterData(Parameter.RodDiameter.ToString(), 0.8f, new PointF(0.5f, 2))
                    },
                    {
                        Parameter.CarvingLength,
                        new ParameterData(Parameter.CarvingLength.ToString(), 12, new PointF(5, 15))
                    },
                    {
                        Parameter.CarvingDiameter,
                        new ParameterData(Parameter.CarvingDiameter.ToString(), 4, new PointF(2, 6))
                    },
                    {
                        Parameter.ScrewDirection,
                        new ParameterData(Parameter.ScrewDirection.ToString(), 0, new PointF(0, 1))
                    },
                    {
                        Parameter.SpringDiameter,
                        new ParameterData(Parameter.SpringDiameter.ToString(), 0.58f, new PointF(0.2f, 0.6f))
                    },
                    {
                        Parameter.FirstHoleDiameter,
                        new ParameterData(Parameter.FirstHoleDiameter.ToString(), 1.35f, new PointF(0.75f, 2))
                    },
                    {
                        Parameter.SecondHoleDiameter,
                        new ParameterData(Parameter.SecondHoleDiameter.ToString(), 1.35f, new PointF(0.75f, 2))
                    },
                    {
                        Parameter.CircleThickness,
                        new ParameterData(Parameter.CircleThickness.ToString(), 0.1f, new PointF(0.1f, 1))
                    },
                    {
                        Parameter.GearPosition,
                        new ParameterData(Parameter.GearPosition.ToString(), 12, new PointF(0, 16))
                    },
                };
        }

        /// <summary>
        /// Проверяет корректность введенных данных.
        /// </summary>
        /// <param name="parameters">Словарь параметров для проверки.</param>
        /// <returns>Список ошибок.</returns>
        public List<string> CheckData(Dictionary<Parameter, ParameterData> parameters)
        {
            var errorList = new List<string>();

            SetDependency(parameters);

            foreach (KeyValuePair<Parameter, ParameterData> parameter in parameters)
            {
                var value = parameter.Value.Value;
                var validValue = GetValidValue(parameter.Key);

                if (validValue == null) continue;

                if (!(value >= validValue.RangeValue.X && value <= validValue.RangeValue.Y))
                {
                    errorList.Add("Значение параметра '" + parameter.Value.Description +
                                  "', должно лежать в диапазоне от " + validValue.RangeValue.X + " до " +
                                  validValue.RangeValue.Y + ".\n");
                }
            }

            return errorList;
        }

        /// <summary>
        /// Устанавливает зависимсти параметров друг от друга.
        /// </summary>
        /// <param name="parameters">Список параметров.</param>
        private void SetDependency(Dictionary<Parameter, ParameterData> parameters)
        {
            foreach (KeyValuePair<Parameter, ParameterData> parameter in parameters)
            {
                switch (parameter.Key)
                {
                    case Parameter.CarvingLength:
                        {
                            SetMaxValue(Parameter.GearPosition, parameter.Value.Value);
                        }
                        break;
                    

                }
            }
        }

        /// <summary>
        /// Возвращает допустимые значения.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Допустимое значение.</returns>
        private ParameterData GetValidValue(Parameter parameter)
        {
            if (Parameters.ContainsKey(parameter))
            {
                return Parameters[parameter];
            }

            return null;
        }

        /// <summary>
        /// Задает новое максимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="maxValue">Новое значение.</param>
        private void SetMaxValue(Parameter parameter, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(currentParameter.RangeValue.X, maxValue));
            }
        }

        /// <summary>
        /// Задает новое минимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Новое значение.</param>
        private void SetMinValue(Parameter parameter, float minValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, currentParameter.RangeValue.Y));
            }
        }

        /// <summary>
        /// Задает новый диапазон значений параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Минимальное значение.</param>
        /// <param name="maxValue">Максимальное значение.</param>
        private void SetRange(Parameter parameter, float minValue, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, maxValue));
            }
        }
    }
}