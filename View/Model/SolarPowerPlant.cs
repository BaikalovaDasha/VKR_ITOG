using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Класс с параметрами СЭС.
    /// </summary>
    public class SolarPowerPlant
    {
        /// <summary>
        /// Номер СЭС.
        /// </summary>
        private int _numberSPP;

        /// <summary>
        /// Нименование СЭС.
        /// </summary>
        private string _nameSPP;

        /// <summary>
        /// Gets or sets статуса СЭС.
        /// </summary>
        public StatusSPP StatusSPP { get; set; }

        /// <summary>
        /// Номер агрегата СЭС в РМ.
        /// </summary>
        private int _SPPNum;

        /// <summary>
        /// Gets or sets Энрегосистема.
        /// </summary>
        public PowerSystem PowerSystem { get; set; }

        /// <summary>
        /// Установленная мощность.
        /// </summary>
        private double _installedCapacity;

        /// <summary>
        /// Gets or sets номера СЭС.
        /// </summary>
        public int NumberSPP
        {
            get
            {
                return _numberSPP;
            }

            set
            {
                _numberSPP = CheckingNumber(value);
            }
        }

        /// <summary>
        /// Gets or sets наименование СЭС.
        /// </summary>
        public string NameSPP
        {
            get
            {
                return _nameSPP;
            }

            set
            {
                _nameSPP = value;
            }
        }

        /// <summary>
        /// Gets or sets узла в РМ.
        /// </summary>
        public int SPPNum
        {
            get
            {
                return _SPPNum;
            }

            set
            {
                _SPPNum = CheckingNumber(value);
            }
        }

        /// <summary>
        /// Gets or sets номера СЭС.
        /// </summary>
        public double InstallCapacity
        {
            get
            {
                return _installedCapacity;
            }

            set
            {
                _installedCapacity = CheckingNumber((int)value);
            }
        }

        /// <summary>
        /// Конструктор структуры.
        /// </summary>
        /// <param name="number">Номер СЭС.</param>
        /// <param name="name">Наименование СЭС.</param>
        /// <param name="status">Статус СЭС.</param>
        /// <param name="num">Номер агрегата СЭС.</param>
        /// <param name="system">ЭС где находится СЭС.</param>
        /// <param name="capacity">Установленная мощность СЭС.</param>
        public SolarPowerPlant(int number, string name, StatusSPP status,
            int num, PowerSystem system, double capacity)
        {
            NumberSPP = number;
            NameSPP = name;
            StatusSPP = status;
            SPPNum = num;
            PowerSystem = system;
            InstallCapacity = capacity;
        }

        /// <summary>
        /// Вывод информации о СЭС.
        /// </summary>
        /// <returns>информация о СЭС.</returns>
        public string GetInfo()
        {
            var sppInfo = $"№ {NumberSPP}. Наименование СЭС: {NameSPP}. ";

            if (StatusSPP == StatusSPP.entered)
            {
                sppInfo += $"Статус: вводимая СЭС. ";
            }
            else
            {
                sppInfo += $"Статус: действующая СЭС. ";
            }

            sppInfo += $"Агрегат: {SPPNum}. ";

            if (PowerSystem == PowerSystem.Zabaikalskaya)
            {
                sppInfo += $"ЭС: Забайкальская. ";
            }
            else
            {
                sppInfo += $"ЭС: пока нет. ";
            }

            sppInfo += $"Мощность: {InstallCapacity}.";

            return sppInfo;
        }

        /// <summary>
        /// Проверка параметра.
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>проверенное число.</returns>
        /// <exception cref="ArgumentException">отбрасывает отрицательные...
        /// ...числа.</exception>
        private static int CheckingNumber(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Параметр должен быть" +
                    " положительным!");
            }

            return number;
        }
    }
}
