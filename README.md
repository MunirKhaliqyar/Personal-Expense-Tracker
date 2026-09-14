# Personal Expense Tracker

A lightweight console application to track personal expenses. The app supports adding, editing, deleting, searching, and summarizing expenses. Data is stored locally in `Data/expenses.json`.

## Table of Contents
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Build & Run](#build--run)
- [Usage](#usage)
- [Data Storage](#data-storage)
- [JSON Schema Example](#json-schema-example)
- [Common Issues & Troubleshooting](#common-issues--troubleshooting)
- [Contributing](#contributing)
- [Git Ignore (Data folder)](#git-ignore-data-folder)
- [License](#license)

## Features
- Add a new expense (description, amount, date, category).
- Edit an existing expense by ID (preserve unchanged fields by leaving input blank).
- Delete an expense by ID.
- View all expenses with formatted output.
- View the total number of expenses.
- Search expenses by description (partial match).
- Search expenses by date range (inclusive by calendar day).
- Filter expenses by category.
- Show monthly and yearly totals.
- Show total of all expenses.
- Persist data to `Data/expenses.json` between runs.

## Requirements
- .NET Framework 4.7.2
- Visual Studio 2022 (recommended) or any compatible IDE that supports .NET Framework

If you use JSON serialization via `System.Text.Json`, install the package for .NET Framework projects or use `Newtonsoft.Json` (Json.NET).

## Installation
1. Clone the repository:

    ```sh
    git clone https://github.com/MunirKhaliqyar/Personal-Expense-Tracker.git
    cd "Personal Expense Tracker"
    ```

2. Open the solution in Visual Studio 2022: double-click the `.sln` file or use __File > Open > Project/Solution__.
3. Restore NuGet packages (Visual Studio will usually do this automatically). If you use `System.Text.Json` on .NET Framework, install the package via __Manage NuGet Packages__ or:

    ```powershell
    Install-Package System.Text.Json
    # or
    Install-Package Newtonsoft.Json
    ```

## Build & Run
- Build: __Build > Build Solution__ or press `Ctrl+Shift+B`.
- Run: __Debug > Start Without Debugging__ or press `Ctrl+F5`.

The console menu will appear. Use the numeric options to operate the application.

## Usage
When you start the application, you will see a menu with options:

- `1` Add an Expense
- `2` Edit an Expense
- `3` Delete an Expense
- `4` View All Expenses
- `5` View Total number of Expenses
- `6` Search by Description
- `7` Search by Date Range
- `8` Search by Category
- `9` Show Monthly Totals
- `10` Show Yearly Totals
- `11` Show Total of All Expenses
- `0` Exit

Interactive prompts accept these special inputs at most fields:
- `R` � return to Main Menu
- `ESC` or `EXIT` � exit the application

**Notes about editing:**
- When editing an expense, leave an input blank to keep its current value.
- IDs are GUID strings shown next to every expense and are the stable identifier for editing/deleting.

## Data Storage
All data is stored in the `Data` folder as `Data/expenses.json` in JSON format. The application creates the `Data` folder automatically if it does not exist.

**Important implementation notes:**
- Ensure your `Expense` model allows the serializer to restore the `Id` value (for example, a private setter plus `[JsonInclude]` for `System.Text.Json`, or a private setter for `Newtonsoft.Json`) so previously saved item IDs are preserved when loading.
- Dates are stored with time. When implementing date range queries, make comparisons using the `Date` component (e.g., `expense.Date.Date`) or treat the end date as end-of-day to ensure inclusive day ranges.

## JSON Schema Example
An example entry stored in `Data/expenses.json`:

[
  {
    "Id": "3f9a7b5b-1f2a-4d5e-9a6b-2a3b4c5d6e7f",
    "Description": "Groceries",
    "Amount": 45.50,
    "Date": "2026-09-02T14:30:00",
    "Category": "Food"
  }
]

## Issues that I encountered & Troubleshooting
- **IDs change after reload:**
  - **Cause:** The `Expense` constructor always generates a new ID, and the serializer could not restore the saved `Id`.
  - **Fix:** Allow JSON deserialization to set `Id` (give `Id` a private setter and mark it with `[JsonInclude]` for `System.Text.Json` or rely on `Newtonsoft.Json` which can set private setters).

- **No results for date-range queries when searching by day:**
  - **Cause:** Stored `Date` includes time of day while the user-provided start/end may be midnight (`00:00`).
  - **Fix:** Compare `expense.Date.Date` with `start.Date`/`end.Date` or set `end` to end-of-day (`end.Date.AddDays(1).AddTicks(-1)`).

- **`System.Text.Json` not found on .NET Framework 4.7.2:**
  - Install `System.Text.Json` NuGet package or prefer `Newtonsoft.Json` for framework compatibility.

- **Visual Studio build errors after package install:**
  - Ensure the package is installed in the correct project (the one that contains `FileService.cs`). Rebuild the solution.

## Contributing
Contributions are welcome. Suggested workflow:
1. Fork the repository.
2. Create a branch for your change: `git checkout -b feature/your-feature`.
3. Implement and test your change in Visual Studio.
4. Commit and push your branch, then open a pull request.

Please follow these project guidelines:
- Keep code formatting consistent with the existing code.
- Add validation and defensive checks for user inputs.
- If you change the persisted data format, add migration guidance in the README.

## Git Ignore (Data folder)
To avoid committing local `Data` files, add to `.gitignore` (place `.gitignore` at the repository root or the level that contains `Data`):

# Ignore runtime data
Data/

If `Data/` is already committed, remove it from the index and commit:

git rm -r --cached "Personal Expense Tracker/Data"
git add .gitignore
git commit -m "Ignore Data folder"

---