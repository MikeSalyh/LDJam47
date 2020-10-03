using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Workforce : MonoBehaviour
{
    public List<Worker> workers;
    protected Worker activeWorker;
    public int wordLength = 3;

    public enum WorkState
    {
        workday,
        onbreak,
        fired,
        paused
    }
    public WorkState currentWorkState = WorkState.workday;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentWorkState == WorkState.workday)
        {
            UpdateWorkers(Time.deltaTime);
            HandleInput();

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

    protected virtual Worker AddWorker()
    {
        Worker newWorker = new Worker();
        newWorker.OnRequestNewWord += GenerateNewWord;
        newWorker.OnFinishWord += ReleaseActiveWorker;
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
        currentWorkState = WorkState.workday;
    }

    protected virtual IEnumerator DoFiredWorkerAnimation(Worker w)
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator DoResumeWorkdayAnimation(Worker w)
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual void GenerateNewWord(Worker w)
    {
        List<char> bannedCharacters = new List<char>();
        for (int i = 0; i < workers.Count; i++)
        {
            if (!workers[i].Fired && !string.IsNullOrEmpty(workers[i].CurrentWord))
                bannedCharacters.Add(workers[i].CurrentWord[0]);
        }
        w.SetWord(WordParser.GetRandomWord(wordLength, bannedCharacters.ToArray()));
    }

    protected virtual void ReleaseActiveWorker(Worker w)
    {
        activeWorker = null;
    }

    private void HandleInput()
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
                    if(workers[i].OnSelected != null)
                        workers[i].OnSelected.Invoke(workers[i]); //This is a little janky, no?
                    activeWorker.DoWork();
                    break;
                }
            }
        }
    }
}
