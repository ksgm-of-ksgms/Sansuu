using System.Collections;
using System.Collections.Generic;
using System.Linq;

struct Quiz
{
    public string exp;
    public int answer;

    public Quiz(string exp, int answer) { this.exp = exp; this.answer = answer; }
}

struct Answer
{
    public bool correct;
    public float t;
    public Answer(bool correct, float t) { this.correct = correct; this.t = t; }
}

static class QuizFactory
{
    public static IEnumerable<Quiz> AdditionTableRowGenerator(int idx)
    {
        if (idx < 0 || idx > 9)
        {
            yield break;
        }

        for (int i = 0; i <= 10; ++i)
        {
            yield return new Quiz($"{idx} + {i} = ?", i + idx);
        }
    }


    public static IEnumerable<Quiz> SubstractionTableRowGenerator(int idx)
    {
        if (idx < 0 || idx > 9)
        {
            yield break;
        }

        for (int i = 0; i <= 20; ++i)
        {
            if (i > idx)
            {
                yield return new Quiz($"{i} - {idx} = ?", i - idx);
            }
        }
    }


    public static IEnumerable<Quiz> MultiplicationTableRowGenerator(int idx)
    {
        if (idx < 0 || idx > 10)
        {
            yield break;
        }

        for (int i = 0; i < 10; ++i)
        {
            yield return new Quiz($"{idx} * {i} = ?", i * idx);
        }

    }

    public static IEnumerator<Quiz> MakeQuiz(List<int> additionTableRowIdx, List<int> substractionTableRowIdx, List<int> multiplicationTableRowIdx, int numOfQuestion=10, bool randomization=true)
    {
        var quizList = new List<Quiz>();

        foreach (int i in additionTableRowIdx)
        {
            quizList.AddRange(AdditionTableRowGenerator(i).ToList());
        }

        foreach (int i in substractionTableRowIdx)
        {
            quizList.AddRange(SubstractionTableRowGenerator(i).ToList());
        }

        foreach (int i in substractionTableRowIdx)
        {
            quizList.AddRange(SubstractionTableRowGenerator(i).ToList());
        }

        if (randomization)
        {
            quizList = quizList.OrderBy(a => System.Guid.NewGuid()).ToList();
        }

        if (numOfQuestion > 0)
        {
            quizList = quizList.Take(numOfQuestion).ToList();
        }

        return quizList.GetEnumerator();
    }

}