using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Workforce : MonoBehaviour
{
    public const int MAX_WORKERS = 12; 

    public List<Worker> workers;
    protected Worker activeWorker;
    public int maxLives = 3;
    public int LivesLeft { get; private set; }

    public enum WorkState
    {
        workday,
        fired,
        over,
        paused
    }
    public WorkState currentWorkState = WorkState.workday;

    private void Start()
    {
        NewGame();
    }

    protected void NewGame()
    {
        LivesLeft = maxLives;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentWorkState == WorkState.workday)
        {
            UpdateWorkers(Time.deltaTime);
            HandleTyping();

            //Debug:
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddWorker();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                for (int i = 0; i < workers.Count; i++)
                {
                    workers[i].DoWork();
                }
            }
        }
    }

    protected virtual void UpdateWorkers(float deltaTime)
    {
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].UpdateWorker(deltaTime);
        }
    }

    public virtual Worker AddWorker()
    {
        Worker newWorker = new Worker();
        newWorker.OnRequestNewWord += GenerateNewWord;
        newWorker.OnFinishWord += HandleWordFinished;
        newWorker.OnFired += HandleWorkerFired;
        GenerateNewWord(newWorker);
        workers.Add(newWorker);
        return newWorker;
    }

    protected void HandleWorkerFired(Worker w)
    {
        if (currentWorkState == WorkState.workday)
        {
            StartCoroutine(HandleWorkerFiredCoroutine(w));
        }
    }

    protected IEnumerator HandleWorkerFiredCoroutine(Worker w)
    {
        currentWorkState = WorkState.fired;
        for (int i = 0; i < workers.Count; i++)
            workers[i].ResetCurrentWord();
        ReleaseActiveWorker(activeWorker);
        yield return DoFiredWorkerAnimation(w);
        yield return DoResumeWorkdayAnimation(w);

        if (LivesLeft > 0)
        {
            currentWorkState = WorkState.workday;
        }
        else
        {
            HandleGameOver();
        }
    }

    protected void LoseLife()
    {
        LivesLeft--;
    }

    protected virtual IEnumerator DoFiredWorkerAnimation(Worker w)
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator DoResumeWorkdayAnimation(Worker w)
    {
        yield return new WaitForEndOfFrame();
    }


    protected void HandleGameOver()
    {
        currentWorkState = WorkState.over;
        MetagameManager.instance.GoToFinale();
    }

    protected virtual void GenerateNewWord(Worker w)
    {
        List<char> bannedCharacters = new List<char>();
        for (int i = 0; i < workers.Count; i++)
        {
            if (!workers[i].Fired && !string.IsNullOrEmpty(workers[i].CurrentWord))
                bannedCharacters.Add(workers[i].CurrentWord[0]);
        }
        w.SetWord(WordParser.GetRandomWord(DifficultyManager.GetWordLength(), bannedCharacters.ToArray()));
    }

    protected virtual void ReleaseActiveWorker(Worker w)
    {
        if(activeWorker != null)
            activeWorker.selected = false;
        activeWorker = null;
    }

    private void HandleTyping()
    {
        string reqChar;
        if (activeWorker != null && !activeWorker.Fired && !activeWorker.WordComplete)
        {
            reqChar = activeWorker.NextLetter.ToString().ToLowerInvariant();
            if (!string.IsNullOrEmpty(reqChar) && Input.GetKeyDown(reqChar))
            {
                activeWorker.DoWork();
            }
        } else {
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].Fired || workers[i].WordComplete)
                    continue;

                reqChar = workers[i].NextLetter.ToString().ToLowerInvariant();
                if (string.IsNullOrEmpty(reqChar))
                    continue;

                if (Input.GetKeyDown(reqChar))
                {
                    activeWorker = workers[i];
                    activeWorker.selected = true;
                    if(workers[i].OnSelected != null)
                        workers[i].OnSelected.Invoke(workers[i]); //This is a little janky, no?
                    activeWorker.DoWork();
                    break;
                }
            }
        }       
    }

    private void HandleWordFinished(Worker w)
    {
        ReleaseActiveWorker(w);
        MetagameManager.instance.score++;
    }
}
