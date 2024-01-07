using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Перечисление существующих статусов СЭС.
    /// </summary>
    public enum StatusSPP
    {
        /// <summary>
        /// вводимая СЭС.
        /// </summary>
        entered,

        /// <summary>
        /// действующая СЭС.
        /// </summary>
        operating
    }
}
