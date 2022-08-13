using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizManager.Scripts.DatabaseScripts;

namespace QuizManager.Pages
{
    public class IndexModel : PageModel
    {

        string _QuizName { get; set; }
        string _QuizNumber { get; set; }
        string _currentQuestion { get; set; }
        string _maxQuestions { get; set; }
        string _IDValue { get; set; }


        public void OnGet()
        {

        }
        public void OnGetCreateQuizQuestions()
        {

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









    }
}