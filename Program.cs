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

        // The main method
        static void Main(string[] args)
        {
            DisplayMenu();
        }

        // Display the menu
        static void DisplayMenu()
        {
            Console.Clear();
            string title = "PERSONAL EXPENSE TRACKER";

            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine('|' + title.PadLeft(((Console.WindowWidth - 2) + title.Length) / 2).PadRight((Console.WindowWidth - 2)) + '|');
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine('|' + " 1. Add an Expense".PadRight(Console.WindowWidth - 2) + '|');
            Console.WriteLine('|' + " 2. Edit an Expense".PadRight(Console.WindowWidth - 2) + '|');
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
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----ADD NEW EXPENSE-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    // Variables to hold user input
                    string description;
                    decimal amount;
                    DateTime date;
                    Category category;

                    // Get Descriptin
                    while (true)
                    {
                        Console.Write("Description: ");
                        description = Console.ReadLine();
                        if (CheckForExitOrReturn(description))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(description))
                        {
                            Console.WriteLine("Description cannot be empty!");
                            continue;
                        }
                        break;
                    }

                    // Get Amount
                    while (true)
                    {
                        Console.Write("Amount: ");
                        string amountInput = Console.ReadLine();
                        if (CheckForExitOrReturn(amountInput))
                        {
                            return;
                        }

                        if (!decimal.TryParse(amountInput, out amount) || amount <= 0)
                        {
                            Console.WriteLine("Please enter a valid positive amount");
                            continue;
                        }
                        break;
                    }

                    // Get Date
                    while (true)
                    {
                        Console.Write("Date(yyyy-mm-dd, press Enter for today): ");
                        string dateInput = Console.ReadLine();
                        if (CheckForExitOrReturn(dateInput))
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(dateInput))
                        {
                            date = DateTime.Now;
                            // The variable 'date' is already initialized with DateTime.Now, so we can just break here
                            break;
                        }
                        else if (!DateTime.TryParse(dateInput, out date))
                        {
                            Console.WriteLine("Invalid date format!");
                            continue;
                        }
                        break;
                    }

                    // Get category
                    while (true)
                    {
                        Console.WriteLine("Category types: Food, Transportation, Utilities, Entertainment, Shopping, Healthcare, Rent, Other");
                        Console.Write("Category: ");
                        string categoryInput = Console.ReadLine();
                        if (CheckForExitOrReturn(categoryInput))
                        {
                            return;
                        }

                        if (!Enum.TryParse<Category>(categoryInput, true, out category))
                        {
                            category = Category.Other; // Default to Other if invalid
                            Console.WriteLine("Invalid category! Using 'Other' as default.");
                        }
                        break;
                    }

                    // Create and add expense
                    Expense expense = new Expense(description, amount, date, category);
                    _expenseService.AddExpense(expense);
                    Console.WriteLine($"\nExpense added successfully! ID: {expense.Id.ToString()}");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
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
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----EDIT AN EXPENSE-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    if (_expenseService.GetExpenseCount() == 0)
                    {
                        Console.WriteLine("No expenses to edit!");
                        Console.WriteLine("\nPress any key to return to main menu.");
                        Console.ReadKey();
                        DisplayMenu();
                        return;
                    }

                    foreach (var expense in _expenseService.GetAllExpenses())
                    {
                        Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                    }

                    Console.Write("\nEnter the ID of the expense you want to edit: ");
                    string idInput = Console.ReadLine();
                    if (CheckForExitOrReturn(idInput))
                    {
                        return;
                    }

                    if (!Guid.TryParse(idInput, out Guid expenseId) || string.IsNullOrWhiteSpace(idInput))
                    {
                        Console.WriteLine("Invalid ID format!");
                        Console.ReadKey();
                        continue;
                    }
                    else if (!_expenseService.ExpenseExists(expenseId.ToString()))
                    {
                        Console.WriteLine("Expense with the given ID does not exist!");
                        Console.ReadKey();
                        continue;
                    }

                    Expense expenseToEdit = _expenseService.GetExpenseById(expenseId.ToString());

                    // Variables to hold user input
                    string newDescription;
                    decimal newAmount;
                    DateTime newDate;
                    Category newCategory;

                    // Get the expense to edit
                    Console.WriteLine("Enter new values for the expense (leave blank to keep current value):");

                    // Edit Descriptin
                    Console.WriteLine();
                    Console.WriteLine($"Current description: {expenseToEdit.Description}");
                    Console.Write("New description: ");
                    newDescription = Console.ReadLine();
                    if (CheckForExitOrReturn(newDescription))
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(newDescription))
                    {
                        newDescription = expenseToEdit.Description; // Keep current value
                    }

                    // Edit Amount
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current amount: {expenseToEdit.Amount:C}");
                        Console.Write("New amount: ");

                        string newAmountInput = Console.ReadLine();
                        if (CheckForExitOrReturn(newAmountInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newAmountInput))
                        {
                            newAmount = expenseToEdit.Amount; // Keep current value
                            break;
                        }else if (!decimal.TryParse(newAmountInput, out newAmount) || newAmount <= 0)
                        {
                            Console.WriteLine("Please enter a valid positive amount");
                            continue;
                        }
                        break;
                    }
                    // Edit Date
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current date: {expenseToEdit.Date.ToShortDateString()}");
                        Console.Write("Date(yyyy-mm-dd, press Enter for today): ");
                        string dateInput = Console.ReadLine();
                        if (CheckForExitOrReturn(dateInput))
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(dateInput))
                        {
                            newDate = expenseToEdit.Date;   // Keep current value
                            break;
                        }else if (!DateTime.TryParse(dateInput, out newDate))
                        {
                            Console.WriteLine("Invalid date format!");
                            continue;
                        } else if (newDate > DateTime.Now)
                        {
                            Console.WriteLine("Date cannot be in the future!");
                        }
                        break;
                    }

                    // Edit category
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Category types: Food, Transportation, Utilities, Entertainment, Shopping, Healthcare, Rent, Other");
                        Console.WriteLine($"Current category: {expenseToEdit.Category}");
                        Console.Write("New Category: ");
                        string categoryInput = Console.ReadLine();
                        if (CheckForExitOrReturn(categoryInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(categoryInput))
                        {
                            newCategory = expenseToEdit.Category; // Keep current value
                            break;
                        }else if (!Enum.TryParse<Category>(categoryInput, true, out newCategory))
                        {
                            Console.WriteLine("Invalid category! Please put one of the above categories.");
                            continue;
                        }
                        break;
                    }

                    // Edit the expense
                    Console.WriteLine();
                    _expenseService.EditExpense(expenseId.ToString(), newDescription, newAmount, newDate, newCategory);
                    Console.WriteLine($"\nExpense with ID: {expenseToEdit.Id} is edited successfully!");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Delete an expense" option
        static void DeleteExpense()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----DELETE AN EXPENSE-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    if (_expenseService.GetExpenseCount() == 0)
                    {
                        Console.WriteLine("No expenses to edit!");
                        Console.WriteLine("\nPress any key to return to Main Menu.");
                        Console.ReadKey();
                        DisplayMenu();
                        return;
                    }

                    foreach (var expense in _expenseService.GetAllExpenses())
                    {
                        Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                    }

                    Console.WriteLine();
                    Console.Write("\nEnter the ID of the expense you want to delete: ");
                    string idInput = Console.ReadLine();
                    if (CheckForExitOrReturn(idInput))
                    {
                        return;
                    }

                    if (!Guid.TryParse(idInput, out Guid expenseId) || string.IsNullOrWhiteSpace(idInput))
                    {
                        Console.WriteLine("Invalid ID format!");
                        Console.ReadKey();
                        continue;
                    }
                    else if (!_expenseService.ExpenseExists(expenseId.ToString()))
                    {
                        Console.WriteLine("Expense with the given ID does not exist!");
                        Console.ReadKey();
                        continue;
                    }

                    Expense expenseToDelete = _expenseService.GetExpenseById(expenseId.ToString());
                    _expenseService.DeleteExpense(expenseId.ToString());        // Delete the expense
                    Console.WriteLine();
                    Console.WriteLine($"\nExpense with ID: {expenseToDelete.Id} is deleted successfully!");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }

        // Display the "All expenses" option
        static void ViewAllExpenses()
        {
            Console.Clear();
            Console.WriteLine("-----ALL EXPENSES-----");
            Console.WriteLine("\nPress any key to return to main menu.\n");

            try
            {
                if (_expenseService.GetExpenseCount() == 0)
                {
                    Console.WriteLine("No expenses yet.");
                    Console.WriteLine("\nPress any key to return to main menu.");
                    Console.ReadKey();
                    DisplayMenu();
                    return;
                }

                foreach (var expense in _expenseService.GetAllExpenses())
                {
                    Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                }

                Console.ReadKey();
                DisplayMenu();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        // Display the "Total number of expenses" option
        static void ViewTotalNumberOfExpenses()
        {
            Console.Clear();
            Console.WriteLine("-----TOTAL NUMBER OF EXPENSES-----");
            Console.WriteLine("\nPress any key to return to main menu.\n");

            try
            {
                Console.WriteLine($"Total number of expenses: {_expenseService.GetExpenseCount()}");

                Console.ReadKey();
                DisplayMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
        // Display the "Expenses by description" option
        static void SearchByDescription()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----EXPENSES BY DESCRIPTION-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    Console.WriteLine();
                    Console.Write("\nEnter the description of the expense: ");
                    string description = Console.ReadLine();
                    if (CheckForExitOrReturn(description))
                    {
                        return;
                    }

                    foreach (var expense in _expenseService.GetExpensesByDescription(description))
                    {
                        Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                    }

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Expenses by date range" option
        static void SearchByDateRange()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----EXPENSES BY DATE RANGE-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    DateTime startDate;
                    DateTime endDate;
                    // Get start date
                    while (true)
                    {
                        Console.Write("Date(yyyy-mm-dd,please Enter the start date): ");
                        string startDateInput = Console.ReadLine();
                        if (CheckForExitOrReturn(startDateInput))
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(startDateInput) || !DateTime.TryParse(startDateInput, out startDate))
                        {
                            Console.WriteLine("Invalid date format!");
                            continue;
                        }
                        break;
                    }
                    // Get end date
                    while (true)
                    {
                        Console.Write("Date(yyyy-mm-dd,please Enter the end date): ");
                        string endDateInput = Console.ReadLine();
                        if (CheckForExitOrReturn(endDateInput))
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(endDateInput) || !DateTime.TryParse(endDateInput, out endDate))
                        {
                            Console.WriteLine("Invalid date format!");
                            continue;
                        }
                        break;
                    }

                    if (_expenseService.GetExpensesByDateRange(startDate, endDate).Count == 0)
                    {
                        Console.WriteLine("\nNo expenses found in the given date range.");
                    }
                    else
                    {
                        foreach (var expense in _expenseService.GetExpensesByDateRange(startDate, endDate))
                        {
                            Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                        }
                    }

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Search expenses by category" option
        static void SearchByCategory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----SEARCH EXPENSES BY CATEGORY-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)");
                Console.WriteLine("Category types: Food, Transportation, Utilities, Entertainment, Shopping, Healthcare, Rent, Other\n");

                try
                {
                    Category category;
                    // Get category
                    while (true)
                    {
                        Console.Write("Please enter the category: ");
                        string categoryInput = Console.ReadLine();
                        if (CheckForExitOrReturn(categoryInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(categoryInput) || !Enum.TryParse<Category>(categoryInput, true, out category))
                        {
                            Console.WriteLine("Invalid category!");
                            continue;
                        }
                        break;
                    }

                    if (_expenseService.GetExpensesByCategory(category).Count == 0)
                    {
                        Console.WriteLine("\nNo expenses found in the given category.");
                    }
                    else
                    {
                        foreach (var expense in _expenseService.GetExpensesByCategory(category))
                        {
                            Console.WriteLine($"ID: {expense.Id} | Description: {expense.Description} | Amount: {expense.Amount:C} | Date: {expense.Date.ToShortDateString()} | Category: {expense.Category}");
                        }
                    }

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Show monthly totals" option
        static void ShowMonthlyTotals()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----SHOW MONTHLY TOTALS-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    int year;
                    int month;

                    // Get year
                    while (true)
                    {
                        Console.Write("Please enter the year: ");
                        string yearInput = Console.ReadLine();
                        if (CheckForExitOrReturn(yearInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(yearInput) || !int.TryParse(yearInput, out year) || year < 1 || year > DateTime.Now.Year)
                        {
                            Console.WriteLine("Invalid year!");
                            continue;
                        }
                        break;
                    }

                    // Get month
                    while (true)
                    {
                        Console.Write("Please enter the month: ");
                        string monthInput = Console.ReadLine();
                        if (CheckForExitOrReturn(monthInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(monthInput) || !int.TryParse(monthInput, out month))
                        {
                            Console.WriteLine("Invalid month!");
                            continue;
                        }
                        month = Convert.ToInt32(monthInput);
                        break;
                    }

                    decimal totalExpense = _expenseService.CalculateTotalOfMonth(year, month);
                    Console.WriteLine($"\nTotal expenses for {month}/{year}: {totalExpense:C}");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Show yearly totals" option
        static void ShowYearlyTotals()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----SHOW YEARLY TOTALS-----");
                Console.WriteLine("\n(Press 'R' to return to Main Menu, 'Esc' to Exit at any time)\n");

                try
                {
                    int year;

                    // Get year
                    while (true)
                    {
                        Console.Write("Please enter the year: ");
                        string yearInput = Console.ReadLine();
                        if (CheckForExitOrReturn(yearInput))
                        {
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(yearInput) || !int.TryParse(yearInput, out year) || year < 1 || year > DateTime.Now.Year)
                        {
                            Console.WriteLine("Invalid year!");
                            continue;
                        }
                        break;
                    }

                    decimal totalExpense = _expenseService.CalculateTotalOfYear(year);
                    Console.WriteLine($"\nTotal expenses for {year}: {totalExpense:C}");

                    Console.WriteLine("\nPress 'R' to return to Main Menu, 'Esc' to Exit, or any key to continue");
                    string key = Console.ReadLine().ToString();
                    if (CheckForExitOrReturn(key))
                    {
                        return;
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        // Display the "Show total of all expenses" option
        static void ShowTotalAllExpense()
        {
            Console.Clear();
            Console.WriteLine("-----SHOW TOTAL OF ALL EXPENSES-----");
            Console.WriteLine("\nPress any key to return to main menu.\n");

            try
            {
                Console.WriteLine($"Total expenses cost: {_expenseService.CalculateTotalExpense():C}");

                Console.ReadKey();
                DisplayMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        // Display the "Exit" option
        static void ExitApplication()
        {
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
        static bool CheckForExitOrReturn(string input)
        {
            input = input.Trim().ToUpper();
            if (input == "ESC" || input == "EXIT" || input == "ESCAPE")
            {
                // Exit the application
                ExitApplication();
                return true;
            }
            else if (input == "R")
            {
                // Return to main menu
                DisplayMenu();
                return true;
            }
            return false;
        }
    }
}


