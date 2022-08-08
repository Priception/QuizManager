using System;
using System.Data.OleDb;

namespace QuizManager.Scripts.DatabaseScripts
{

    public class AccessDatabase
    {
        const string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=G:\Visual Studio Repos\repos\QuizManager\QuizManager\Databases\QuizManager.accdb;Persist Security Info=True";

        public List<string> DatabaseTest()
        {
            string strSQL = "SELECT * FROM Students";
            List<string> resourceNames = new List<string>();
            // Create a connection    
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            { 
                OleDbCommand command = new OleDbCommand(strSQL, connection);
                try
                {
                    // Open connecton    
                    connection.Open();
                    // Execute command    
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resourceNames.Add(String.Concat("{0} {1}", reader["First Name"].ToString(), reader["Last Name"]).ToString());
                        }
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }
                return resourceNames;
            }
        }

        public int AddNewQuizInfoToDatabase(string QuizName, string QuizSubject)
        {
            int joinValue = 0;
            
            try
            {
                Random random = new Random();
                joinValue = random.Next(100000, 999999);

                List<int> checkForDups = ReadJoinNumbersFromQuizInfo();
                for (int i = 0; i < checkForDups.Count; i++)
                {
                    if (joinValue == checkForDups[i])
                    {
                        joinValue = random.Next(100000, 999999);
                        i = 0;
                    }
                }

                string strSQL = string.Concat("INSERT INTO QuizInfo(QuizName, QuizSubject, JoinCode) VALUES ('", QuizName, "', '", QuizSubject, "', '", joinValue, "')");

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();
                }

                return joinValue;
            }
            catch (Exception e)
            {
                return 0;
            }
            
        }

        public List<int> ReadJoinNumbersFromQuizInfo()
        {
            string strSQL = "SELECT * FROM QuizInfo";
            List<int> joinNames = new List<int>();
            List<string> resourceNames = new List<string>();
            // Create a connection    
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(strSQL, connection);
                try
                {
                    // Open connecton    
                    connection.Open();
                    // Execute command    
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resourceNames.Add(reader["JoinCode"].ToString());
                        }

                    }
                    connection.Close();
                    return resourceNames.Select(int.Parse).ToList();
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }
            }
        }
    } 
        
}
