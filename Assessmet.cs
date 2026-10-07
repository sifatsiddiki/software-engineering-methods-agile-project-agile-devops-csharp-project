using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

class PopulationReportSystem
{
    static string connectionString = "Server=localhost;Database=world;Uid=root;Pwd=password"; // Replace with your credentials

    static void Main()
    {
        Console.WriteLine("Welcome to the Population Report System");
        Console.WriteLine("Please choose a report type:");
        Console.WriteLine("1. Countries by population");
        Console.WriteLine("2. Cities by population");
        Console.WriteLine("3. Capital Cities by population");
        Console.WriteLine("4. Top N populated countries");
        Console.WriteLine("5. Language statistics");
        Console.WriteLine("6. Population breakdown by continent/region/country/city");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                DisplayCountriesByPopulation();
                break;
            case 2:
                DisplayCitiesByPopulation();
                break;
            case 3:
                DisplayCapitalCitiesByPopulation();
                break;
            case 4:
                DisplayTopNPopulatedCountries();
                break;
            case 5:
                DisplayLanguageStatistics();
                break;
            case 6:
                DisplayPopulationBreakdown();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    // 1. Countries organized by population
    static void DisplayCountriesByPopulation()
    {
        string query = "SELECT Name, Population FROM country ORDER BY Population DESC";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // 2. Cities organized by population
    static void DisplayCitiesByPopulation()
    {
        string query = "SELECT Name, Population FROM city ORDER BY Population DESC";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // 3. Capital cities organized by population
    static void DisplayCapitalCitiesByPopulation()
    {
        string query = "SELECT city.Name, country.Name AS Country, city.Population FROM city JOIN country ON city.ID = country.Capital ORDER BY city.Population DESC";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // 4. Top N populated countries (Input N from the user)
    static void DisplayTopNPopulatedCountries()
    {
        Console.WriteLine("Enter the value of N:");
        int N = int.Parse(Console.ReadLine());
        string query = $"SELECT Name, Population FROM country ORDER BY Population DESC LIMIT {N}";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // 5. Language speaker’s statistics
    static void DisplayLanguageStatistics()
    {
        string query = "SELECT Language, SUM(Speakers) AS TotalSpeakers FROM country_language GROUP BY Language ORDER BY TotalSpeakers DESC";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // 6. Population breakdown by continent/region/country/city
    static void DisplayPopulationBreakdown()
    {
        string query = "SELECT Continent, SUM(Population) AS TotalPopulation FROM country GROUP BY Continent";

        var data = ExecuteQuery(query);
        DisplayData(data);
    }

    // Execute the SQL query and return the result as a DataTable
    static DataTable ExecuteQuery(string query)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }
    }

    // Display the data in a formatted way
    static void DisplayData(DataTable data)
    {
        foreach (DataColumn column in data.Columns)
        {
            Console.Write(column.ColumnName + "\t");
        }
        Console.WriteLine();

        foreach (DataRow row in data.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                Console.Write(item + "\t");
            }
            Console.WriteLine();
        }
    }
}
