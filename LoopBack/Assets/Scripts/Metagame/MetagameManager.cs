using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MetagameManager : MonoBehaviour
{
    public static MetagameManager instance;
    public CanvasGroup dimPlane;
    public int score = 0;
    public int level = 0;

    private int _numWorkers;
    public int NumWorkers
    {
        get { return _numWorkers; }
        set {
            _numWorkers = value;
            if (value > MaxWorkersUnlocked)
                MaxWorkersUnlocked = value;
        }
    }
    public int MaxWorkersUnlocked = 1; //no saving.

    public enum GameState
    {
        Init,
        Menu,
        Gameplay,
        Finale,
        Success,
        Loading
    }

    private GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            if (dimPlane.alpha > 0f)
                dimPlane.alpha = 0f;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GoToGameplay()
    {
        if (currentState != GameState.Loading)
            StartCoroutine(GoToGameplayCoroutine());
    }

    private IEnumerator GoToGameplayCoroutine()
    {
        SwitchState(GameState.Loading);
        dimPlane.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Gameplay");
        SwitchState(GameState.Gameplay);
        dimPlane.DOFade(0f, 0.5f);
    }

    public void GoToMenu()
    {
        if (currentState != GameState.Loading)
            StartCoroutine(GoToMenuCoroutine());
    }

    private IEnumerator GoToMenuCoroutine()
    {
        SwitchState(GameState.Loading);
        dimPlane.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Menu");
        SwitchState(GameState.Menu);
        dimPlane.DOFade(0f, 0.5f);
    }

    public void GoToSuccess()
    {
        if (currentState != GameState.Loading)
            StartCoroutine(GoToSuccessCoroutine());
    }

    private IEnumerator GoToSuccessCoroutine()
    {
        SwitchState(GameState.Loading);
        dimPlane.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Success");
        SwitchState(GameState.Success);
        dimPlane.DOFade(0f, 0.5f);
    }

    public void GoToFinale()
    {
        if (currentState != GameState.Loading)
            StartCoroutine(GoToFinaleCoroutine());
    }

    private IEnumerator GoToFinaleCoroutine()
    {
        SwitchState(GameState.Loading);
        dimPlane.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Finale");
        SwitchState(GameState.Finale);
        dimPlane.DOFade(0f, 0.5f);
    }


    public void SwitchState(GameState value)
    {
        if (currentState == value)  //If it's already in this state, do nothing.
            return;

        currentState = value;
        switch (value)
        {
            case GameState.Menu:
                score = 0;
                level = 0;
                NumWorkers = 0;
                break;
            case GameState.Gameplay:
                break;
            case GameState.Success:
                break;
            case GameState.Finale:
                break;
        }
    }
}
