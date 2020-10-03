using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordParser : MonoBehaviour
{
    public TextAsset[] wordSourceFiles;
    public static string[][] allWords;
    private static int shortedWordLength;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWordsFromCSVs();
    }

    private void GenerateWordsFromCSVs()
    {
        allWords = new string[wordSourceFiles.Length][];
        for (int i = 0; i < wordSourceFiles.Length; i++)
        {
            allWords[i] = wordSourceFiles[i].text.Split('\n');
        }
        shortedWordLength = allWords[0][0].Length;
        Debug.Log("Generated all the words");
    }

    public static string GetRandomWord(int charLength, char[] bannedCharacters = null, int maxAttempts = 10)
    {
        string suggestedWord = allWords[charLength - shortedWordLength + 1][UnityEngine.Random.Range(0, allWords[charLength - shortedWordLength + 1].Length)].ToLowerInvariant();
        if (bannedCharacters == null)
            return suggestedWord;

        bool reroll = false;
        for (int i = 0; i < bannedCharacters.Length; i++)
        {
            if (suggestedWord[0] == bannedCharacters[i])
            {
                Debug.Log("regenerating word [" + suggestedWord + "] because the letter " + bannedCharacters[i] + " is already in use.");
                reroll = true;
            }
        }

        if (reroll && maxAttempts > 0)
        {
            //Re-run the randomization if the letter is already in use.
            maxAttempts--;
            return GetRandomWord(charLength, bannedCharacters, maxAttempts);
        }
        else
        {
            if (maxAttempts <= 0)
                Debug.LogWarning("Two words with the same letter were generated.");

            return suggestedWord;
        }
    }
}
