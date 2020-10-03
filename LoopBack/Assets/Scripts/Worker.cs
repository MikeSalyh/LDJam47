using UnityEngine;

[System.Serializable]
public class Worker
{
    private static readonly bool addBonusTime = false; //WIP; I don't know which plays better

    public delegate void WorkEvent();
    public WorkEvent OnFinishWord;
    public WorkEvent OnFired;
    
    [Range(2,8)]
    public int wordLength = 3;
    public float newAskDuration = 10f;
    public float wordCompletePause = 1f;

    public float LastWordRequestedTime { get; protected set;  }
    public float LastWordCompleteTime { get; protected set;  }
    public float AskDuration { get; protected set; }
    public string RequestedWord { get; protected set; }
    public int WorkDone { get; protected set; }

    public bool WordComplete {
        get { return WorkDone >= wordLength; }
    }

    public bool HasRequestedWord
    {
        get { return !string.IsNullOrEmpty(RequestedWord); }
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

    public void RequestNewWord()
    {
        LastWordRequestedTime = Time.time;
        if (addBonusTime)
            AskDuration = Mathf.Max(TimeRemainingOnAsk, 0) + newAskDuration;
        else
            AskDuration = newAskDuration;

        RequestedWord = WordParser.GetRandomWord(wordLength);
        WorkDone = 0;
    }

    public void RequestNewWord(int wordLength)
    {
        this.wordLength = wordLength;
        RequestNewWord();
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
            LastWordCompleteTime = Time.time;
            if(OnFinishWord != null)
                OnFinishWord.Invoke();
        }
    }

    public void UpdateWorker()
    {
        if (WorkDone < wordLength)
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
                RequestNewWord();
            }
        }
    }
}
