using UnityEngine;

[System.Serializable]
public class Worker
{
    private static readonly bool addBonusTime = false; //WIP; I don't know which plays better

    public delegate void WorkEvent();
    public WorkEvent OnFired;

    public delegate void WordEvent(Worker w);
    public WordEvent OnRequestNewWord;
    public WordEvent OnFinishWord;

    public float newAskDuration = 10f;
    public float wordCompletePause = 1f;

    public float LastWordRequestedTime { get; protected set;  }
    public float LastWordCompleteTime { get; protected set;  }
    public float AskDuration { get; protected set; }
    public string CurrentWord { get; protected set; }
    public int WorkDone { get; protected set; }

    public bool WordComplete {
        get { return WorkDone >= CurrentWord.Length - 1; }
    }

    public bool HasRequestedWord
    {
        get { return !string.IsNullOrEmpty(CurrentWord); }
    }

    public bool Fired { get; private set; }

    public float TimeRemainingOnAsk
    {
        get { return (LastWordRequestedTime + AskDuration) - Time.time; }
    }

    public float TimeRemainingOnCompletionDelay
    {
        get { return (LastWordCompleteTime + wordCompletePause) - Time.time; }
    }

    public Worker()
    {
    }

    public void SetWord(string newWord)
    {
        LastWordRequestedTime = Time.time;
        if (addBonusTime)
            AskDuration = Mathf.Max(TimeRemainingOnAsk, 0) + newAskDuration;
        else
            AskDuration = newAskDuration;

        CurrentWord = newWord;
        WorkDone = 0;
    }


    public void FireMe()
    {
        Fired = true;
        if(OnFired != null)
            OnFired.Invoke();
    }

    public void DoWork()
    {
        if (WordComplete) return; //If the word's already complete, don't do any more work.
        WorkDone++;
        if (WordComplete)
        {
            //Word is finished
            Debug.Log("The word " + CurrentWord + " was completed.");
            LastWordCompleteTime = Time.time;
            if(OnFinishWord != null)
                OnFinishWord.Invoke(this);
        }
    }

    public void UpdateWorker()
    {
        if (!WordComplete)
        {
            if (!Fired && TimeRemainingOnAsk < 0)
            {
                FireMe();
            }
        }
        else
        {
            if (TimeRemainingOnCompletionDelay < 0)
            {
                if(OnRequestNewWord != null)
                    OnRequestNewWord.Invoke(this);
            }
        }
    }
}
