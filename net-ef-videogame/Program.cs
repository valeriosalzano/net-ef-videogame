using net_ef_videogame.Models;
using System.Globalization;
using ConsoleTables;
using net_ef_videogame.Database;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace net_ef_videogame
{
    public class Program
    {
        static void Main(string[] args)
        {

            DisplayConsoleOutput("PROGRAM START");

            string userChoice;
            do
            {
                var menu = new ConsoleTable(" MENU ");

                menu.AddRow("1 - Insert a new video game.");
                menu.AddRow("2 - Search for a video game by ID.");
                menu.AddRow("3 - Search for all games with the name containing a certain string.");
                menu.AddRow("4 - Delete a video game.");
                menu.AddRow("5 - Insert a new software house.");
                menu.AddRow("6 - Search videogames by Software House ID.");
                menu.AddRow("0 - Close the program");

                Console.WriteLine("");
                menu.Write();
                Console.WriteLine("");

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
                    case "6":
                        FindVideogamesBySoftwareHouseId();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid key.");
                        break;
                }
            } while (userChoice != "0");

            DisplayConsoleOutput("PROGRAM END");
        }

        // MENU OPTIONS
        public static void InsertVideogame()
        {
            DisplayConsoleOutput("Inserting a new videogame");

            Console.Write("Enter the videogame name: ");
            string videogameName = GetValidStringFromUser();

            string videogameOverview = GetOptionalParameterFromUser("overview");

            Console.Write("Enter the videogame release date (dd/mm/yyyy): ");
            DateTime videogameReleaseDate = GetValidDateFromUser();

            List<SoftwareHouse> shList = GetSoftwareHousesList();

            var shMenu = new ConsoleTable("Software Houses List");
            foreach (SoftwareHouse house in shList)
                shMenu.AddRow($"{house.SoftwareHouseId} - {house.Name}");
            Console.WriteLine();
            shMenu.Write(Format.Minimal);

            Console.Write("Enter the software house id: ");
            int videogameSHId = GetValidPositiveIntegerFromUser();
            while (videogameSHId > shList.Count)
            {
                Console.Write("Insert a valid software house id: ");
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
                bool videogameInserted = false;
                try
                {
                    db.Add(newVideogame);
                    db.SaveChanges();
                    videogameInserted = true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    videogameInserted = false;
                }
                finally
                {
                    DisplayConsoleOutput(videogameInserted ? "Success! Videogame added." : "Error! Something went wrong.");
                }
            }
        }
        public static void FindVideogameById()
        {
            DisplayConsoleOutput("Finding a videogame by ID");

            Console.Write("Enter the videogame ID: ");
            int videogameId = GetValidPositiveIntegerFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame foundVideogame = db.Videogames.Where(v => v.VideogameId == videogameId).First();
                    DisplayConsoleOutput("Success! Videogame found.");
                    Console.WriteLine("\t" + foundVideogame);
                }
                catch (Exception ex)
                {
                    DisplayConsoleOutput("Videogame not found");
                }
            }

        }
        public static void FindVideogameByName()
        {
            DisplayConsoleOutput("Finding a videogame by Name");

            Console.Write("Enter the videogame name: ");
            string videogameName = GetValidStringFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    List<Videogame> foundVideogames = db.Videogames.Where(videogame => videogame.Name.Contains(videogameName)).ToList<Videogame>();

                    if (foundVideogames.Count > 0)
                    {
                        DisplayConsoleOutput($"Success! Videogame{(foundVideogames.Count > 1 ? "s":"")} found");

                        foreach (Videogame videogame in foundVideogames)
                            Console.WriteLine($"\t- {videogame}");
                    }
                    else
                        DisplayConsoleOutput("Videogame not found");
                }
                catch (Exception ex)
                {
                    DisplayConsoleOutput("Error! Something went wrong");
                    //Console.WriteLine(ex.Message);
                }
            }
        }
        public static void DeleteVideogame()
        {
            DisplayConsoleOutput("Deleting a videogame by ID");

            Console.Write("Enter the videogame ID: ");
            int videogameId = GetValidPositiveIntegerFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame foundVideogame = db.Videogames.Where(v => v.VideogameId == videogameId).First();
                    try
                    {
                        db.Videogames.Remove(foundVideogame);
                        db.SaveChanges();
                        DisplayConsoleOutput("Success! Videogame deleted");
                    }
                    catch (Exception ex)
                    {
                        DisplayConsoleOutput("Cannot delete the videogame");
                        //Console.WriteLine(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    DisplayConsoleOutput("Videogame not found");
                    //Console.WriteLine(ex.Message);
                }
            }
        }
        public static void InsertSoftwareHouse()
        {
            DisplayConsoleOutput("Inserting a new software house");

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
                    //Console.WriteLine(ex.Message);
                    inserted = false;
                }
                finally
                {
                    DisplayConsoleOutput(inserted ? "Success! Software house added" : "Error! Something went wrong");
                }
            }
        }
        public static void FindVideogamesBySoftwareHouseId()
        {
            DisplayConsoleOutput("Finding videogames by Software House ID");

            Console.Write("Enter the software house id: ");
            int softwareHouseId = GetValidPositiveIntegerFromUser();

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    SoftwareHouse foundSoftwareHouse = db.SoftwareHouses.Where(sh => sh.SoftwareHouseId == softwareHouseId).Include(sh => sh.Videogames).First();
                    if (foundSoftwareHouse.Videogames.Count > 0)
                    {
                        Console.WriteLine($"Here is the list of videogames published by {foundSoftwareHouse.Name}:");
                        foreach (Videogame videogame in foundSoftwareHouse.Videogames)
                                Console.WriteLine($"\t- {videogame}");
                    }
                    else
                        DisplayConsoleOutput("This software house hasn't released a videogame yet.");
                }
                catch (Exception ex)
                {
                    DisplayConsoleOutput("Error! Software house not found.");
                    //Console.WriteLine(ex.Message);
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
                    Console.WriteLine("Error! Can't get the software houses list.");
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
                Console.Write("Insert a valid date format (dd/mm/yyyy): ");
            }
            return userInput;
        }
        public static int GetValidPositiveIntegerFromUser()
        {
            int userInput;
            while (!int.TryParse(Console.ReadLine(), out userInput) || userInput <= 0)
                Console.Write("Insert a positive number: ");
            return userInput;
        }
        public static string GetOptionalParameterFromUser(string parameterName)
        {
            string outputString = "";
            bool isInputValid = false;
            while(!isInputValid)
            {
                Console.Write($"Do you want to set the {parameterName}? (y/n) ");
                string userChoice = Console.ReadLine() ?? "" ;

                switch (userChoice.ToLower().Trim())
                {
                    case "yes":
                    case "y":
                        Console.Write($"Enter the {parameterName}: ");
                        outputString = GetValidStringFromUser();
                        isInputValid = true; 
                        break;
                    case "":
                    case "n":
                    case "no":
                        isInputValid = true;
                        break;
                    default:
                        Console.WriteLine("Invalid key.");
                        isInputValid = false;
                        break;
                }

            }
            return outputString;
        }

        // CONSOLE OUTPUT FUNCTIONS
        public static void DisplayConsoleOutput(string message)
        {
            Console.WriteLine($"\n --- {message} --- \n");
        }

        // SEEDER
        public static void Seeder()
        {
            List<SoftwareHouse> shList = new List<SoftwareHouse>();
            shList.Add( new SoftwareHouse()
            {
                Name = "Nintendo",
                TaxId = "ME-697-14-7528-0",
                City = "Kyoto",
                Country = "Japan"
            });
            shList.Add(new SoftwareHouse()
            {
                Name = "Valve Corporation",
                TaxId = "UT-277-92-7542-2",
                City = "Bellevue",
                Country = "United States"
            });
            shList.Add(new SoftwareHouse()
            {
                Name = "Rockstar Games",
                TaxId = "GA-160-16-9503-1",
                City = "New York City",
                Country = "United States"
            });
            shList.Add(new SoftwareHouse()
            {
                Name = "Electronic Arts",
                TaxId = "SD-032-99-9226-3",
                City = "Redwood City",
                Country = "United States"
            });
            shList.Add(new SoftwareHouse()
            {
                Name = "Ubisoft",
                TaxId = "NC-134-01-6528-4",
                City = "Montreuil",
                Country = "France"
            });
            shList.Add(new SoftwareHouse()
            {
                Name = "Konami",
                TaxId = "ID-418-30-7570-5",
                City = "Kyoto",
                Country = "Japan"
            });



            List<Videogame> vgList = new List<Videogame>();

            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;

            for(int i = 0; i < 30; i++)
            {
                vgList.Add(new Videogame()
                {
                    Name = "videogame" + (i + 1),
                    Overview = "Lorem ipsum",
                    ReleaseDate = start.AddDays(Random.Shared.Next(range)),
                    SoftwareHouseId = Random.Shared.Next(1, shList.Count + 1)
                });
            }

            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    foreach (SoftwareHouse sh in shList)
                    {
                        db.Add(sh);
                        db.SaveChanges();
                    }
                    foreach (Videogame vg in vgList)
                    {
                        db.Add(vg);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }


        }
    }
}