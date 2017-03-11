namespace Model.Models
{
    public interface ISubQuestion
    {
        string questionID { get; set; }
        string subQuestionID { get; set; }
        string questionText { get; set; }
        string answerText { get; set; }
        string flags { get; set; }

        int AnswerScore { get; set; }
    }
}