using System.Collections.Generic;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // QUESTION TYPE ENUM
    // Used so the Quiz GUI knows whether to draw 4 multiple-choice buttons
    // or just 2 true/false buttons for a given question
    // -----------------------------------------------------------------------
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse
    }


    // -----------------------------------------------------------------------
    // QUIZ QUESTION CLASS
    // Simple data model representing one cybersecurity quiz question,
    // its possible answers, the correct answer, and the explanation shown
    // to the user as feedback after they answer
    // -----------------------------------------------------------------------
    public class QuizQuestion
    {// start of class

        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
        public QuestionType Type { get; set; }

        public QuizQuestion(string question, List<string> options, int correctIndex, string explanation, QuestionType type)
        {
            Question = question;
            Options = options;
            CorrectIndex = correctIndex;
            Explanation = explanation;
            Type = type;
        }

    }// end of class
}// end of namespace
