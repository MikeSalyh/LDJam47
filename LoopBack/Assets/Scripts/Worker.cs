using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Worker
{
    [Range(2,8)]
    public int wordLength = 3;
    public float lastWordRequestedTime = 0;
    public float newAskDuration = 10f;
    public float AskDuration { get; private set; }

    public string requestedWord;
    public bool HasRequestedWord
    {
        get { return !string.IsNullOrEmpty(requestedWord); }
    }

    public float TimeRemainingOnAsk
    {
        get { return (lastWordRequestedTime + AskDuration) - Time.time; }
    }

    public Worker()
    {
    }

    public void RequestNewWord()
    {
        lastWordRequestedTime = Time.time;
        AskDuration = Mathf.Max(TimeRemainingOnAsk, 0) + newAskDuration;
        requestedWord = WordParser.GetRandomWord(wordLength);
    }

    public void RequestNewWord(int wordLength)
    {
        this.wordLength = wordLength;
        RequestNewWord();
    }
}
