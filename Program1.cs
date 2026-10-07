using System;
using MySql.Data.MySqlClient;

namespace PopulationReportApp
{
    class Program
    {
        // MySQL connection string
        static string connectionString = "Server=localhost;Database=PopulationReport;user=root;password=;";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Population Report System!");

            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add a Country");
                Console.WriteLine("2. Add a City");
                Console.WriteLine("3. Add a Capital City");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCountry();
                        break;
                    case "2":
                        AddCity();
                        break;
                    case "3":
                        AddCapitalCity();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Method to add a new country
        static void AddCountry()
        {
            Console.Write("\nEnter the country name: ");
            string countryName = Console.ReadLine();

            Console.Write("Enter the continent: ");
            string continent = Console.ReadLine();

            Console.Write("Enter the region: ");
            string region = Console.ReadLine();

            Console.Write("Enter the population: ");
            int population = int.Parse(Console.ReadLine());

            Console.Write("Enter the capital city: ");
            string capital = Console.ReadLine();

            // SQL command to insert the country into the database
            string query = "INSERT INTO Countries (Name, Continent, Region, Population, Capital) VALUES (@name, @continent, @region, @population, @capital)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", countryName);
                command.Parameters.AddWithValue("@continent", continent);
                command.Parameters.AddWithValue("@region", region);
                command.Parameters.AddWithValue("@population", population);
                command.Parameters.AddWithValue("@capital", capital);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Country added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Method to add a new city
        static void AddCity()
        {
            Console.Write("\nEnter the city name: ");
            string cityName = Console.ReadLine();

            Console.Write("Enter the country name: ");
            string countryName = Console.ReadLine();

            // Get the country ID for the city
            int countryId = GetCountryIdByName(countryName);

            if (countryId == -1)
            {
                Console.WriteLine("Country not found.");
                return;
            }

            Console.Write("Enter the district: ");
            string district = Console.ReadLine();

            Console.Write("Enter the population: ");
            int population = int.Parse(Console.ReadLine());

            // SQL command to insert the city into the database
            string query = "INSERT INTO Cities (Name, CountryID, District, Population) VALUES (@name, @countryId, @district, @population)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", cityName);
                command.Parameters.AddWithValue("@countryId", countryId);
                command.Parameters.AddWithValue("@district", district);
                command.Parameters.AddWithValue("@population", population);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("City added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Method to add a new capital city
        static void AddCapitalCity()
        {
            Console.Write("\nEnter the capital city name: ");
            string capitalName = Console.ReadLine();

            Console.Write("Enter the country name: ");
            string countryName = Console.ReadLine();

            // Get the country ID for the capital city
            int countryId = GetCountryIdByName(countryName);

            if (countryId == -1)
            {
                Console.WriteLine("Country not found.");
                return;
            }

            Console.Write("Enter the population: ");
            int population = int.Parse(Console.ReadLine());

            // SQL command to insert the capital city into the database
            string query = "INSERT INTO CapitalCities (Name, CountryID, Population) VALUES (@name, @countryId, @population)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", capitalName);
                command.Parameters.AddWithValue("@countryId", countryId);
                command.Parameters.AddWithValue("@population", population);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Capital city added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Helper method to get the CountryID by name
        static int GetCountryIdByName(string countryName)
        {
            string query = "SELECT CountryID FROM Countries WHERE Name = @name";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", countryName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return -1;
                }
            }
        }
    }
}
