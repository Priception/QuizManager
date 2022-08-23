using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

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

        public string CreateNewQuestionTable(int joinValue)
        {
            try
            {
                string joinName = joinValue.ToString();

                List<int> checkForDups = ReadJoinNumbersFromQuizInfo();
                //"( QuestionNum Long, Answer Long, QuesDescription LongText, QuesAnswer1 LongText, QuesAnswer2 LongText, QuesAnswer3 LongText, QuesAnswer4 LongText, );");

                string strSQL = string.Concat("CREATE TABLE ", joinName);

                string createTable = string.Concat("(ID INT,Answer INT,QuesDescription VARCHAR(255),QuesAnswer1 VARCHAR(255),QuesAnswer2 VARCHAR(255),QuesAnswer3 VARCHAR(255),QuesAnswer4 VARCHAR(255), QuestionNum INT)");
                string populateTable = string.Concat("(ID,Answer,QuesDescription,QuesAnswer1,QuesAnswer2,QuesAnswer3,QuesAnswer4,QuestionNum) VALUES (1,0,'Description','Answer1','Answer2','Answer3','Answer4', 1)");

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand command = new OleDbCommand { Connection = connection })
                    {
                        command.CommandText = string.Concat(strSQL, createTable);// "CREATE TABLE Members(FirstName CHAR(255),LastName CHAR(255),JoinedYear INT)";
                        connection.Open();
                        command.ExecuteNonQuery();
                        strSQL = string.Concat("INSERT INTO ", joinName);
                        command.CommandText = string.Concat(strSQL, populateTable);  //"INSERT INTO Members (FirstName,LastName,JoinedYear) VALUES ('Paul','Gallagher',2013)";
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return joinName;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public bool WriteToCurrentQuiz(string quizName, string quizNumber, int currentQuestion, int maxQuestions, int iDValue)
        {
            string strSQL = "UPDATE CurrentQuiz";
            string updateColumns = string.Concat(" SET QuizName = '", quizName, "', QuizNumber = '", quizNumber, "', CurrentQuestion = '", currentQuestion, "', MaxQuestions = '", maxQuestions, "', IDValue = '", iDValue, "' WHERE ID = 1");
            strSQL = string.Concat(strSQL, updateColumns);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return true;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return false;
                }

            }
        }

        public bool WriteToJoinQuiz(string quizName, string quizNumber, int currentQuestion, int maxQuestions, int iDValue, int questonsAnswered)
        {
            string strSQL = "UPDATE CurrentAnswers";
            string updateColumns = string.Concat(" SET QuizName = '", quizName, "', QuizNumber = '", quizNumber, "', CurrentQuestion = '", currentQuestion, "', MaxQuestions = '", maxQuestions, "', IDValue = '", iDValue, "', QuestonsAnswered = '", questonsAnswered, "' WHERE ID = 1");
            strSQL = string.Concat(strSQL, updateColumns);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return true;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return false;
                }

            }
        }

        public List<string> ReadCurrentQuizInfo()
        {
            string strSQL = "SELECT * FROM CurrentQuiz";
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
                            resourceNames.Add(String.Concat(reader["QuizName"].ToString()));
                            resourceNames.Add(String.Concat(reader["QuizNumber"].ToString()));
                            resourceNames.Add(String.Concat(reader["CurrentQuestion"].ToString()));
                            resourceNames.Add(String.Concat(reader["MaxQuestions"].ToString()));
                            resourceNames.Add(String.Concat(reader["IDValue"].ToString()));
                        }
                    }
                    connection.Close();



                    return resourceNames;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }

            }
        }

        public List<string> ReadCurrentJoinQuizInfo()
        {
            string strSQL = "SELECT * FROM CurrentAnswers";
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
                            resourceNames.Add(String.Concat(reader["QuizName"].ToString()));
                            resourceNames.Add(String.Concat(reader["QuizNumber"].ToString()));
                            resourceNames.Add(String.Concat(reader["CurrentQuestion"].ToString()));
                            resourceNames.Add(String.Concat(reader["MaxQuestions"].ToString()));
                            resourceNames.Add(String.Concat(reader["IDValue"].ToString()));
                            resourceNames.Add(String.Concat(reader["QuestonsAnswered"].ToString()));
                        }
                    }
                    connection.Close();



                    return resourceNames;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }

            }
        }

        public bool UpdateCurrentQuizQuestionTable(int joinValue, int questionNum, int answer, string quesDescription,
                                                    string answerdes1, string answerdes2, string answerdes3, string answerdes4)
        {
            string strSQL = string.Concat("UPDATE ", joinValue);
            string updateColumns = string.Concat(" SET Answer = '", answer, "', QuesDescription = '", quesDescription, "' , QuesAnswer1 = '", answerdes1, "' , QuesAnswer2 = '", answerdes2, "' , QuesAnswer3 = '", answerdes3, "' , QuesAnswer4 = '", answerdes4, "' WHERE QuestionNum = ", questionNum);
            strSQL = string.Concat(strSQL, updateColumns);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return true;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return false;
                }

            }

        }

        public List<string> ReadCurrentQuizQuestionTable(int joinValue, int questionNum)
        {
            string strSQL = string.Concat("SELECT * FROM ", joinValue, " WHERE QuestionNum = ", questionNum);
            List<string> resourceNames = new List<string>();
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
                            resourceNames.Add(String.Concat(reader["ID"].ToString()));                //0
                            resourceNames.Add(String.Concat(reader["Answer"].ToString()));            //1
                            resourceNames.Add(String.Concat(reader["QuesDescription"].ToString()));   //2
                            resourceNames.Add(String.Concat(reader["QuesAnswer1"].ToString()));       //3
                            resourceNames.Add(String.Concat(reader["QuesAnswer2"].ToString()));       //4
                            resourceNames.Add(String.Concat(reader["QuesAnswer3"].ToString()));       //5
                            resourceNames.Add(String.Concat(reader["QuesAnswer4"].ToString()));       //6
                            resourceNames.Add(String.Concat(reader["QuestionNum"].ToString()));       //7
                        }
                    }
                    connection.Close();

                    return resourceNames;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }

            }

        }

        public bool AddNewQuizQuestionToCurrentQuiz(int maxQuestions, string quizNumber, int idValue)
        {
            try
            {

                string strSQL = string.Concat("INSERT INTO ", quizNumber, " (ID,Answer,QuesDescription,QuesAnswer1,QuesAnswer2,QuesAnswer3,QuesAnswer4, QuestionNum) VALUES (", idValue, ",0,'Description','Answer1','Answer2','Answer3','Answer4',", maxQuestions, ")");

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public bool QuizQuestionDeleteRow(string quizNumber, int questionNum)
        {
            try
            {

                string strSQL = string.Concat("DELETE FROM ", quizNumber, " WHERE QuestionNum=", questionNum);

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool UpdateCurrentQuizQuestionNumber(string quizNumber, int questionNum, int idValue)
        {
            try
            {
                string strSQL = string.Concat("UPDATE ", quizNumber, " SET QuestionNum = ", questionNum, " WHERE ID=", idValue);

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<string> GetIDValueFromQuestionNumber(string joinValue)
        {

            string strSQL = string.Concat("SELECT * FROM ", joinValue, " ORDER BY ID");
            List<string> items = new List<string>();

            DataTable rstData = MyRst(strSQL);
            //dataGridView1.DataSource = rstData;
            List<string> MyIDList = new List<string>();
            foreach (DataRow MyOneRow in rstData.Rows)
            {
                MyIDList.Add(MyOneRow["ID"].ToString());
            }

            return MyIDList;


        }

        DataTable MyRst(string strSQL)
        {
            DataTable rstData = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmdSQL = new OleDbCommand(strSQL, conn))
                {
                    conn.Open();
                    rstData.Load(cmdSQL.ExecuteReader());
                }
            }
            return rstData;
        }


        public List<string> GetJoinQuizInfo(string joinValue)
        {

            string strSQL = string.Concat("SELECT * FROM QuizInfo WHERE JoinCode = ", joinValue);
            List<string> items = new List<string>();
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
                            items.Add(String.Concat(reader["ID"].ToString()));                //0
                            items.Add(String.Concat(reader["QuizName"].ToString()));          //1
                            items.Add(String.Concat(reader["QuizSubject"].ToString()));       //2
                            items.Add(String.Concat(reader["JoinCode"].ToString()));          //3
                        }
                    }
                    connection.Close();

                    return items;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public List<string> GetJoinQuizTableInfo(string joinValue, int returnValue = 0)
        {

            string strSQL = string.Concat("SELECT * FROM ", joinValue);
            List<string> resourceNames = new List<string>();
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
                            switch (returnValue)
                            {
                                case 0:
                                    resourceNames.Add(String.Concat(reader["ID"].ToString()));                //0
                                    break;
                                case 1:
                                    resourceNames.Add(String.Concat(reader["Answer"].ToString()));            //1
                                    break;

                                case 2:
                                    resourceNames.Add(String.Concat(reader["QuesDescription"].ToString()));   //2
                                    break;
                                case 3:
                                    resourceNames.Add(String.Concat(reader["QuesAnswer1"].ToString()));       //3
                                    break;
                                case 4:
                                    resourceNames.Add(String.Concat(reader["QuesAnswer2"].ToString()));       //4
                                    break;
                                case 5:
                                    resourceNames.Add(String.Concat(reader["QuesAnswer3"].ToString()));       //5
                                    break;
                                case 6:
                                    resourceNames.Add(String.Concat(reader["QuesAnswer4"].ToString()));       //6
                                    break;
                                case 7:
                                    resourceNames.Add(String.Concat(reader["QuestionNum"].ToString()));       //7
                                    break;

                            }
                        }
                    }
                    connection.Close();

                    return resourceNames;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public List<string> GetTeachersInfo(string value)
        {

            string strSQL = string.Concat("SELECT * FROM Teachers WHERE FirstName = '", value, "'");
            List<string> items = new List<string>();
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
                            items.Add(String.Concat(reader["FirstName"].ToString()));      //0
                            items.Add(String.Concat(reader["LastName"].ToString()));       //1
                            items.Add(String.Concat(reader["Password"].ToString()));       //2
                        }
                    }
                    connection.Close();

                    return items;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<string> GetStudentsInfo(string value)
        {

            string strSQL = string.Concat("SELECT * FROM Students WHERE FirstName = '", value, "'");
            List<string> items = new List<string>();
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
                            items.Add(String.Concat(reader["FirstName"].ToString()));      //0
                            items.Add(String.Concat(reader["LastName"].ToString()));       //1
                            items.Add(String.Concat(reader["Password"].ToString()));       //2
                        }
                    }
                    connection.Close();

                    return items;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public bool AddPasswordToDatabase(string tableName, string value, string firstName)
        {
            try
            {
                string strSQL = string.Concat("UPDATE ", tableName, " SET Password = '", value, "' WHERE FirstName = '", firstName, "'");

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool WriteToCurrentUser(string firstName, string lastName, string type)
        {
            string strSQL = "UPDATE CurrentUser";
            string updateColumns = string.Concat(" SET FirstName = '", firstName, "', LastName = '", lastName, "', Type = '", type, "' WHERE ID = 1");
            strSQL = string.Concat(strSQL, updateColumns);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return true;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return false;
                }

            }
        }

        public List<string> ReadUserInfo()
        {
            string strSQL = "SELECT * FROM CurrentUser";
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
                            resourceNames.Add(String.Concat(reader["FirstName"].ToString()));
                            resourceNames.Add(String.Concat(reader["LastName"].ToString()));
                            resourceNames.Add(String.Concat(reader["Type"].ToString()));
                        }
                    }
                    connection.Close();



                    return resourceNames;
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
