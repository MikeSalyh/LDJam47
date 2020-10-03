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
    public float lastWordRequestedTime = 0;
    public float newAskDuration = 10f;
    public float AskDuration { get; private set; }
    public int WorkDone { get; private set; }

    public string RequestedWord { get; private set; }
    public bool HasRequestedWord
    {
        get { return !string.IsNullOrEmpty(RequestedWord); }
    }

    public bool Fired { get; private set; }

    public float TimeRemainingOnAsk
    {
        get { return (lastWordRequestedTime + AskDuration) - Time.time; }
    }

    public Worker()
    {
        OnFinishWord += RequestNewWord;
    }

    public void RequestNewWord()
    {
        lastWordRequestedTime = Time.time;

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
        OnFired.Invoke();
        //requestedWord = string.Empty;
    }

    public void DoWork()
    {
        WorkDone++;
        if (WorkDone >= wordLength)
        {
            OnFinishWord.Invoke();
        }
    }
}
