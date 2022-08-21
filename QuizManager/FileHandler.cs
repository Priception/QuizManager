namespace QuizManager
{
    public class FileHandler
    {
        public string ReadFromColoursFile(int colour, string testPath = null)
        {
            string path = "";
            if (testPath == null)
            {
                path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\colours.txt";
            }
            else
            {
                path = testPath;
            }

            FileHandler filehandler = new FileHandler();
            try
            {
                return filehandler.Readlines(path, colour);
            }
            catch (FileNotFoundException)
            {
                filehandler.CreateFile(path);
                string[] createText = { "#00a2e8", "#006a97", "#7f7f7f" };  //Resets to Default Values
                filehandler.Writelines(path, createText);
                return createText[colour];
            }

        }

        public bool WriteColoursToFile(string colour1, string colour2, string colour3, string testPath = null)
        {
            string path = "";
            try
            {
                if (testPath == null)
                {
                    path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\colours.txt";
                }
                else
                {
                    path = testPath;
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                FileHandler filehandler = new FileHandler();
                filehandler.CreateFile(path);
                string[] createText = { colour1, colour2, colour3 };  //Resets to Default Values
                filehandler.Writelines(path, createText);
                return true;
            } 
            catch (Exception e)
            {
                return false;
            }

        }

        public string Readlines(string path, int value)
        {
            string[] readText = File.ReadAllLines(path);

            string content = readText[value];
                
            return content;
        }

        public string ReadSpecificLine(string path, int lineNum)
        {
            string content  = File.ReadLines(path).Skip(lineNum).Take(1).First();

            return content;
        }

        public string[] ReadAllLines(string path)
        {
            string[] content = File.ReadAllLines(path);

            return content;
        }

        public void Writelines(string path, string[] createText)
        {
            File.WriteAllLines(path, createText);
        }

        public void CreateFile(string path)
        {
            File.Create(path).Close();
        }

        public bool DoesFileExist(string path)
        {
            return File.Exists(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public bool WriteToAnsweredQuestionsFile(string quizNumber, int currentQuestion, int answer, int maxQuestion)
        {
            try
            {
                string path = "";
                if (quizNumber != null)
                {
                    path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\";
                    path = string.Concat(path, "Answers", quizNumber, ".txt");

                    if(File.Exists(path))
                    {
                        string[] fileArray = ReadAllLines(path);
                        List<string> fileList = fileArray.ToList();

                        fileList[currentQuestion - 1] = answer.ToString();
                        fileArray = fileList.ToArray();
                        Writelines(path, fileArray);
                    }
                    else
                    {
                        CreateFile(path);
                        List<string> questions = new List<string>();
                        for (int i = 0; i < maxQuestion; i++)
                        {
                            questions.Add("0");
                        }

                        string[] createText = questions.ToArray();
                        Writelines(path, createText);
                        WriteToAnsweredQuestionsFile(quizNumber, currentQuestion, answer, maxQuestion);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {



                return false;
            }
        }
    }
}
