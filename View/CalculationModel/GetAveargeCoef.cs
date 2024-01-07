using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationModel
{
    /// <summary>
    /// Класс для вычисления среднего коэффициента выработки...
    /// мощности по действующим СЭС.
    /// </summary>
    public class GetAveargeCoef
    {
        /// <summary>
        /// Словарь средних коэф. выработки мощности для каждого режима.
        /// </summary>
        public static readonly Dictionary<OperatingModePS, double> ModesOperating = new()
        {
            [OperatingModePS.KWinterMaxAM] = 0.0757,
            [OperatingModePS.KWinterMaxPM] = 0.01005,
            [OperatingModePS.KWinterMin] = 0,
            [OperatingModePS.KSummerMaxAM] = 0.2787,
            [OperatingModePS.KSummerMaxPM] = 0.01024,
            [OperatingModePS.KSummerMin] = 0.0085,
        };
    }
}
