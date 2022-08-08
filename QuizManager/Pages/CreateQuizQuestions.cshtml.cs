using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuizManager.Pages
{
    public class CreateQuizQuestionsModel : PageModel
    {
        public void OnGet()
        {

        }
        public string GetColours(int colour)
        {
            IndexModel indexModel = new IndexModel();
            return indexModel.GetColours(colour);
        }
    }
}
