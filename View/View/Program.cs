using Model;
namespace View
{
    internal class Program
    {
        static void Main()
        {
            List<SolarPowerPlant> solarPowerPlant = new()
            {
                new SolarPowerPlant(1, "Борзинская", StatusSPP.entered, 60303100, PowerSystem.Zabaikalskaya, 60),
                new SolarPowerPlant(2, "Быстринская", StatusSPP.entered, 60303107, PowerSystem.Zabaikalskaya, 60),
                new SolarPowerPlant(3, "Дружная", StatusSPP.entered, 60303102, PowerSystem.Zabaikalskaya, 60),
                new SolarPowerPlant(4, "Жимбирская", StatusSPP.entered, 60303106, PowerSystem.Zabaikalskaya, 60),
                new SolarPowerPlant(5, "Пограничная", StatusSPP.entered, 60303101, PowerSystem.Zabaikalskaya, 60),
                new SolarPowerPlant(6, "Ононская", StatusSPP.entered, 60303109, PowerSystem.Zabaikalskaya, 127.8),
                new SolarPowerPlant(7, "Луговая", StatusSPP.entered, 60303103, PowerSystem.Zabaikalskaya, 133),
                new SolarPowerPlant(8, "Полевая", StatusSPP.entered, 60303105, PowerSystem.Zabaikalskaya, 141),
                new SolarPowerPlant(9, "Майдари", StatusSPP.entered, 60303104, PowerSystem.Zabaikalskaya, 210)
            };
        }
    }
}