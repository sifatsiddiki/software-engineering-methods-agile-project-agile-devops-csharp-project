using MySql.Data.MySqlClient;
using System;

public class Program
{
    // Main method that connects to MySQL and fetches reports
    public static void Main(string[] args)
    {
        try
        {
            // Connection string for MySQL server 
            string connectionString = "Server=localhost; database=world; user=root; password=;";

            // Establish connection to MySQL database
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Display welcome message
                Console.WriteLine("Connected to the MySQL database!");

                // Calling different report generation methods
                GetCountriesByPopulation(connection);                 // Report 1: Countries ordered by population
                GetTopPopulatedCountries(connection, 5);              // Report 2: Top 5 populated countries
                GetLanguageStatistics(connection);                    // Report 3: Language statistics
                GetPopulationBreakdown(connection);                   // Report 4: Population breakdown by city vs non-city
            }
        }
        catch (Exception ex)
        {
            // Catch and display any errors that occur
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    // Report 1: Fetch countries ordered by population in descending order
    public static void GetCountriesByPopulation(MySqlConnection connection)
    {
        Console.WriteLine("\n--- Countries ordered by Population ---");
        string query = "SELECT Name, Population FROM country ORDER BY Population DESC;";

        // Execute query and display results
        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]} - Population: {reader["Population"]}");
                }
            }
        }
    }

    // Report 2: Fetch top N populated countries, where N is provided by the user
    public static void GetTopPopulatedCountries(MySqlConnection connection, int N)
    {
        Console.WriteLine($"\n--- Top {N} Populated Countries ---");
        string query = "SELECT Name, Population FROM country ORDER BY Population DESC LIMIT @N;";

        // Execute query with parameter N
        using (var cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@N", N);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]} - Population: {reader["Population"]}");
                }
            }
        }
    }

    // Report 3: Language statistics for languages such as Chinese, English, Hindi, Spanish, and Arabic
    public static void GetLanguageStatistics(MySqlConnection connection)
    {
        Console.WriteLine("\n--- Language Statistics ---");
        string query = @"
            SELECT language, SUM(population) AS Population
            FROM countrylanguage 
            JOIN country ON country.countryCode = countrylanguage.countryCode
            WHERE language IN ('Chinese', 'English', 'Hindi', 'Spanish', 'Arabic')
            GROUP BY language
            ORDER BY Population DESC;";

        // Execute query and display results
        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["language"]} - Population: {reader["Population"]}");
                }
            }
        }
    }

    // Report 4: Population breakdown by city vs non-city (cities in the continent, region, and country)
    public static void GetPopulationBreakdown(MySqlConnection connection)
    {
        Console.WriteLine("\n--- Population Breakdown by City vs Non-City ---");
        string query = @"
            SELECT continent, 
                   SUM(CASE WHEN city.Population > 0 THEN city.Population ELSE 0 END) AS CityPopulation,
                   SUM(CASE WHEN city.Population = 0 THEN country.Population ELSE 0 END) AS NonCityPopulation
            FROM country
            LEFT JOIN city ON country.Code = city.CountryCode
            GROUP BY continent;";

        // Execute query and display results
        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["continent"]} - City Population: {reader["CityPopulation"]}, Non-City Population: {reader["NonCityPopulation"]}");
                }
            }
        }
    }
}

