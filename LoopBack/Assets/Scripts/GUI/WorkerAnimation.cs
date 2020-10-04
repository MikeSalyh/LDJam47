using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class AnimFrames
{
    public string name;
    public Sprite[] sprites;
}

public class WorkerAnimation : MonoBehaviour
{
    public const float animDuration = 1f;
    [SerializeField] private AnimFrames[] allAnims;
    private int currentFrame = 0;
    public int CharacterIndex { get; private set; }

    private Image img;

    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void SetCharacter(int value)
    {
        CharacterIndex = value;
        img.sprite = allAnims[CharacterIndex].sprites[0];
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float timePerFrame = animDuration / allAnims[CharacterIndex].sprites.Length;
        int i = 0;
        while (i < allAnims[CharacterIndex].sprites.Length)
        {
            img.sprite = allAnims[CharacterIndex].sprites[i];
            i++;
            yield return new WaitForSeconds(timePerFrame);
        }
    }
}
