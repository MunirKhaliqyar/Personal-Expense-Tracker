using System;
using System.Linq;
using Personal_Expense_Tracker.Models;

namespace Personal_Expense_Tracker.Models
{
    internal class Expense
    {
        // Fields
        private string _id;
        private string _description;
        private decimal _amount;
        private DateTime _date;
        private Category _category;

        // Constructor
        public Expense(string description, decimal amount, DateTime date, Category category)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Decription cannot be empty.");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive.");
            }
            if (date > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in future.");
            }

            _id = GenerateId();          // Assign an ID
            _description = description;
            _amount = amount;
            _date = date;
            _category = category;
        }

        // Properties
        public string Id { get { return _id; } }
        public string Description { get { return _description; } }
        public decimal Amount { get { return _amount; } }
        public DateTime Date { get { return _date; } }
        public Category Category 
        {
            get { return _category; }
            set { _category = value; }
        }

        // Methods
        
        // Returns a string of unique ID
        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
