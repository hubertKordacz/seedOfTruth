using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    enum MenuState
    {
        NONE = 0,
        SELECT = 1,
        LOADING = 2,
        GAMEPLAY = 3,
        CURRENT_RESULT =4,
        FINAL_RESULT = 5
        SPLASH = 6
    }


    public List<string> levels = new List<string>();
    public List<PlayerMenuSlot> menuSlots = new List<PlayerMenuSlot>();
    public RectTransform selectScreen;
    public RectTransform loadingScreen;
    public RectTransform splashScreen;
    public RectTransform gameOverScreen;
    public AudioClip music;
    public int maxWins= 5;
    private List<int> score = new List<int>();
    private int currentSceneIndex=0;
    private MenuState state;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < 4; i++)
            score.Add(0);
      var   audioSource = GetComponent<AudioSource>();
        if (audioSource != null && music != null)
            audioSource.PlayOneShot(music);
    }
    void Start()
    {
        if (selectScreen)
            selectScreen.gameObject.SetActive(false);
        if (loadingScreen)
            loadingScreen.gameObject.SetActive(false);
        if (gameOverScreen)
            gameOverScreen.gameObject.SetActive(false);
        if (splashScreen)
            splashScreen.gameObject.SetActive(true);


        currentSceneIndex = -1;

        state = MenuState.SPLASH;
        foreach (var item in menuSlots)
        {
            item.Unselect();
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case MenuState.SPLASH:
                {
                    UpdateSplash();
                    break;
                }
            case MenuState.SELECT:
                {
                    UpdateSelect();
                    break;
                }
            case MenuState.CURRENT_RESULT:
                {
                    UpdateSelect();
                    break;
                }
            default:
                break;
        }


    }
    public bool IsPlaing(InputID playerId)
    {
        foreach (var item in menuSlots)
        {
            if (item.player == playerId)
                return item.IsSelected;
        }
        return false;
    }
    private void UpdateSelect()
    {
        for (int i = 0; i < 4; i++)
        {
            var player = ReInput.players.GetPlayer(i);

            if (player!=null && player.GetButtonDown("start"))
            {
                int selected = 0;
                foreach (var item in menuSlots)
                {
                    if (item.IsSelected)
                        selected++;
                }

                if (selected > 1)
                {

                    LoadNextLevel();
                    return;
                }

           
            }
        }
    }
    private void UpdateResult()
    {
        for (int i = 0; i < 4; i++)
        {
            var player = ReInput.players.GetPlayer(i);

            if (player != null && player.GetButtonDown("start"))
            {
              
                    LoadNextLevel();
                return;

            }
        }
    }
    private void UpdateSplash()
    {
        for (int i = 0; i < 4; i++)
        {
            var player = ReInput.players.GetPlayer(i);

            if (player != null && player.GetAnyButton())
            {
               state= MenuState.SELECT;

                if (splashScreen)
                    splashScreen.gameObject.SetActive(false);

                if (selectScreen)
                    selectScreen.gameObject.SetActive(true);
            }
        }
    }
    private void LoadNextLevel()
    {
        currentSceneIndex++;
        state = MenuState.LOADING;
        if (selectScreen)
            selectScreen.gameObject.SetActive(false);

        if (loadingScreen)
            loadingScreen.gameObject.SetActive(true);

        StartCoroutine(LoadScene());
    

    }

    private IEnumerator LoadScene()
    {
        var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levels[currentSceneIndex], UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        StartGameplay();

        yield return null;
    }

    private void StartGameplay()
    {
        state = MenuState.LOADING;
      

        if (loadingScreen)
            loadingScreen.gameObject.SetActive(false);
    }

    public void StopGameplay(InputID  winnerID)
    {

        int winner = 0;

        foreach (var item in menuSlots)
        {
            if (item.player == winnerID)
                winner = menuSlots.IndexOf(item);
        }

        score[winner]++;
        Debug.Log("StopGameplay | winner score: " + score[winner]);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(levels[currentSceneIndex]);
        if (score[winner] >= maxWins)
        {
            ShowFinalResult();
        }
        else
        {
            ShowCurrentResult(winner);
        }
    }

    private void ShowCurrentResult(int winner)
    {

        if (selectScreen)
            selectScreen.gameObject.SetActive(true);

        state = MenuState.CURRENT_RESULT;
        for (int i = 0; i < 4; i++)
        {
            menuSlots[i].Result(score[i], false);
        }
       

    }

    private void ShowFinalResult()
    {

        state = MenuState.FINAL_RESULT;
        for (int i = 0; i < 4; i++)
        {
            menuSlots[i].Result(score[i], true);
        }
        if (selectScreen)
            selectScreen.gameObject.SetActive(true);
        if (gameOverScreen)
            gameOverScreen.gameObject.SetActive(true);
    }
}
