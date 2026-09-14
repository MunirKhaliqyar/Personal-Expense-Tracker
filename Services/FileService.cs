using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;

namespace Personal_Expense_Tracker.Services
{
    internal class FileService
    {
        // Fields
        private readonly string filePath; 
        
        // Constructor
        public FileService()
        {
            string projectDirecoty = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName;
            string dataDirectory = Path.Combine(projectDirecoty, "Data");
            Directory.CreateDirectory(dataDirectory);                   // Create the Data directory if it doesn't exist
            filePath = Path.Combine(dataDirectory, "expenses.json");    // Create the file path for expenses.json
            
        }

        // Methods

        // Save all expense to a file
        public void SaveExpenses(List<Expense> expenses)
        {
            if (!(expenses == null))
            {
                JsonSerializerOptions option = new JsonSerializerOptions { WriteIndented = true };

                option.Converters.Add(new JsonStringEnumConverter());       // Add converter for enum serialization

                string json = JsonSerializer.Serialize(expenses, option);

                File.WriteAllText(filePath, json);
            }
        }

        // Load expenses from a file
        public List<Expense> LoadExpenses()
        {
            if (FileExists())
            {
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Expense>();
                }

                JsonSerializerOptions option = new JsonSerializerOptions();
                option.Converters.Add(new JsonStringEnumConverter());       // Add converter for enum serialization

                List<Expense> expenses = JsonSerializer.Deserialize<List<Expense>>(json, option);
                return expenses ?? new List<Expense>();
            }
            else
            {
                return new List<Expense>();
            }
        }

        // Check if the data file exists
        public bool FileExists()
        {
            return File.Exists(filePath);
        }
    }
}
