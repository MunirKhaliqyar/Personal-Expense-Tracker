using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Personal_Expense_Tracker.Models;

namespace Personal_Expense_Tracker.Services
{
    internal class ExpenseService
    {
        // Fields
        FileService _fileService = new FileService();
        private readonly List<Expense> _expenseList = new List<Expense>();

        // Constructor
        public ExpenseService()
        {
            // Load expenses from file on initialization
            var loadedExpenses = _fileService.LoadExpenses();
            if (loadedExpenses != null)
            {
                _expenseList.AddRange(loadedExpenses);
            }
        }

        // Methods

        // Adds an expense to the list
        public void AddExpense(Expense expense) 
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense), "Expense cannot be null.");
            }
            _expenseList.Add(expense);
        }

        // Removes an expense from the list by id
        public bool DeleteExpense(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof (id), "ID cannot be empty");
            }
            var expense = _expenseList.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                return false;
            }
            return _expenseList.Remove(expense);
        }
        // Edits an expense - only updates the fields that are provided (non-null)
        public void EditExpense(string id, 
            string newDescription,
            decimal newAmount,
            DateTime newDate,
            Category newCategory)
        {
            // Validate ID
            if (string.IsNullOrWhiteSpace(id)) 
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be empty");
            }

            // Find the expense
            var expense = _expenseList.FirstOrDefault(e => e.Id == id);

            // Update only the fields that are provided
            if (newDescription != null)
            {
                if (string.IsNullOrWhiteSpace(newDescription)) 
                {
                    throw new ArgumentNullException(nameof(newDescription), "Description cannot be empty");
                }

                expense.Description = newDescription;
            }

            if (newAmount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newAmount), "Amount must be positive");
            }
            expense.Amount = newAmount;
            
            expense.Date = newDate;

            expense.Category = newCategory;
        }

        // Returns a copy of all expenses
        public List<Expense> GetAllExpenses()
        {
            return new List<Expense>(_expenseList);
        }

        // Returns all expenses in a specific category
        public List<Expense> SearchExpenses(Category category)
        {
            // Using Linq to filter the expenses based on the category
            return _expenseList
                .Where(expense => expense.Category == category)
                .ToList();
        }

        // Calculates total expenses of a specific month
        public decimal CalculateTotalOfMonth(int year, int month)
        {
            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be a positive integer.");
            }
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
            }

            return _expenseList
                .Where(expense => expense.Date.Year == year && expense.Date.Month == month)
                .Sum(expense => expense.Amount);
        }

        // Calculates total expenses of a specific year
        public decimal CalculateTotalOfYear(int year)
        {
            return _expenseList
                .Where(expense => expense.Date.Year == year)
                .Sum (expense => expense.Amount);
        }

        // Calculates total expenses of all time
        public decimal CalculateTotalExpense()
        {
            return _expenseList.Sum(expense => expense.Amount);
        }

        // Gets expense count
        public int GetExpenseCount()
        {
            return _expenseList.Count;
        }

        // Get expenses within a date range
        public List<Expense> GetExpensesByDateRange(DateTime starteDate, DateTime endDate)
        {
            return _expenseList
                .Where(expense => expense.Date.Date >= starteDate && expense.Date.Date <= endDate)
                .ToList();
        }

        // Get expenses by description (partial match)
        public List<Expense> GetExpensesByDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return new List<Expense>();
            }
            return _expenseList
                .Where(expense => expense.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        // Get expenses by category
        public List<Expense> GetExpensesByCategory(Category category)
        {
            return _expenseList
                .Where(expense => expense.Category == category)
                .ToList();
        }

        // Get expense by ID
        public Expense GetExpenseById(string id)
        {
            return _expenseList.FirstOrDefault(expense => expense.Id == id);
        }

        // Checks if an expense exists by ID
        public bool ExpenseExists(string id)
        {
            return _expenseList.Any(expense => expense.Id == id);
        }
    }
}
