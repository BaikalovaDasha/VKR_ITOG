using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationModel
{
    /// <summary>
    /// Класс для расчёта потребления мощности в зависимости от наружного воздуха.
    /// </summary>
    public class CalculPowerConsumption
    {
        /// <summary>
        /// Расчёт потребления мощности. 
        /// </summary>
        /// <param name="pInit">Исходная мощность потребления ЭС.</param>
        /// <param name="TempRasch">расчётное значение температуры.</param>
        /// <param name="TempIsx">значение температуры исходных условий.</param>
        /// <param name="k">коэффициент знависимости изменения потребления мощности.</param>
        /// <returns>Расчётное значение мощности.</returns>
        private static double CalculatePower(double pInit, double TempRasch, double TempIsx, double k)
        {
            return pInit * (1 + k / 100 * (TempRasch - TempIsx));
        }

        /// <summary>
        /// Метод перерасчёта потребления мощности в зависимости от наружного воздуха.
        /// </summary>
        /// <param name="tempCalcul">расчётное значение температуры.</param>
        /// <param name="tempInit">исходное значение температуры.</param>
        /// <param name="pInit">исходное значение мощности.</param>
        public static double GetPowerMax1(double tempCalcul, double tempInit, double pInit)
        {
            ExcelHandler.GetParamFromExcel paramFromExcel = new();

            double[,] excelarray = paramFromExcel.FindExcelArray();

            double pCalaul = pInit;

            for (int j = 0; j < excelarray.GetLength(1); j++)
            {
                double koef = excelarray[2, j];

                if (tempCalcul > excelarray[0, j] && tempCalcul < excelarray[1, j])
                {
                    Dictionary<string, double> dictionaryKoef = paramFromExcel.GetkoefToES();

                    if (tempInit == dictionaryKoef["tsrSIPR"])
                    {
                        pCalaul = CalculatePower(pCalaul, tempCalcul, tempInit, koef);
                        break;
                    }
                    else
                    {
                        pCalaul = CalculatePower(pCalaul, tempCalcul, excelarray[0, j], koef);
                        break;
                    }

                }
                else
                {
                    pCalaul = CalculatePower(pCalaul, excelarray[1, j], tempInit, koef);
                    tempInit = excelarray[0, j + 1];
                }
            }

            return pCalaul;
        }

        /// <summary>
        /// Метод перерасчёта потребления мощности в зависимости от наружного воздуха.
        /// </summary>
        /// <param name="tempCalcul">расчётное значение температуры.</param>
        /// <param name="tempInit">исходное значение температуры.</param>
        /// <param name="pInit">исходное значение мощности.</param>
        public static double GetPowerMax(double tempCalcul, double tempInit, double pInit, Dictionary<string, double> dictionaryKoef, List<double[]> excelarray)
        {
            double pCalaul = pInit;

            foreach (var item in excelarray)
            {
                double koef = item[2];

                if (tempCalcul > item[0] && tempCalcul < item[1])
                {

                    if (tempInit == dictionaryKoef["tsrSIPR"])
                    {
                        pCalaul = CalculatePower(pCalaul, tempCalcul, tempInit, koef);
                        break;
                    }
                    else
                    {
                        pCalaul = CalculatePower(pCalaul, tempCalcul, item[0], koef);
                        break;
                    }

                }
                else
                {
                    pCalaul = CalculatePower(pCalaul, item[1], tempInit, koef);
                    tempInit = excelarray[excelarray.IndexOf(item) + 1][0];
                }
            }

            return pCalaul;
        }



        /// <summary>
        /// Метод расчёта потребления мощности в зависимости от наружного воздуха.
        /// </summary>
        /// <param name="pInit">Мощность полученная в результате перерасчёта.</param>
        /// <param name="k">коэффициент соотношения максимального...
        /// и минимального потребления мощности в зависимости от...
        /// периода года.</param>
        /// <returns>Расчётное значение мощности.</returns>
        private static double Powerkoef(double pInit, double k)
        {
            return pInit * k;
        }

        public static double[] CalculatePowerConsumption(int pInit)
        {
            ExcelHandler.GetParamFromExcel paramFromExcel = new();

            Dictionary<string, double> dictionaryKoef = paramFromExcel.GetkoefToES();

            List<double[]> excelArrayKoef = paramFromExcel.FindExcelArray1();

            double[] arrayPower = new double[7];

            double pMaxZima092 = GetPowerMax(dictionaryKoef["tZima0.92"], dictionaryKoef["tsrSIPR"], pInit, dictionaryKoef, excelArrayKoef);
            double pMaxZimaGOST = GetPowerMax(dictionaryKoef["tGOST"], dictionaryKoef["tsrSIPR"], pInit, dictionaryKoef, excelArrayKoef);
            double pMinZima092 = Powerkoef(pMaxZima092, dictionaryKoef["kZimaMinMax"]);
            double pMinZimaGOST = Powerkoef(pMaxZimaGOST, dictionaryKoef["kZimaMinMax"]);

            double pMaxLetoCalcul = Powerkoef(pInit, dictionaryKoef["kLZMax"]);

            double pMaxLeto098 = GetPowerMax(dictionaryKoef["tLeto0.98"], dictionaryKoef["tsrSIPR"], pMaxLetoCalcul, dictionaryKoef, excelArrayKoef);
            double pMaxLetoNorm = GetPowerMax(dictionaryKoef["tLetoNorm"], dictionaryKoef["tsrSIPR"], pMaxLetoCalcul, dictionaryKoef, excelArrayKoef);
            double pMinLetoNorm = Powerkoef(pMaxLetoNorm, dictionaryKoef["kLetoMinMax"]);

            arrayPower[0] = pMaxZima092;
            arrayPower[1] = pMaxZimaGOST;
            arrayPower[2] = pMinZima092;
            arrayPower[3] = pMinZimaGOST;
            arrayPower[4] = pMaxLeto098;
            arrayPower[5] = pMaxLetoNorm;
            arrayPower[6] = pMinLetoNorm;

            for (int i = 0; i < arrayPower.Length; i++)
            {
                Console.WriteLine(arrayPower[i]);
            }

            return arrayPower;
        }
    }
}
