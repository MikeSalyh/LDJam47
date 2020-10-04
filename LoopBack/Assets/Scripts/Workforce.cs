using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Workforce : MonoBehaviour
{
    public const int MAX_WORKERS = 12; 

    public List<Worker> workers;
    protected Worker activeWorker;

    public bool TryingToAddWorker { get; protected set; }

    public enum WorkState
    {
        workday,
        fired,
        over,
        win,
        paused
    }
    public WorkState currentWorkState = WorkState.workday;

    private void Start()
    {
        NewGame();
    }

    protected void NewGame()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentWorkState == WorkState.workday)
        {
            UpdateWorkers(Time.deltaTime);
            HandleTyping();

            //Debug:
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    TryToAddWorker();
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    for (int i = 0; i < workers.Count; i++)
            //    {
            //        HandleWorkDone();
            //        workers[i].DoWork();
            //    }
            //}
        }
    }

    protected virtual void UpdateWorkers(float deltaTime)
    {
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].UpdateWorker(deltaTime);
        }
    }

    public bool TryToAddWorker()
    {
        if (!TryingToAddWorker)
        {
            StartCoroutine(TryToAddWorkerCoroutine());
            return true;
        }
        else
        {
            Debug.Log("Cannot add a new worker; already trying to add one");
            return false;
        }
    }


    private bool CheckAllWorkersDone()
    {
        for (int i = 0; i < workers.Count; i++)
            if (!workers[i].WordComplete)
                return false;

        return true;
    }

    private IEnumerator TryToAddWorkerCoroutine()
    {
        TryingToAddWorker = true;
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].readyForNewWord = false;
        }
        yield return new WaitUntil(
            () => CheckAllWorkersDone());

        yield return new WaitForSeconds(0.15f);
        AddWorker();
        yield return new WaitUntil(
            () => CheckAllWorkersDone());

        //Check if the player has won the game
        if (workers.Count >= MAX_WORKERS)
        {
            //the player has won the game!
            HandleWinGame(workers[workers.Count-1]);
        }
        else
        {

            yield return new WaitForSeconds(1.25f);
            for (int i = 0; i < workers.Count; i++)
            {
                workers[i].readyForNewWord = true;
                workers[i].timeRemainingOnCompletionDelay += (i / 8f) + (Random.value / 4f); //a lil shake so they don't all resume at once.
            }
            TryingToAddWorker = false;
        }
    }

    protected virtual Worker AddWorker()
    {
        Worker newWorker = new Worker();
        newWorker.OnRequestNewWord += GenerateNewWord;
        newWorker.OnFinishWord += HandleWordFinished;
        newWorker.OnFired += HandleWorkerFired;
        workers.Add(newWorker);

        if (workers.Count == 1)
            newWorker.SetWord("ant");
        else if (workers.Count == MAX_WORKERS)
            newWorker.SetWord("queen");
        else
            GenerateNewWord(newWorker);

        newWorker.selected = true;
        MetagameManager.instance.numWorkers = workers.Count;
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
        HandleGameOver();
    } 

    protected virtual IEnumerator DoFiredWorkerAnimation(Worker w)
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
                HandleWorkDone();
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
                    HandleWorkDone();
                    break;
                }
            }
        }       
    }

    protected virtual void HandleWorkDone()
    {
    }

    protected virtual void HandleWordFinished(Worker w)
    {
        ReleaseActiveWorker(w);
        MetagameManager.instance.score++;
    }

    protected void HandleWinGame(Worker w)
    {
        if (currentWorkState == WorkState.workday)
        {
            StartCoroutine(HandleWinCoroutine(w));
        }
    }

    protected IEnumerator HandleWinCoroutine(Worker w)
    {
        currentWorkState = WorkState.win;
        for (int i = 0; i < workers.Count; i++)
            workers[i].ResetCurrentWord();
        ReleaseActiveWorker(activeWorker);
        yield return DoWinAnimation(w);
        HandleGameOver();
    }

    protected virtual IEnumerator DoWinAnimation(Worker w)
    {
        yield return new WaitForEndOfFrame();
    }
}
