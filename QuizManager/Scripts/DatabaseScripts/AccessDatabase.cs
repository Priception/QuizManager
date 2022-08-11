﻿using System;
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

        public string CreateNewQuestionTable(int joinValue)
        {
            try
            {
                string joinName = joinValue.ToString();

                List<int> checkForDups = ReadJoinNumbersFromQuizInfo();
                //"( QuestionNum Long, Answer Long, QuesDescription LongText, QuesAnswer1 LongText, QuesAnswer2 LongText, QuesAnswer3 LongText, QuesAnswer4 LongText, );");

                string strSQL = string.Concat("CREATE TABLE ", joinName);

                string createTable = string.Concat("(QuestionNum INT,Answer INT,QuesDescription VARCHAR(255),QuesAnswer1 VARCHAR(255),QuesAnswer2 VARCHAR(255),QuesAnswer3 VARCHAR(255),QuesAnswer4 VARCHAR(255))");
                string populateTable = string.Concat(" (QuestionNum,Answer,QuesDescription,QuesAnswer1,QuesAnswer2,QuesAnswer3,QuesAnswer4) VALUES (1,0,'Description','Answer1','Answer2','Answer3','Answer4')");

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

        public bool WriteToCurrentQuiz(string quizName, string quizNumber)
        {
            string strSQL = "UPDATE CurrentQuiz";
            string updateColumns = string.Concat(" SET QuizName = '", quizName, "', QuizNumber = '", quizNumber, "' WHERE ID = 1");
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

        public bool CheckSQLIDExists(string joinValue, int questionNum) //WIP
        {
            string strSQL = string.Concat("SELECT QuestionNum FROM ", joinValue , " WHERE QuestionNum = '", questionNum, "'");
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
                            resourceNames.Add(String.Concat(reader["QuestionNum"].ToString()));
                            
                        }
                    }
                    connection.Close();



                    return true;
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
