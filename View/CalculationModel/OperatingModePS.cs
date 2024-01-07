using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationModel
{
    /// <summary>
    /// Перечисление коэффициентов средней выработки СЭС.
    /// </summary>
    public enum OperatingModePS
    {
        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для максимального зимнего режима (утро).
        /// </summary>
        KWinterMaxAM,

        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для максимального зимнего режима (вечер).
        /// </summary>
        KWinterMaxPM,

        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для минимального зимнего режима.
        /// </summary>
        KWinterMin,

        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для максимального летнего режима (утро).
        /// </summary>
        KSummerMaxAM,

        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для максимального летнего режима (вечер).
        /// </summary>
        KSummerMaxPM,

        /// <summary>
        /// Коэффициент средней выработки мощности действующих...
        /// ...СЭС для минимального летнего режима.
        /// </summary>
        KSummerMin
    }
}
