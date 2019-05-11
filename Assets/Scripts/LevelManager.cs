using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    private List<PlayerHealth> players = new List<PlayerHealth>();
    //private List<PlayerHealth> players = new List<PlayerHealth>();
    // Update is called once per frame

    private bool isFinshed = false;
    private PlayerHealth lastPlayer = null;
    private void Start()
    {
        var foundObjects = FindObjectsOfType<PlayerHealth>();

        var gameManager = GameManager.instance;
        foreach (var item in foundObjects)
        {

            var playerInput = item.GetComponent<PlayerInput>();
            var isPlaing = true;
            if (gameManager)
            {
                isPlaing = gameManager.IsPlaing(playerInput.inputID);
            }

            if (!isPlaing)
            {
                item.RemoveFromGame();
            }
            else
            {
                players.Add(item);

            }


        }
    }

    private void Update()
    {
        if (isFinshed)
            return;

        var count = 0;

        foreach (var item in players)
        {
            if(!item.IsDead)
            {
                count++;
                lastPlayer = item;
            }
        }

        if (count < 2)
            FinishLevel();
    }

    private void FinishLevel()
    {
        isFinshed = true;
        StartCoroutine(FinishLevelSequence());
    }

    private IEnumerator FinishLevelSequence()
    {
        yield return new WaitForSeconds(5);

        var gameManager = GameManager.instance;
        if (gameManager != null && lastPlayer != null)
            gameManager.StopGameplay(lastPlayer.GetComponent<PlayerInput>().inputID);
    }
}
