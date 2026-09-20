using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsExpense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode _Mode = enMode.AddNew;

        public int ExpenseID { set; get; }
        //public int PersonID { set; get; }
        //public clsPerson PersonInfo;

        public string ExpenseName { set; get; }

        public string ExpenseDescription { set; get; }
        public clsExpense()

        {
            this.ExpenseID = -1;
            this.ExpenseName = "";
            this.ExpenseDescription = "";

            _Mode = enMode.AddNew;

        }

        private clsExpense(int ExpenseID, string ExpenseName,
            string ExpenseDescription)

        {
            this.ExpenseID = ExpenseID;
            //this.PersonID = PersonID;
            //PersonInfo = clsPerson.Find(PersonID);
            this.ExpenseName = ExpenseName;
            this.ExpenseDescription = ExpenseDescription;

            _Mode = enMode.Update;

        }

        /// <summary>
        /// Add new Expense and returns the result.
        /// </summary>
        /// <returns>If Expense added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewExpense()
        {
            //call DataAccess Layer 

            this.ExpenseID = clsExpenseData.AddNewExpense(this.ExpenseName,
                this.ExpenseDescription);

            return (this.ExpenseID != -1);
        }

        /// <summary>
        /// Update Expense info and returns the result.
        /// </summary>
        /// <returns>If Expense updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateExpense()
        {
            //call DataAccess Layer 

            //return clsExpenseData.UpdateExpense(this.ExpenseID, this.PersonID,
            //    this.ExpenseName, this.ExpenseDescription, this.isActive);


            return clsExpenseData.UpdateExpense(this.ExpenseID,
                this.ExpenseName, this.ExpenseDescription);

        }

        /// <summary>
        /// Find Expense info by <paramref name="ExpenseID"/> and returns the result.
        /// </summary>
        /// <param name="ExpenseID">The Expense ID for find Expense info.</param>
        /// <returns>If Expense find successfully return Expense <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsExpense FindByExpenseID(int ExpenseID)
        {

            string ExpenseName = "", ExpenseDescription = "";

            if (clsExpenseData.GetExpenseInfoByID(ExpenseID,
                ref ExpenseName, ref ExpenseDescription))

                return new clsExpense(ExpenseID, ExpenseName,
                    ExpenseDescription);
            else
                return null!;

        }

        /// <summary>
        /// Find Expense info by <paramref name="ExpenseName"/> and returns the result.
        /// </summary>
        /// <param name="ExpenseName">The Expense Name for find Expense info.</param>
        /// <returns>If Expense find successfully return Expense <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsExpense FindByExpenseName(string ExpenseName)
        {

            int ExpenseID = -1;
            string ExpenseDescription = "";

            if (clsExpenseData.GetExpenseInfoByExpenseName(ExpenseName, ref ExpenseID, ref ExpenseDescription))

                return new clsExpense(ExpenseID, ExpenseName,
                    ExpenseDescription);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new Expense and returns the result, 
        /// if Mode update so will Save Expense changes and returns the result.
        /// </summary>
        /// <returns>If Expense save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewExpense())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateExpense();

            }

            return false;
        }

        /// <summary>
        /// This function will get all Expenses from database. 
        /// </summary>
        /// <returns>All Expenses info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetAllExpenses()
        {
            return clsExpenseData.GetAllExpenses();
        }

        /// <summary>
        /// This function will delete Expense from database if <paramref name="ExpenseID"/> is exists and returns the result.
        /// </summary>
        /// <param name="ExpenseID">The Expense ID for delete Expense info.</param>
        /// <returns>If Expense deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteExpense(int ExpenseID)
        {
            return clsExpenseData.DeleteExpense(ExpenseID);
        }


        /// <summary>
        /// This function will check if Expense exists into database by <paramref name="ExpenseID"/> and returns the result, 
        /// </summary>
        /// <param name="ExpenseID">The Expense ID for check Expense is exists.</param>
        /// <returns>If Expenseexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isExpenseExist(int ExpenseID)
        {
            return clsExpenseData.IsExpenseExist(ExpenseID);
        }

        /// <summary>
        /// This function will check if Expense exists into database by <paramref name="ExpenseName"/> and returns the result, 
        /// </summary>
        /// <param name="ExpenseName">The Expense Name for check Expense is exists.</param>
        /// <returns>If Expenseexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isExpenseExist(string ExpenseName)
        {
            return clsExpenseData.IsExpenseExist(ExpenseName);
        }

    }
}
