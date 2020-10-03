﻿using UnityEngine;

[System.Serializable]
public class Worker
{
    private static readonly bool addBonusTime = false; //WIP; I don't know which plays better

    public delegate void WorkEvent(Worker w);
    public WorkEvent OnFired;
    public WorkEvent OnSelected;

    public delegate void WordEvent(Worker w);
    public WordEvent OnRequestNewWord;
    public WordEvent OnFinishWord;

    public float newAskDuration = 3f;
    public float wordCompletePause = 1f;

    public GameObject visualRepresentation;

    public string CurrentWord { get; protected set; }
    public int WorkDone { get; protected set; }

    public bool WordComplete {
        get { return WorkDone >= CurrentWord.Length - 1; }
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

    public float TimeRemainingOnCompletionDelay
    {
        get; protected set;
        //get { return (LastWordCompleteTime + wordCompletePause) - Time.time; }
    }

    public char NextLetter
    {
        get { return CurrentWord[WorkDone]; }
    }

    public Worker()
    {
    }

    public void SetWord(string newWord)
    {
        TimeRemainingOnAsk = newAskDuration;
        CurrentWord = newWord;
        WorkDone = 0;
    }

    public void ResetCurrentWord()
    {
        if (!WordComplete)
        {
            WorkDone = 0;
            TimeRemainingOnAsk = newAskDuration;
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
            Debug.Log("The word " + CurrentWord + " was completed.");
            TimeRemainingOnCompletionDelay = wordCompletePause;
            if(OnFinishWord != null)
                OnFinishWord.Invoke(this);
        }
    }

    public void UpdateWorker(float deltaTime)
    {
        if(TimeRemainingOnAsk > 0)
            TimeRemainingOnAsk -= deltaTime;
        if (TimeRemainingOnCompletionDelay > 0)
            TimeRemainingOnCompletionDelay -= deltaTime;

        if (!WordComplete)
        {
            if (!Fired && TimeRemainingOnAsk < 0)
            {
                FireMe();
            }
        }
        else
        {
            if (!Fired && TimeRemainingOnCompletionDelay < 0)
            {
                if(OnRequestNewWord != null)
                    OnRequestNewWord.Invoke(this);
            }
        }
    }
}
