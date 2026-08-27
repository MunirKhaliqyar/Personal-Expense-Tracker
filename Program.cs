using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Personal_Expense_Tracker.Models;
using Personal_Expense_Tracker.Services;

namespace Personal_Expense_Tracker
{
    internal class Program
    {
        private static ExpenseService _expenseService = new ExpenseService();
        private static bool exit = false;
        // The main method
        static void Main(string[] args)
        {
            DisplayMenu();
        }
        
        // Display the menu
        static void DisplayMenu()
        { 
            string title = "PERSONAL EXPENSE TRACKER";

            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine('|' + title.PadLeft(((Console.WindowWidth - 2 ) + title.Length) / 2).PadRight((Console.WindowWidth - 2 )) + '|');
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine('|' + " 1. Add an Expense".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 2. Edit and Expense".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 3. Delete and Expense".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 4. View All Expenses".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 5. View Total number of Expenses".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 6. Search by Description".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 7. Search by Date Range".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 8. Search by Category".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 9. Show Monthly Totals".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + "10. Show Yearly Totals".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + "11. Show Total of All Expenses".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 0. Exit".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine(new string('-', Console.WindowWidth));

            while (true)
            {
                try
                {
                    Console.Write("Select an option: ");
                    string choice = Console.ReadLine();

                    // Check if choice is null or empty
                    if (string.IsNullOrEmpty(choice))
                    {
                        Console.WriteLine("Please enter a number between 0 - 11");
                        continue;
                    }

                    if (int.Parse(choice) >= 0 && int.Parse(choice) <= 11)
                    {
                        HandleMenuChoice(choice);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number between 0 - 11");
                        continue;
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a number between 0 - 11");
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                    
                }
            }
        }

        // Handles menu choice
        static void HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1": AddExpense(); break;
                case "2": EditExpense(); break;
                case "3": DeleteExpense(); break;
                case "4": ViewAllExpenses(); break;
                case "5": ViewTotalNumberOfExpenses(); break;
                case "6": SearchByDescription(); break;
                case "7": SearchByDateRange(); break;
                case "8": SearchByCategory(); break;
                case "9": ShowMonthlyTotals(); break;
                case "10": ShowYearlyTotals(); break;
                case "11": ShowTotalAllExpense(); break;
                case "0": ExitApplication(); break;
                default:
                    Console.WriteLine("\nInvalid option! Press any key to continue...");
                    Console.ReadKey();
                    break;


            }
        }
        // Display the "Add an expense" option
        static void AddExpense()
        {
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("-----ADD NEW EXPENSE-----");
                Console.WriteLine("\n(Press 'R' to return to menu, 'Esc' to exit at any time)\n");

                try
                {
                    // Get Descriptin
                    string description;
                    while (true)
                    {
                        Console.Write("Description: ");
                        description = Console.ReadLine();
                        CheckForExitOrReturn(description);

                        if (string.IsNullOrWhiteSpace(description)) 
                        {
                            Console.WriteLine("Description cannot be empty!");
                            continue;
                        }
                        break;
                    }

                    // Get Amount
                    decimal amount;
                    while (true)
                    {
                        Console.Write("Amount: ");
                        string amountInput = Console.ReadLine();
                        CheckForExitOrReturn(amountInput);

                        if (!decimal.TryParse(amountInput, out amount) || amount <= 0)
                        {
                            Console.WriteLine("Please enter a valid positive amount");
                            continue;
                        }
                        break;
                    }

                    DateTime date;
                    while (true)
                    {
                        // Get Date
                        Console.Write("Date(yyyy-mm-dd, press Enter for today): ");
                        string dateInput = Console.ReadLine();
                        CheckForExitOrReturn(dateInput);

                        if (string.IsNullOrEmpty(dateInput)) 
                        {
                            date = DateTime.Now;
                            break;
                        }
                        else if(!DateTime.TryParse(dateInput, out date))
                        {
                            Console.WriteLine("Invalid date format!");
                            continue;
                        }
                        break;
                    }

                    Category category;
                    while (true)
                    {
                        // Get category
                        Console.WriteLine("Category types: Food, Transportation, Utilities, Entertainment, Shopping, Healthcare, Rent, Other");
                        Console.Write("Category: ");
                        string categoryInput = Console.ReadLine();
                        CheckForExitOrReturn(categoryInput);

                        if(!Enum.TryParse<Category>(categoryInput, true, out category))
                        {
                            Console.WriteLine("Invalid category! Using 'Other' as default.");
                            category = Category.Other;
                        }
                        break;
                    }

                    // Create and add expense
                    Expense expense = new Expense(description, amount, date, category);
                    _expenseService.AddExpense(expense);
                    Console.WriteLine($"\nExpense added successfully! ID: {expense.Id.ToString()}");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    var key = Console.ReadKey(true).Key;
                    CheckForExitOrReturn(key.ToString());
                    
                    continue;       // Continue adding expenses

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Edit an expense" option
        static void EditExpense()
        {

        }

        // Display the "Delete an expense" option
        static void DeleteExpense()
        {

        }
        // Display the "All expenses" option
        static void ViewAllExpenses() 
        {
            
        }
        // Display the "Total number of expenses" option
        static void ViewTotalNumberOfExpenses()
        {

        }
        // Display the "Expenses by description" option
        static void SearchByDescription()
        {

        }
        // Display the "Expenses by date range" option
        static void SearchByDateRange()
        {

        }
        // Display the "Search expenses by category" option
        static void SearchByCategory()
        {

        }
        // Display the "Show monthly totals" option
        static void ShowMonthlyTotals()
        {

        }
        // Display the "Show yearly totals" option
        static void ShowYearlyTotals()
        {

        }
        // Display the "Show total of all expenses" option
        static void ShowTotalAllExpense()
        {

        }

        // Display the "Exit" option
        static void ExitApplication()
        {
            exit = true;
            string goodByeMessage = "THANK YOU FOR USING THE";
            string expenseTrackerApp = "EXPENSE TRACKER APP!";
            string goodBye = "Goodbye!";
            Console.Clear();
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine('|' + goodByeMessage.PadLeft(((Console.WindowWidth - 2) + goodByeMessage.Length) / 2).PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + expenseTrackerApp.PadLeft(((Console.WindowWidth - 2) + expenseTrackerApp.Length) / 2).PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + goodBye.PadLeft(((Console.WindowWidth - 2) + goodBye.Length) / 2).PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine(new string('-', Console.WindowWidth));
            Environment.Exit(0);
        }

        // Helper function for returning to main menu or exiting the program at anytime
        static void CheckForExitOrReturn(string input)
        {            
            input = input.Trim().ToUpper();
            if (input == "ESC" || input == "EXIT")
            {
                // Exit the application
                ExitApplication();
            } else if (input == "R")
            {
                // Return to main menu
                DisplayMenu();
            }
        }
    }
}
