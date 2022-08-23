using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizManager.Scripts.DatabaseScripts;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuizManager.Pages
{
    public class IndexModel : PageModel
    {

        string _QuizName { get; set; }
        string _QuizNumber { get; set; }
        string _currentQuestion { get; set; }
        string _maxQuestions { get; set; }
        string _IDValue { get; set; }
        string _QuestonsAnswered { get; set; }


        public IActionResult OnGet()
        {
            string path = HttpContext.Request.Path;

            if (path != "/")
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> userDetails = accessDatabase.ReadUserInfo();
                if (userDetails.Count <= 2)
                {
                    return new RedirectToPageResult("Index");
                }
                if (string.IsNullOrEmpty(userDetails[0]) || string.IsNullOrEmpty(userDetails[1]) || string.IsNullOrEmpty(userDetails[2]))
                {
                    return new RedirectToPageResult("Index");
                }
            }
            else
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> userDetails = accessDatabase.ReadUserInfo();
                if (!string.IsNullOrEmpty(userDetails[0]) || !string.IsNullOrEmpty(userDetails[1]) || !string.IsNullOrEmpty(userDetails[2]))
                {
                    return new RedirectToPageResult("JoinPage");
                }
            }
             
            path = path.Replace("/", "");
            return new PageResult();
        }

        public string GetColours(int colour)
        {
            FileHandler filehandler = new FileHandler();
            return filehandler.ReadFromColoursFile(colour);
        }

        public IActionResult OnPostQuizInfo()
        {
            string quizName = InvalidCharCheck(Request.Form["QuizName"]);
            string quizType = InvalidCharCheck(Request.Form["QuizType"]);            

            AccessDatabase accessDatabase = new AccessDatabase();
            int joinValue = accessDatabase.AddNewQuizInfoToDatabase(quizName, quizType);

            string joinName = accessDatabase.CreateNewQuestionTable(joinValue);
            int currentQuestion = 1;
            accessDatabase.WriteToCurrentQuiz(quizName, joinName, currentQuestion, currentQuestion, currentQuestion);

            //accessDatabase.SQLTableSetAutoIncrement(joinName);

            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostColourChange()
        {
            string colour1 = Request.Form["colour1"];
            string colour2 = Request.Form["colour2"];
            string colour3 = Request.Form["colour3"];

            FileHandler filehandler = new FileHandler();
            filehandler.WriteColoursToFile(colour1, colour2, colour3);

            return RedirectToPage("Settings");
        }

        public IActionResult OnPostCreateQuizInfoSave()
        {
            string radio = InvalidCharCheck(Request.Form["answerpick"].ToString());
            if (string.IsNullOrEmpty(radio))
            {
                return RedirectToPage("CreateQuizQuestions"); //Add error page
            }
            int answer = Int32.Parse(radio);
            string quesDescription = InvalidCharCheck(Request.Form["QuizQuestion"].ToString());
            string answerdes1 = InvalidCharCheck(Request.Form["QuizAnswer1"].ToString());
            string answerdes2 = InvalidCharCheck(Request.Form["QuizAnswer2"].ToString());
            string answerdes3 = InvalidCharCheck(Request.Form["QuizAnswer3"].ToString());
            string answerdes4 = InvalidCharCheck(Request.Form["QuizAnswer4"].ToString());
            if(string.IsNullOrEmpty(quesDescription) || string.IsNullOrEmpty(answerdes1) || string.IsNullOrEmpty(answerdes2) || string.IsNullOrEmpty(answerdes3) || string.IsNullOrEmpty(answerdes4))
            {
                return RedirectToPage("CreateQuizQuestions"); //Add error page
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int quizNumber = Int32.Parse(_QuizNumber);
            int currentQuestion = Int32.Parse(_currentQuestion);

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.UpdateCurrentQuizQuestionTable(quizNumber, currentQuestion, answer, quesDescription,
                                           answerdes1, answerdes2, answerdes3, answerdes4);

            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostCreateQuizInfoSaveAndQuit()
        {
            string radio = InvalidCharCheck(Request.Form["answerpick"].ToString());
            if (string.IsNullOrEmpty(radio))
            {
                return RedirectToPage("CreateQuizQuestions"); //Add error page
            }
            int answer = Int32.Parse(radio);
            string quesDescription = InvalidCharCheck(Request.Form["QuizQuestion"].ToString());
            string answerdes1 = InvalidCharCheck(Request.Form["QuizAnswer1"].ToString());
            string answerdes2 = InvalidCharCheck(Request.Form["QuizAnswer2"].ToString());
            string answerdes3 = InvalidCharCheck(Request.Form["QuizAnswer3"].ToString());
            string answerdes4 = InvalidCharCheck(Request.Form["QuizAnswer4"].ToString());
            if (string.IsNullOrEmpty(quesDescription) || string.IsNullOrEmpty(answerdes1) || string.IsNullOrEmpty(answerdes2) || string.IsNullOrEmpty(answerdes3) || string.IsNullOrEmpty(answerdes4))
            {
                return RedirectToPage("CreateQuizQuestions"); //Add error page
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int quizNumber = Int32.Parse(_QuizNumber);
            int currentQuestion = Int32.Parse(_currentQuestion);

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.UpdateCurrentQuizQuestionTable(quizNumber, currentQuestion, answer, quesDescription,
                                           answerdes1, answerdes2, answerdes3, answerdes4);

            return RedirectToPage("CreateQuiz");
        }

        public IActionResult OnPostCreateQuizInfoBack()
        {
            return RedirectToPage("CreateQuiz");
        }

        public IActionResult OnPostAnswerQuizInfoBack()
        {
            return RedirectToPage("JoinPage");
        }

        public IActionResult OnPostAnswerQuizInfoFinish()
        {
            return RedirectToPage("QuizResults");
        }

        public IActionResult OnPostCreateQuizInfoAdd()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentQuestionIDValue();
            }
            int maxQuestions = Int32.Parse(_maxQuestions);
            int currentQuestion = Int32.Parse(_currentQuestion);
            int idValue = Int32.Parse(_IDValue);
            maxQuestions = maxQuestions + 1;
            idValue = idValue + 1;

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.AddNewQuizQuestionToCurrentQuiz(maxQuestions, _QuizNumber, idValue);

            accessDatabase.WriteToCurrentQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue);

            _maxQuestions = maxQuestions.ToString();

            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostCreateQuizInfoDelete()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentQuestionIDValue();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.QuizQuestionDeleteRow(_QuizNumber, currentQuestion);

            maxQuestions = maxQuestions - 1;

            if (currentQuestion > maxQuestions )
            {
                currentQuestion = maxQuestions;
                _currentQuestion = currentQuestion.ToString();
            }
            
            int idValue = Int32.Parse(_IDValue);

            _maxQuestions = maxQuestions.ToString();
            accessDatabase.WriteToCurrentQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue);

            List<string> idList = accessDatabase.GetIDValueFromQuestionNumber(_QuizNumber);

            for (int count = 0; count < idList.Count; count++)
            {
                accessDatabase.UpdateCurrentQuizQuestionNumber(_QuizNumber, count + 1, Int32.Parse(idList[count]));
            }
                
            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostCreateQuizInfoNext()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentQuestionIDValue();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);
            int idValue = Int32.Parse(_IDValue);

            currentQuestion = currentQuestion + 1;

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToCurrentQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue);

            _currentQuestion = currentQuestion.ToString();
            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostCreateQuizInfoPrev()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentQuestionIDValue();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);
            int idValue = Int32.Parse(_IDValue);

            currentQuestion = currentQuestion - 1;

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToCurrentQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue);

            _currentQuestion = currentQuestion.ToString();
            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostJoinQuizInfoNext()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentJoinQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentJoinQuestionIDValue();
            }
            if (string.IsNullOrEmpty(_QuestonsAnswered))
            {
                _QuestonsAnswered = GetCurrentJoinQuestionQuestonsAnswered();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);
            int idValue = Int32.Parse(_IDValue);
            int questonsAnswered = Int32.Parse(_QuestonsAnswered);

            currentQuestion = currentQuestion + 1;

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToJoinQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue, questonsAnswered);

            _currentQuestion = currentQuestion.ToString();
            return RedirectToPage("AnsweringQuizQuestions");
        }

        public IActionResult OnPostJoinQuizInfoPrev()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentJoinQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentJoinQuestionIDValue();
            }
            if (string.IsNullOrEmpty(_QuestonsAnswered))
            {
                _QuestonsAnswered = GetCurrentJoinQuestionQuestonsAnswered();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);
            int idValue = Int32.Parse(_IDValue);
            int questonsAnswered = Int32.Parse(_QuestonsAnswered);

            currentQuestion = currentQuestion - 1;

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToJoinQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue, questonsAnswered);

            _currentQuestion = currentQuestion.ToString();
            return RedirectToPage("AnsweringQuizQuestions");
        }

        public IActionResult OnPostJoinQuizButton()
        {
            string joinNumber = InvalidCharCheck(Request.Form["joinNumber"].ToString());
            if (string.IsNullOrEmpty(joinNumber))
            {
                return RedirectToPage("JoinPageError");
            }
            bool isIntString = joinNumber.All(char.IsDigit);

            if (!isIntString)
            {
                return RedirectToPage("JoinPageError");
            }

            int intJoinNumber = Int32.Parse(joinNumber);

            AccessDatabase accessDatabase = new AccessDatabase();
            List<int> joinCodes = accessDatabase.ReadJoinNumbersFromQuizInfo();

            for (int count = 0; count < joinCodes.Count; count++)
            {
                if (joinCodes[count] == intJoinNumber)
                {
                    List<string> info = accessDatabase.GetJoinQuizInfo(intJoinNumber.ToString());

                    List<string> questions = accessDatabase.GetJoinQuizTableInfo(intJoinNumber.ToString(), 7); //Change so it returns one Table Column at a time.

                    int maxQuestions = questions.Count;


                    accessDatabase.WriteToJoinQuiz(info[1], info[3], 1, maxQuestions, maxQuestions, 0);
                    // Set up information for test answering questions. 

                    string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\";
                    path = string.Concat(path, "Answers", joinNumber, ".txt");

                    FileHandler fileHandler = new FileHandler();
                    if (fileHandler.DoesFileExist(path))
                    {
                        fileHandler.DeleteFile(path); 
                    }

                    fileHandler.WriteToAnsweredQuestionsFile(joinNumber, 1, 0, maxQuestions);


                    return RedirectToPage("AnsweringQuizQuestions");
                }
            }

            return RedirectToPage("JoinPageWrongCode"); // Change to error pagge
        }

        public IActionResult OnPostLoginButton()
        {
            string firstName = InvalidCharCheck(Request.Form["FirstName"].ToString());
            string lastName = InvalidCharCheck(Request.Form["LastName"].ToString());
            string password = InvalidCharCheck(Request.Form["Password"].ToString());
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password))
            {

            }

            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> teachersInfo = accessDatabase.GetTeachersInfo(firstName);
            List<string> studentInfo = accessDatabase.GetStudentsInfo(firstName);

            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            string hasedPassword = hash.ToString();

            if (teachersInfo.Count == 0 && studentInfo.Count > 0)
            {
                if (studentInfo[2] == hasedPassword)
                {

                    accessDatabase.WriteToCurrentUser(firstName, lastName, "Student");
                }

            }
            else if (teachersInfo.Count > 0 && studentInfo.Count == 0)
            {
                if (teachersInfo[2] == hasedPassword)
                {
                    accessDatabase.WriteToCurrentUser(firstName, lastName, "Teacher");
                }
            }
            else
            {
                accessDatabase.WriteToCurrentUser("", "", "");
                // Error Page
            }
            
            return RedirectToPage("JoinPage");
        }

        public IActionResult OnPostLogOut()
        {
            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToCurrentUser("", "", "");

            return RedirectToPage("Index");
        }

        public IActionResult OnPostBackToCreate()
        {
            return RedirectToPage("CreateQuiz");
        }

        public IActionResult OnPostLoadExistingQuiz()
        {
            return RedirectToPage("LoadCreateQuiz");
        }

        public IActionResult OnPostEditExistingQuiz()
        {
            string quizNumber = InvalidCharCheck(Request.Form["joinNumber"]);
            int intJoinNumber = Int32.Parse(quizNumber);

            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> iDs = accessDatabase.GetJoinQuizTableInfo(intJoinNumber.ToString(), 0);
            List<string> questions = accessDatabase.GetJoinQuizTableInfo(intJoinNumber.ToString(), 7);
            List<string> quizInfo = accessDatabase.GetJoinQuizInfo(quizNumber);

            accessDatabase.WriteToCurrentQuiz(quizInfo[1], quizNumber, 1, questions.Count, Int32.Parse(iDs[0]));

            return RedirectToPage("CreateQuizQuestions");
        }

        public IActionResult OnPostCreateNewQuiz()
        {
            return RedirectToPage("CreateQuizInfo");
        }

        
        public string GetCurrentQuizName()
        {
            if (_QuizName == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());

                return _QuizName;
            }
            else
            {
                return _QuizName;
            }
        }

        public string GetCurrentJoinQuizName()
        {
            if (_QuizName == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentJoinQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _QuizName;
            }
            else
            {
                return _QuizName;
            }
        }

        public string GetCurrentQuizNumber()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());

                return _QuizNumber;
            }
            else
            {
                return _QuizNumber;
            }
        }

        public string GetCurrentQuestionNumber()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());

                return _currentQuestion;
            }
            else
            {
                return _currentQuestion;
            }
        }

        public string GetCurrentJoinQuestionNumber()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentJoinQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _currentQuestion;
            }
            else
            {
                return _QuizNumber;
            }
        }

        public string GetCurrentJoinQuizNumber()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _QuizNumber;
            }
            else
            {
                return _QuizNumber;
            }
        }

        public string GetCurrentQuestionMaxQuestion()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());

                return _maxQuestions;
            }
            else
            {
                return _maxQuestions;
            }
        }

        public string GetCurrentJoinQuestionMaxQuestion()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _maxQuestions;
            }
            else
            {
                return _maxQuestions;
            }
        }

        public string GetCurrentQuestionIDValue()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());

                return _maxQuestions;
            }
            else
            {
                return _maxQuestions;
            }
        }

        public string GetCurrentJoinQuestionIDValue()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentJoinQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _maxQuestions;
            }
            else
            {
                return _maxQuestions;
            }
        }

        public string GetCurrentJoinQuestionQuestonsAnswered()
        {
            if (_QuizNumber == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentJoinQuizInfo();
                _QuizName = InvalidCharCheck(currentQuiz[0].ToString());
                _QuizNumber = InvalidCharCheck(currentQuiz[1].ToString());
                _currentQuestion = InvalidCharCheck(currentQuiz[2].ToString());
                _maxQuestions = InvalidCharCheck(currentQuiz[3].ToString());
                _IDValue = InvalidCharCheck(currentQuiz[4].ToString());
                _QuestonsAnswered = InvalidCharCheck(currentQuiz[5].ToString());

                return _QuestonsAnswered;
            }
            else
            {
                return _QuestonsAnswered;
            }
        }

        private string InvalidCharCheck(string value)
        {
            if (value.Contains("'") && !value.Contains("''"))
            {
                value = value.Replace("'", "''");
            }

            return value;
        }

        public string GetCurrentQuizQuestion()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }

            return string.Concat("Question: ", _currentQuestion);
        }

        public string GetCurrentQuizAnswerQuestion(int number)
        {
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int quizNumber = Int32.Parse(_QuizNumber);
            int currentQuestion = Int32.Parse(_currentQuestion);

            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> currentQuiz = accessDatabase.ReadCurrentQuizQuestionTable(quizNumber, currentQuestion);
            string description = currentQuiz[number];

            return description;
        }

        public bool GetCurrentQuizRadioButton(int number)
        {
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int quizNumber = Int32.Parse(_QuizNumber);
            int currentQuestion = Int32.Parse(_currentQuestion);

            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> currentQuiz = accessDatabase.ReadCurrentQuizQuestionTable(quizNumber, currentQuestion);
            string value = currentQuiz[1];
            int numvalue = Int32.Parse(value);

            if (numvalue == number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetJoinQuizRadioButton(int number)
        {
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuestonsAnswered))
            {
                _QuestonsAnswered = GetCurrentJoinQuestionQuestonsAnswered();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);

            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\";
            path = string.Concat(path, "Answers", _QuizNumber, ".txt");

            FileHandler fileHander = new FileHandler();
            string[] quizAnswers = fileHander.ReadAllLines(path);

            string value = quizAnswers[currentQuestion - 1];

            int numvalue = Int32.Parse(value);

            if (numvalue == number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateQuizShouldShowNextButton()
        {
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int maxquestions = Int32.Parse(_maxQuestions);
            int currentQuestion = Int32.Parse(_currentQuestion);

            if (maxquestions == currentQuestion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool JoinQuizShouldShowNextButton()
        {
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            int maxquestions = Int32.Parse(_maxQuestions);
            int currentQuestion = Int32.Parse(_currentQuestion);

            if (maxquestions == currentQuestion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateQuizShouldShowPrevButton()
        {
            
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentQuestionNumber();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);

            if (currentQuestion == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool JoinQuizShouldShowPrevButton()
        {

            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);

            if (currentQuestion == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateQuizShouldShowDeleteButton()
        {

            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentQuestionMaxQuestion();
            }
            int maxquestions = Int32.Parse(_maxQuestions);

            if (maxquestions == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public string GetCurrentQuestionNumber()
        //{
        //    string quizNumber = GetCurrentQuizNumber();

        //}

        
        public IActionResult OnPostAnswerQuizInfoSave()
        {
            string radio = InvalidCharCheck(Request.Form["answerpick"].ToString());
            if (string.IsNullOrEmpty(radio))
            {
                return RedirectToPage("AnsweringQuizQuestions"); //Add error page
            }
            int answer = Int32.Parse(radio);

            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentJoinQuestionNumber();
            }
            int maxquestions = Int32.Parse(_maxQuestions);
            int currentquestion = Int32.Parse(_currentQuestion);

            FileHandler fileHandler = new FileHandler();
            fileHandler.WriteToAnsweredQuestionsFile(_QuizNumber, currentquestion, answer, maxquestions); // finish setting up this


            //Add function to read from text file and have it display on answering page

            return RedirectToPage("AnsweringQuizQuestions");
        }

        public string GetCurrentJoinQuizQuestion()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }

            return string.Concat("Question: ", _currentQuestion);
        }


        public string GetJoinAnswerQuestion(int number)
        {
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentJoinQuizNumber();
            }
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            int quizNumber = Int32.Parse(_QuizNumber);
            int currentQuestion = Int32.Parse(_currentQuestion);

            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> currentQuiz = accessDatabase.ReadCurrentQuizQuestionTable(quizNumber, currentQuestion);
            string description = currentQuiz[number];

            return description;
        }

        public string JoinTotalAnsweredQuestions()
        {
            if (string.IsNullOrEmpty(_currentQuestion))
            {
                _currentQuestion = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentJoinQuestionNumber();
            }
            if (string.IsNullOrEmpty(_QuizName))
            {
                _QuizName = GetCurrentJoinQuizName();
            }
            if (string.IsNullOrEmpty(_IDValue))
            {
                _IDValue = GetCurrentJoinQuestionIDValue();
            }
            if (string.IsNullOrEmpty(_QuestonsAnswered))
            {
                _QuestonsAnswered = GetCurrentJoinQuestionQuestonsAnswered();
            }
            int currentQuestion = Int32.Parse(_currentQuestion);
            int maxQuestions = Int32.Parse(_maxQuestions);
            int idValue = Int32.Parse(_IDValue);

            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\";
            path = string.Concat(path, "Answers", _QuizNumber, ".txt");

            FileHandler fileHander = new FileHandler();
            string[] quizAnswers = fileHander.ReadAllLines(path);
            int completedQuestions= 0;

            for (int count = 0; count < quizAnswers.Length; count++)
            {
                if (Int32.Parse(quizAnswers[count]) != 0)
                {
                    completedQuestions = completedQuestions + 1;
                }
            }

            AccessDatabase accessDatabase = new AccessDatabase();
            accessDatabase.WriteToJoinQuiz(_QuizName, _QuizNumber, currentQuestion, maxQuestions, idValue, completedQuestions);

            _QuestonsAnswered = completedQuestions.ToString();
            _maxQuestions = maxQuestions.ToString();

            return string.Concat("Questions Answered: ", completedQuestions,"/", _maxQuestions);
        }

        public bool AllQuestionsAnswered()
        {
            JoinTotalAnsweredQuestions();
            if (string.IsNullOrEmpty(_QuestonsAnswered))
            {
                _QuestonsAnswered = GetCurrentJoinQuestionQuestonsAnswered();
            }
            if (string.IsNullOrEmpty(_maxQuestions))
            {
                _maxQuestions = GetCurrentJoinQuestionMaxQuestion();
            }
            int questonsAnswered = Int32.Parse(_QuestonsAnswered);
            int maxQuestions = Int32.Parse(_maxQuestions);

            if (questonsAnswered == maxQuestions)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public string GetJoinQuizResults()
        {
            List<string> quizInfo = new List<string>();
            AccessDatabase accessDatabase = new AccessDatabase();
            quizInfo = accessDatabase.ReadCurrentJoinQuizInfo();

            string maxQuestions = quizInfo[5];
            string quizNumber = quizInfo[1];
            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\QuizManager\\Config\\";
            path = string.Concat(path, "Answers", quizNumber, ".txt");

            FileHandler fileHandler = new FileHandler();
            string[] answers = fileHandler.ReadAllLines(path);

            List<string> correctAnswers = accessDatabase.GetJoinQuizTableInfo(quizNumber, 1);

            int gotRight = 0;

            for (int count = 0; count < correctAnswers.Count; count++)
            {
                if (correctAnswers[count] == answers[count])
                {
                    gotRight++;
                }
            }

            return string.Concat(gotRight.ToString(), " out of ", maxQuestions);
        }

        public string ShowNavigationBar()
        {
            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> userDetails = accessDatabase.ReadUserInfo();
            if (string.IsNullOrEmpty(userDetails[0]) || string.IsNullOrEmpty(userDetails[1]) || string.IsNullOrEmpty(userDetails[2]))
            {
                return "hidden";
            }
            if (userDetails[2] != "Teacher")
            {
                return "hidden";
            }
            else
            {
                return "";
            }

        }

        public bool ShowLogOutButton()
        {
            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> userDetails = accessDatabase.ReadUserInfo();
            if (string.IsNullOrEmpty(userDetails[0]) || string.IsNullOrEmpty(userDetails[1]) || string.IsNullOrEmpty(userDetails[2]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ShowJoinQuizNumber()
        {
            if (string.IsNullOrEmpty(_QuizNumber))
            {
                _QuizNumber = GetCurrentQuizNumber();
            }

            return _QuizNumber;
        }
    }
}