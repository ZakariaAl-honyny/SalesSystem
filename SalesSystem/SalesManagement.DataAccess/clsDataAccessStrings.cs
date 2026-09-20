using System;
using System.Configuration;

namespace SalesManagement.DataAccess
{
    internal static class clsDataAccessSettings
    {


        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        //public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;";
        //public static string ConnectionString = "Server=.;Database=DVLD;Integrated Security=True;";

    }

}
