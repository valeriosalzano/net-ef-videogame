using net_ef_videogame.Models;
using System.Globalization;
using ConsoleTables;
using net_ef_videogame.Database;

namespace net_ef_videogame
{
    public class Program
    {
        static void Main(string[] args)
        {
            string userChoice;
            do
            {
                var menu = new ConsoleTable("                   MENU");

                menu.AddRow("1 - Insert a new video game.");
                menu.AddRow("2 - Search for a video game by ID.");
                menu.AddRow("3 - Search for all games with the name containing a certain string.");
                menu.AddRow("4 - Delete a video game.");
                menu.AddRow("5 - Insert a new software house.");
                menu.AddRow("0 - Close the program");

                Console.WriteLine("\n");
                menu.Write();
                Console.WriteLine("\n");

                Console.Write("Press the desidered command key: ");
                userChoice = Console.ReadLine() ?? "";

                switch (userChoice)
                {
                    case "1":
                        InsertVideogame();
                        break;
                    case "2":
                        FindVideogameById();
                        break;
                    case "3":
                        FindVideogameByName();
                        break;
                    case "4":
                        DeleteVideogame();
                        break;
                    case "5":
                        InsertSoftwareHouse();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid key.");
                        break;

                }
            } while (userChoice != "0");

            Console.WriteLine("\n --- Program END --- \n");
        }

        public static void InsertVideogame()
        {
            Console.WriteLine("\n--- Inserting a new videogame ---\n");

            Console.Write("Enter the videogame name: ");
            string videogameName = GetValidStringFromUser();

            string videogameOverview = GetOptionalParameterFromUser("overview");

            Console.Write("Enter the videogame release date (dd/mm/yyyy): ");
            DateTime videogameReleaseDate = GetValidDateFromUser();

            List<SoftwareHouse> shList = GetSoftwareHousesList();
            var shMenu = new ConsoleTable(" --- Choose Software House --- ");
            foreach (SoftwareHouse house in shList)
            {
                shMenu.AddRow($"{house.SoftwareHouseId} - {house.Name}");
            }

            Console.WriteLine("\n");
            shMenu.Write();
            Console.WriteLine("\n");

            Console.Write("Enter the software house id: ");
            int videogameSHId = GetValidPositiveIntegerFromUser(); ;
            while (videogameSHId > shList.Count())
            {
                Console.Write("Id out of range. Insert a valid value: ");
                videogameSHId = GetValidPositiveIntegerFromUser();
            } 
            
            Videogame newVideogame = new Videogame()
            {
                Name = videogameName,
                Overview = videogameOverview,
                ReleaseDate = videogameReleaseDate,
                SoftwareHouseId = videogameSHId
            };

            using (VideogameContext db = new VideogameContext())
            {
                bool inserted = false;
                try
                {
                    db.Add(newVideogame);
                    db.SaveChanges();
                    inserted = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    inserted = false;
                }
                finally
                {
                    Console.WriteLine(inserted ? "Success! Videogame added." : "Error! Something went wrong.");
                }
            }
        }

        public static void FindVideogameById()
        {
            Console.WriteLine("\n--- Finding a videogame by ID ---\n");

            Console.Write("Enter the videogame ID: ");
            int videogameId = GetValidPositiveIntegerFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame? foundVideogame = db.Videogames.Where(v => v.VideogameId == videogameId).First();
                    Console.WriteLine("Success! Videogame found. " + foundVideogame);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Videogame not found.");
                }
            }

        }
        public static void FindVideogameByName()
        {
            Console.WriteLine("\n--- Finding a videogame by Name ---\n");

            Console.Write("Enter the videogame name: ");
            string videogameName = GetValidStringFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    List<Videogame> foundVideogames = db.Videogames.Where(videogame => videogame.Name.Contains(videogameName)).ToList<Videogame>();

                    if (foundVideogames.Count() > 0)
                        foreach (Videogame videogame in foundVideogames)
                            Console.WriteLine($"- {videogame}");
                    else
                        Console.WriteLine("Videogame not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! Something went wrong.");
                    //Console.WriteLine(ex.Message);
                }
            }


        }
        public static void DeleteVideogame()
        {
            Console.WriteLine("\n--- Deleting a videogame by ID ---\n");

            Console.Write("Enter the videogame ID: ");
            int videogameId = GetValidPositiveIntegerFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame? foundVideogame = db.Videogames.Where(v => v.VideogameId == videogameId).First();

                    try
                    {
                        db.Videogames.Remove(foundVideogame);
                        db.SaveChanges();
                        Console.WriteLine("Success! Videogame deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot delete the videogame.");
                        //Console.WriteLine(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot find the videogame.");
                    //Console.WriteLine(ex.Message);
                }
            }
        }
        public static void InsertSoftwareHouse()
        {
            Console.WriteLine("\n--- Inserting a new software house ---\n");

            Console.Write("Enter the software house name: ");
            string softwareHouseName = GetValidStringFromUser();

            Console.Write("Enter the software house VAT: ");
            string softwareHouseTaxId = GetValidStringFromUser();

            Console.Write("Enter the software house city: ");
            string softwareHouseCity = GetValidStringFromUser();

            Console.Write("Enter the software house country: ");
            string softwareHouseCountry = GetValidStringFromUser();

            SoftwareHouse newSoftwareHouse = new SoftwareHouse()
            {
                Name = softwareHouseName,
                TaxId = softwareHouseTaxId,
                City = softwareHouseCity,
                Country = softwareHouseCountry
            };

            using (VideogameContext db = new VideogameContext())
            {
                bool inserted = false;
                try
                {
                    db.Add(newSoftwareHouse);
                    db.SaveChanges();
                    inserted = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    inserted = false;
                }
                finally
                {
                    Console.WriteLine(inserted ? "Success! Software house added." : "Error! Something went wrong.");
                }
            }
        }

        // UTILITIES

        public static List<SoftwareHouse> GetSoftwareHousesList()
        {
            List<SoftwareHouse> softwareHousesList = new List<SoftwareHouse>();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    softwareHousesList = db.SoftwareHouses.ToList<SoftwareHouse>();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            }

            return softwareHousesList;
        }

        // USER INPUT FUNCTIONS
        public static string GetValidStringFromUser()
        {
            string? userInput = Console.ReadLine();
            while (string.IsNullOrEmpty(userInput) || userInput.Trim() == "")
            {
                Console.Write("Insert a valid value: ");
                userInput = Console.ReadLine();
            }
            return userInput;
        }
        public static DateTime GetValidDateFromUser()
        {
            DateTime userInput;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out userInput))
            {
                Console.Write("Insert a valid date format: ");
            }
            return userInput;
        }
        public static int GetValidPositiveIntegerFromUser()
        {
            int userInput;
            while (!int.TryParse(Console.ReadLine(), out userInput) || userInput <= 0)
            {
                Console.Write("Insert a positive number: ");
            }
            return userInput;
        }
        public static string GetOptionalParameterFromUser(string parameterName)
        {
            string outputString = "";
            string userChoice = "";
            while (userChoice != "n")
            {
                Console.Write($"Do you want to set the {parameterName}? (y/n) ");
                userChoice = GetValidStringFromUser().ToLower();

                if (userChoice == "y")
                {
                    Console.Write($"Enter the {parameterName}: ");
                    outputString = GetValidStringFromUser();
                }
                else if (userChoice == "n")
                {
                    Console.WriteLine("Fine.");
                }
                else
                    Console.WriteLine("Invalid key.");
            }
            return outputString;
        }
    }
}