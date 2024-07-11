using System.Xml.Linq;
using carList_LinQ;

namespace CarProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Car> cars = InitializeCars();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n\nVeuillez choisir une option parmi les suivantes : ");
                Console.WriteLine("\n=====================================================");
                Console.WriteLine("1 - Afficher la liste de toutes les voitures");
                Console.WriteLine("2 - Afficher les voitures triées par années");
                Console.WriteLine("3 - Afficher la liste des voitures avant une année donnée");
                Console.WriteLine("4 - Rechercher des voitures par nom de constructeur");
                Console.WriteLine("5 - Rechercher des voitures par nom de modèle");
                Console.WriteLine("6 - Rechercher des voitures électriques (depuis la liste ou depuis un fichier XML)");
                Console.WriteLine("7 - Générer un XML avec la liste de voiture présente");
                Console.WriteLine("10 - Quitter");
                Console.WriteLine("=====================================================\n");

                Console.Write("N° de l'option choisie (tapez 10 pour quitter le menu) : ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayCars(cars);
                        break;
                    case 2:
                        DisplayCars(cars.OrderBy(car => car.Year).ToList());
                        break;
                    case 3:
                        Console.Write("Entrez l'année de construction maximale : ");
                        int year = int.Parse(Console.ReadLine());
                        DisplayCars(cars.Where(car => car.Year < year).ToList());
                        break;
                    case 4:
                        Console.Write("Entrez le constructeur dont vous souhaitez afficher les voitures : ");
                        string constructor = Console.ReadLine();
                        DisplayCars(cars.Where(car => car.Constructor == constructor).ToList());
                        break;
                    case 5:
                        Console.Write("Entrez le modèle que vous recherchez : ");
                        string model = Console.ReadLine();
                        DisplayCars(cars.Where(car => car.Model == model).ToList());
                        break;
                    case 6:
                        Console.WriteLine("1 - Depuis la liste de base ");
                        Console.WriteLine("2 - Depuis un fichier XML");
                        int sourceChoice = int.Parse(Console.ReadLine());
                        switch (sourceChoice)
                        {
                            case 1:
                                DisplayCars(cars.Where(car => car.IsElectric == true).ToList());
                                break;
                            case 2:
                                DisplayCarsFromXML();
                                break;
                        }
                        break;
                    case 7:
                        XElement xml = CarsToXml(cars);
                        Console.WriteLine("\nXML Généré:");
                        Console.WriteLine(xml);
                        break;
                    case 10:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Option non valide, essayez à nouveau.");
                        break;
                }
            }
        }

        static List<Car> InitializeCars()
        {
            return new List<Car>
            {
                new Car(1, "Tesla", "Model S", 2020, true),
                new Car(2, "Ford", "Mustang", 2021, false),
                new Car(3, "Nissan", "Leaf", 2019, true),
                new Car(4, "Chevrolet", "Volt", 2018, true),
                new Car(5, "Toyota", "Camry", 2022, false),
                new Car(6, "Volkswagen", "Golf", 2020, false),
                new Car(7, "Renault", "Clio", 2021, false),
                new Car(8, "Peugeot", "208", 2019, false),
                new Car(9, "BMW", "i3", 2021, true),
                new Car(10, "Audi", "e-tron", 2020, true),
                new Car(11, "Hyundai", "Kona Electric", 2021, true),
                new Car(12, "Kia", "Soul EV", 2020, true),
                new Car(13, "Jaguar", "I-PACE", 2019, true),
                new Car(14, "Honda", "Civic", 2022, false),
                new Car(15, "Mazda", "CX-5", 2021, false),
                new Car(16, "Volkswagen", "Polo", 2017, false)
            };
        }

        static void DisplayCars(List<Car> cars)
        {
            if (cars.Count > 0)
            {
                foreach (var car in cars)
                {
                    Console.WriteLine($"Id: {car.Id}, Année de construction: {car.Year}, Constructeurr: {car.Constructor}, Modèle: {car.Model}, Eléctrique: {car.IsElectric}");
                }
            }
            else
            {
                Console.WriteLine("Aucune voiture ne correspond à la recherche.");
            }
        }

        static void DisplayCarsFromXML()
        {
            var xml = XDocument.Load("../../../Cars.xml");

            var electricCars = from car in xml.Descendants("Car")
                               where (bool)car.Element("IsElectric")
                               select new
                               {
                                   ID = (int)car.Element("ID"),
                                   Constructor = (string)car.Element("Constructor"),
                                   Model = (string)car.Element("Model"),
                                   Year = (int)car.Element("Year"),
                                   IsElectric = (bool)car.Element("IsElectric")
                               };

            foreach (var car in electricCars)
            {
                Console.WriteLine($"ID: {car.ID}, Constructor: {car.Constructor}, Model: {car.Model}, Year: {car.Year}, Electric: {car.IsElectric}");
            }
        }

        static XElement CarsToXml(List<Car> cars)
        {
            XElement xmlCars = new XElement("Cars",
                cars.Select(car => new XElement("Car",
                    new XElement("ID", car.Id),
                    new XElement("Constructor", car.Constructor),
                    new XElement("Model", car.Model),
                    new XElement("Year", car.Year),
                    new XElement("IsElectric", car.IsElectric)
                ))
            );

            return xmlCars;
        }
    }
}
