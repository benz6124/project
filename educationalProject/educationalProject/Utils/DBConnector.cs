using System;

namespace educationalProject.Utils
{
    public class DBConnector
    {
        public System.Data.Common.DbCommand iCommand;
        public System.Data.Common.DbDataAdapter iAdapter;
        public bool SQLConnect()
        {
            try
            {
                string con = System.Configuration.ConfigurationManager.ConnectionStrings["projectdbconnection"].ConnectionString;
                iCommand = new System.Data.SqlClient.SqlCommand("", new System.Data.SqlClient.SqlConnection(con));
                iAdapter = new System.Data.SqlClient.SqlDataAdapter();
                iCommand.Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SQLDisconnect()
        {
            if (iCommand != null)
            {
                if (iCommand.Connection.State == System.Data.ConnectionState.Open)
                {
                    iCommand.Connection.Close();
                }
                return true;
            }
            return false;
        }
    }
}