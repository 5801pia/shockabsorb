using Kompas6API5;
using System.Collections.Generic;
using ShockAbsorber.Enumerations;

namespace ShockAbsorber.Interfaces
{
    /// <summary>
    /// Интерфейс части модели.
    /// </summary>
    public interface IModelPart
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters);
    }
}