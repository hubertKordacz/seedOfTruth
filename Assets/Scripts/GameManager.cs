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
    }


    public List<string> levels = new List<string>();
    public List<PlayerMenuSlot> menuSlots = new List<PlayerMenuSlot>();
    public RectTransform selectScreen;
    public RectTransform loadingScreen;
    public int maxWins= 5;
    private List<int> score = new List<int>();
    private int currentSceneIndex=0;
    private MenuState state;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {


        currentSceneIndex = -1;
        if (selectScreen)
            selectScreen.gameObject.SetActive(true);

        state = MenuState.SELECT;
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
                    LoadNextLevel();

           
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
    }
}
