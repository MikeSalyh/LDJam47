using UnityEngine;

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

    public float wordCompletePause = 1f;

    public GameObject visualRepresentation;

    public string CurrentWord { get; protected set; }
    public int WorkDone { get; protected set; }
    public float latestAskDuration { get; protected set; }

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
        latestAskDuration = DifficultyManager.GetAskDuration();
        TimeRemainingOnAsk = latestAskDuration;
        CurrentWord = newWord;
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
            TimeRemainingOnCompletionDelay = wordCompletePause;
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
