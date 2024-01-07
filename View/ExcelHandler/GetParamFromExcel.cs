using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ExcelHandler
{
    /// <summary>
    /// Получение 
    /// </summary>
    public class GetParamFromExcel
    {
        private readonly string _pathFile1 = "C:\\Users\\Дарья\\Desktop" +
                                    "\\1. ВКР\\ИТ\\Excel\\power_consum_max_coefficient_2023_140623.xlsx";

        private readonly string _pathFile2 = "C:\\Users\\Дарья\\Desktop" +
                            "\\1. ВКР\\ИТ\\Excel\\temp_coefficient_2023_140623.xlsx";

        private readonly string _textToFind = "Забайкальская";


        /// <summary>
        /// Метод открывающий файл Excel.
        /// </summary>
        /// <param name="_pathFile">Открываемый файл.</param>
        /// <returns>Лист в открытом файле excel.</returns>
        private Worksheet OpenFileExcel(string _pathFile)
        {
            // Excel
            Application xlApp = new();

            //рабочая книга
            Workbook xlWB;

            //лист Excel
            Worksheet xlSht;

            //название файла Excel
            xlWB = xlApp.Workbooks.Open(_pathFile);

            //название листа или 1-й лист в книге xlSht = xlWB.Worksheets[1];
            xlSht = xlWB.Worksheets[1];

            // добавление закрытия excel.xml
            // xlApp.Workbooks.Close();
            // xlApp.Quit();

            return xlSht;
        }

        /// <summary>
        /// Метод позволяющий искать ЭС в Excel.
        /// </summary>
        /// <param name="textToFind">Искомое слово.</param>
        /// <param name="xlSht">Лист в Excel.</param>
        /// <returns>Номер строки в Excel.</returns>
        private static int FindRowInExcel(string textToFind, Worksheet xlSht)
        {
            //диапазон ячеек
            Excel.Range Rng;

            textToFind = textToFind.Length > 6
                ? textToFind.Remove(textToFind.Length - 4)
                : textToFind.Remove(textToFind.Length - 2);

            // осуществляем поиск на листе
            Rng = xlSht.Cells.Find(textToFind, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart);

            int rowExcel = 0;

            if (Rng != null)
            {
                rowExcel = Convert.ToInt32(Regex.Replace(Rng.Address, @"[^\d]+", ""));
            }
            else
            {
                Console.WriteLine($"Текст {textToFind} на листе не найден!");
            }

            return rowExcel;
        }

        /// <summary>
        /// Получение массива коэффициенты зависимости изменения максимума...
        /// потребления мощности территориальных энергосистем при изменении...
        /// температуры наружного воздуха.
        /// </summary>
        /// <returns>массив коэффициентов.</returns>
        private double[,] FindExcelArray()
        {
            Worksheet xlSht = OpenFileExcel(_pathFile1);
            int rowExcel = FindRowInExcel(_textToFind, xlSht);

            double[,] arrayExcel = new double[3, 6];

            for (int i = 0; i < arrayExcel.GetLength(0); i++)
            {
                for (int j = 0; j < arrayExcel.GetLength(1); j++)
                {
                    if (xlSht.Cells[rowExcel + i, 3 + j].Value != null)
                    {
                        arrayExcel[i, j] = xlSht.Cells[rowExcel + i, 3 + j].Value;
                    }
                    else
                    {
                        arrayExcel[i, j] = 0;
                    }
                }
            }

            return arrayExcel;
        }

        /// <summary>
        /// Определение коэффициентов и расчётных температур наружного...
        /// воздуха ЭС, по наименованию ЭС.
        /// </summary>
        /// <returns>словарь коэффициентов и температур.</returns>
        private Dictionary<string, double> GetkoefToES()
        {
            Worksheet xlSht = OpenFileExcel(_pathFile2);
            int rowExcel = FindRowInExcel(_textToFind, xlSht);

            Dictionary<string, double> dictionaryKoef = new()
            {
                { "kZimaMinMax", xlSht.Cells[rowExcel, 3].Value },
                { "kLetoMinMax", xlSht.Cells[rowExcel, 4].Value },
                { "kLZMax", xlSht.Cells[rowExcel, 6].Value },
                { "tsrSIPR", xlSht.Cells[rowExcel, 38].Value },
                { "tLeto0.98", xlSht.Cells[rowExcel, 37].Value },
                { "tZima0.92", xlSht.Cells[rowExcel, 36].Value },
                { "tGOST", xlSht.Cells[rowExcel, 40].Value },
                { "tLetoNorm", xlSht.Cells[rowExcel, 39].Value }
            };

            return dictionaryKoef;
        }

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
        public double GetPowerMax(double tempCalcul, double tempInit, double pInit)
        {
            double[,] excelarray = FindExcelArray();

            double pCalaul = pInit;

            for (int j = 0; j < excelarray.GetLength(1); j++)
            {
                double koef = excelarray[2, j];

                if (tempCalcul > excelarray[0, j] && tempCalcul < excelarray[1, j])
                {
                    Dictionary<string, double> dictionaryKoef = GetkoefToES();

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

        public double[] CalculatePowerConsumption(int pInit)
        {
            Dictionary<string, double> dictionaryKoef = GetkoefToES();

            double[] arrayPower = new double[7];

            double pMaxZima092 = GetPowerMax(dictionaryKoef["tZima0.92"], dictionaryKoef["tsrSIPR"], pInit);
            double pMaxZimaGOST = GetPowerMax(dictionaryKoef["tGOST"], dictionaryKoef["tsrSIPR"], pInit);
            double pMinZima092 = Powerkoef(pMaxZima092, dictionaryKoef["kZimaMinMax"]);
            double pMinZimaGOST = Powerkoef(pMaxZimaGOST, dictionaryKoef["kZimaMinMax"]);

            double pMaxLetoCalcul = Powerkoef(pInit, dictionaryKoef["kLZMax"]);

            double pMaxLeto098 = GetPowerMax(dictionaryKoef["tLeto0.98"], dictionaryKoef["tsrSIPR"], pMaxLetoCalcul);
            double pMaxLetoNorm = GetPowerMax(dictionaryKoef["tLetoNorm"], dictionaryKoef["tsrSIPR"], pMaxLetoCalcul);
            double pMinLetoNorm = Powerkoef(pMaxLetoNorm, dictionaryKoef["kLetoMinMax"]);

            arrayPower[0] = pMinZima092;
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
