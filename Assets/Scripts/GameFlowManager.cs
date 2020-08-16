using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions;


public class GameFlowManager : MonoBehaviour
{

    [Tooltip("Target board prefab")]
    [SerializeField] TargetBoardController targetBoardPrefab;

    [Tooltip("Target board span")]
    [SerializeField] Vector2 boardSpan;

    [SerializeField] AudioClip wrongSound;

    [SerializeField] AudioClip correctSound;

    [SerializeField] List<int> additionTableRowIdx;

    [SerializeField] List<int> substractionTableRowIdx;

    [SerializeField] List<int> multiplicationTableRowIdx;

    [SerializeField] bool randomization;

    [SerializeField] int numOfQuestion;

    [SerializeField] float waitResultTime;

    [SerializeField] float timeout;
    [SerializeField] float lastQuizTime;

    [SerializeField] TargetBoardController infoBoardPrefab;

    IEnumerator<Quiz> quizEnumerator;

    Answer? lastAnswer = null;

    AudioSource sound;

    List<TargetBoardController> targetBoardList = new List<TargetBoardController>();

    TargetBoardController infoBoard;
    TargetBoardController timeBoard;

    int score = 0;

    void PrepareQuiz(in Quiz q)
    {
        Assert.IsTrue(targetBoardList.Count == 0);
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var v = i * 10 + j;
                var board = Instantiate(targetBoardPrefab, gameObject.transform, false);
                board.transform.localPosition = new Vector3(boardSpan.x * j, boardSpan.y * (9 - i), 0);
                board.SetText(v.ToString());

                if (v == q.answer)
                {
                    board.RegisterBangListener(OnCurrectAnswer);
                }
                else
                {
                    board.RegisterBangListener(OnWrongAnswer);

                }
             
                targetBoardList.Add(board);
            }
        }

        infoBoard.SetText(q.exp);
        infoBoard.RegisterBangListener(OnWrongAnswer);
        lastQuizTime = Time.time;
        lastAnswer = null;
    }

    void ClearTargetBoard()
    {
        foreach (var b in targetBoardList)
        {
            b.Kill();
        }
        targetBoardList.Clear();
    }

    void StepQuiz()
    {
        if (quizEnumerator.MoveNext())
        {
            PrepareQuiz(quizEnumerator.Current);
        }
        else
        {
            ShowResult();
        }
    }

    void ShowResult()
    {
        lastAnswer = null;
        infoBoard.SetText($"Score {score}");
    }

    void JudgeAnswer(bool ansCorrect, float v)
    {
        if (ansCorrect && correctSound)
        {
            sound.PlayOneShot(correctSound);
            score += (int)v + 2;
        }
        else if (!ansCorrect && wrongSound)
        {
            sound.PlayOneShot(wrongSound);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        infoBoard = Instantiate(infoBoardPrefab, gameObject.transform, false);
        infoBoard.transform.localPosition = new Vector3(boardSpan.x * 5, boardSpan.y * (11), 0);
        quizEnumerator = QuizFactory.MakeQuiz(additionTableRowIdx, substractionTableRowIdx, multiplicationTableRowIdx, numOfQuestion, randomization);

        StepQuiz();

        timeBoard = Instantiate(infoBoardPrefab, gameObject.transform, false);
        timeBoard.transform.localPosition = new Vector3(boardSpan.x * 5, boardSpan.y * (12), 0);
    }

    // Update is called once per frame
    void Update()
    {
        float v = timeout - Time.time + lastQuizTime;

        if (lastAnswer.HasValue)
        {
            if (Time.time - lastAnswer.Value.t > waitResultTime)
            {
                JudgeAnswer(lastAnswer.Value.correct, v);
                StepQuiz();
            }
        }
        else if (targetBoardList.Count != 0)
        {
            if (v <= 0)
            {
                infoBoard.Bang();
                
            }
            else
            {
                timeBoard.SetText($"{(int)v}");
            }
        }



    }

    void OnCurrectAnswer()
    {
        ClearTargetBoard();
        lastAnswer = new Answer(true, Time.time);
    }

    void OnWrongAnswer()
    {
        ClearTargetBoard();
        lastAnswer = new Answer(false, Time.time);
    }
}
