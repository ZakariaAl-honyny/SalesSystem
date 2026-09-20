using SalesManagement.DataAccess;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsUser
    {

        public enum enMode { AddNew = 0, Update = 1 };
        protected enMode _Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int EmployeeID { set; get; }
        public clsEmployee EmployeeInfo;

        public string UserName { set; get; }

        public string Password { set; get; }
        public bool isActive { set; get; }

        public clsUser()

        {
            this.UserID = -1;
            this.EmployeeID = -1;
            this.UserName = "";
            this.Password = "";
            this.isActive = false;

            _Mode = enMode.AddNew;

        }

        private clsUser(int UserID, int EmployeeID, string UserName,
            string Password, bool isAction)

        {
            this.UserID = UserID;
            this.EmployeeID = EmployeeID;
            EmployeeInfo = clsEmployee.FindByEmployeeID(EmployeeID);
            this.UserName = UserName;
            this.Password = Password;
            this.isActive = isAction;

            _Mode = enMode.Update;

        }

        /// <summary>
        /// Add new User and returns the result.
        /// </summary>
        /// <returns>If User added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _AddNewUser()
        {
            //call DataAccess Layer 

            this.UserID = clsUserData.AddNewUser(this.EmployeeID,this.UserName, this.Password, this.isActive);

            return (this.UserID != -1);
        }

        /// <summary>
        /// Update User info and returns the result.
        /// </summary>
        /// <returns>If User updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        private bool _UpdateUser()
        {
            //call DataAccess Layer 

            return clsUserData.UpdateUser(this.UserID, this.EmployeeID, this.UserName, this.Password, this.isActive);

        }

        /// <summary>
        /// Find User info by <paramref name="UserID"/> and returns the result.
        /// </summary>
        /// <param name="UserID">The User ID for find User info.</param>
        /// <returns>If User find successfully return User <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsUser FindByUserID(int UserID)
        {
            int EmployeeID = -1;
            string UserName = "", Password = "";
            bool isAction = false;

            if (clsUserData.GetUserInfoByID(UserID, ref EmployeeID, ref UserName, ref Password, ref isAction))

                return new clsUser(UserID, EmployeeID, UserName,
                    Password, isAction);
            else
                return null!;

        }

        /// <summary>
        /// Find User info by <paramref name="UserName"/> and returns the result.
        /// </summary>
        /// <param name="UserName">The User Name for find User info.</param>
        /// <returns>If User find successfully return User <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsUser FindByUserName(string UserName)
        {

            int UserID = -1, EmployeeID = -1;
            string Password = "";
            bool isAction = false;

            if (clsUserData.GetUserInfoByUserName(UserName, ref UserID, ref EmployeeID, ref Password, ref isAction))

                return new clsUser(UserID, EmployeeID, UserName,
                    Password, isAction);
            else
                return null!;

        }

        /// <summary>
        /// Find User info by <paramref name="UserName"/> and <paramref name="Password"/> and  returns the result.
        /// </summary>
        /// <param name="UserName">The User Name for find User info.</param>
        /// <param name="Password">The Password for find User info.</param>
        /// <returns>If User find successfully return User <seealso cref="object"/>, otherwise <see langword="-1"/>.</returns>
        public static clsUser FindByUserNameAndPassword(string UserName, string Password)
        {

            int UserID = -1, EmployeeID = -1;
            bool isAction = false;

            if (clsUserData.GetUserInfoByUserNameAndPassword(UserName, Password, ref UserID, ref EmployeeID, ref isAction))

                return new clsUser(UserID, EmployeeID, UserName,
                    Password, isAction);
            else
                return null!;

        }

        /// <summary>
        /// If Mode add new so will add new User and returns the result, 
        /// if Mode update so will Save User changes and returns the result.
        /// </summary>
        /// <returns>If User save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }

        /// <summary>
        /// This function will get all Users from database. 
        /// </summary>
        /// <returns>All Users info into <seealso cref="DataView"/> class.</returns>
        public static DataView GetUsers()
        {
            return clsUserData.GetAllUsers();
        }

        /// <summary>
        /// This function will delete User from database if <paramref name="UserID"/> is exists and returns the result.
        /// </summary>
        /// <param name="UserID">The User ID for delete User info.</param>
        /// <returns>If User deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        /// <summary>
        /// This function will change or update password for <paramref name="UserID"/> and returns the result.
        /// </summary>
        /// <param name="UserID">The User ID for change password for <paramref name="UserID"/>.</param>
        /// <param name="NewPassword">The New Password for set current password to <paramref name="NewPassword"/>.</param>
        /// <returns>If password changed successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool ChangePassword(  int UserID, string NewPassword)
        {
            return clsUserData.ChangePassword(UserID, NewPassword);
        }

        /// <summary>
        /// This function will check if User exists into database by <paramref name="UserID"/> and returns the result, 
        /// </summary>
        /// <param name="UserID">The User ID for check User is exists.</param>
        /// <returns>If Userexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }

        /// <summary>
        /// This function will check if User exists into database by <paramref name="UserName"/> and returns the result, 
        /// </summary>
        /// <param name="UserName">The User Name for check User is exists.</param>
        /// <returns>If Userexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        /// <summary>
        /// This function will check if User exists into database by <paramref name="EmployeeID"/> and returns the result, 
        /// </summary>
        /// <param name="EmployeeID">The Employee ID for check User is exists.</param>
        /// <returns>If Userexists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
        public static bool IsUserExistByEmployeeID(int EmployeeID)
        {
            return clsUserData.IsUserExistByEmployeeID(EmployeeID);
        }

    }
}
