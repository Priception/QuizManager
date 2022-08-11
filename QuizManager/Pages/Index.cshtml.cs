using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizManager.Scripts.DatabaseScripts;

namespace QuizManager.Pages
{
    public class IndexModel : PageModel
    {

        string _QuizName { get; set; }
        string _QuizNumber { get; set; }

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
            string quizName = Request.Form["QuizName"];
            string quizType = Request.Form["QuizType"];

            AccessDatabase accessDatabase = new AccessDatabase();
            int joinValue = accessDatabase.AddNewQuizInfoToDatabase(quizName, quizType);

            string joinName = accessDatabase.CreateNewQuestionTable(joinValue);

            accessDatabase.WriteToCurrentQuiz(quizName, joinName);

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
            string colour1 = Request.Form["answer1pick"];
            string colour2 = Request.Form["answer2pick"];
            string colour3 = Request.Form["answer3pick"];
            string colour4 = Request.Form["answer4pick"];



            return RedirectToPage("CreateQuizQuestions");
        }

        public void OnPostCreateQuizInfoDelete()
        {
            
        }


        public string GetCurrentQuizName()
        {
            if (_QuizName == null)
            {
                AccessDatabase accessDatabase = new AccessDatabase();
                List<string> currentQuiz = accessDatabase.ReadCurrentQuizInfo();
                _QuizName = currentQuiz[0].ToString();
                _QuizNumber = currentQuiz[1].ToString();

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
                _QuizName = currentQuiz[0].ToString();
                _QuizNumber = currentQuiz[1].ToString();

                return _QuizNumber;
            }
            else
            {
                return _QuizNumber;
            }
        }

        //public string GetCurrentQuestionNumber()
        //{
        //    string quizNumber = GetCurrentQuizNumber();

        //}

    }
}