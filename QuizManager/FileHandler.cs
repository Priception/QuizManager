﻿namespace QuizManager
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

        public string Readlines(string path, int colour)
        {
            string[] readText = File.ReadAllLines(path);

            string content = readText[colour];

            //int num = content.IndexOf("=");
            //if (num >= 0)
            //{
            //    content = content.Substring(0, num);
            //}
                
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
    }
}
