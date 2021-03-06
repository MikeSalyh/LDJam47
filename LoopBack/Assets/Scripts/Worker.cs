﻿using UnityEngine;

[System.Serializable]
public class Worker
{
    public delegate void WorkEvent(Worker w);
    public WorkEvent OnFired;
    public WorkEvent OnSelected;

    public delegate void WordEvent(Worker w);
    public WordEvent OnRequestNewWord;
    public WordEvent OnFinishWord;

    public bool selected;

    public float wordCompletePause = 1.25f;

    public GameObject visualRepresentation;
    public bool readyForNewWord = true;

    public string CurrentWord { get; protected set; }
    public int WorkDone { get; protected set; }
    public float latestAskDuration { get; protected set; }

    public bool WordComplete {
        get { return WorkDone >= CurrentWord.Length; }
    }

    public bool HasRequestedWord
    {
        get { return !string.IsNullOrEmpty(CurrentWord); }
    }

    public bool Fired { get; protected set; }

    public float TimeRemainingOnAsk
    {
        get; protected set;
        //get { return (LastWordRequestedTime + AskDuration) - Time.time; }
    }

    public float timeRemainingOnCompletionDelay;

    public char NextLetter
    {
        get { return CurrentWord[WorkDone]; }
    }

    public Worker()
    {
    }

    public void SetWord(string newWord)
    {
        latestAskDuration = DifficultyManager.GetAskDuration();
        TimeRemainingOnAsk = latestAskDuration;
        CurrentWord = newWord.Trim();
        WorkDone = 0;
    }

    public void ResetCurrentWord()
    {
        if (!WordComplete)
        {
            WorkDone = 0;
            latestAskDuration = DifficultyManager.GetAskDuration();
            TimeRemainingOnAsk = latestAskDuration;
            selected = false;
        }
    }


    public void FireMe()
    {
        Fired = true;
        CurrentWord = string.Empty;
        if(OnFired != null)
            OnFired.Invoke(this);
    }

    public void DoWork()
    {
        if (WordComplete) return; //If the word's already complete, don't do any more work.
        WorkDone++;
        if (WordComplete)
        {
            //Word is finished
            timeRemainingOnCompletionDelay = wordCompletePause;
            if(OnFinishWord != null)
                OnFinishWord.Invoke(this);
        }
    }

    public void UpdateWorker(float deltaTime)
    {
        if (selected)
            TimeRemainingOnAsk = latestAskDuration;

        if(TimeRemainingOnAsk > 0 && !selected)
            TimeRemainingOnAsk -= deltaTime;
        if (timeRemainingOnCompletionDelay > 0)
            timeRemainingOnCompletionDelay -= deltaTime;

        if (!WordComplete)
        {
            if (!Fired && TimeRemainingOnAsk < 0)
            {
                FireMe();
            }
        }
        else
        {
            if (!Fired && timeRemainingOnCompletionDelay < 0 && readyForNewWord)
            {
                if(OnRequestNewWord != null)
                    OnRequestNewWord.Invoke(this);
            }
        }
    }
}
