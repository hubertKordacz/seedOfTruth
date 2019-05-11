using UnityEngine;
using System.Collections;
using Rewired;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerMenuSlot : MonoBehaviour
{
    public CanvasGroup group;
    public TextMeshProUGUI scoreLabel;
  
    public InputID player;
    private bool isSelected;

    public bool IsSelected { get => isSelected;}
    private bool isFinal = false;
    private void Update()
    {
        var rePlayer = ReInput.players.GetPlayer((int)player);
        if (!isFinal && rePlayer != null && rePlayer.GetButtonDown("select"))
        {

            if (isSelected)
                Unselect();
            else
                Select();
        }

    }

    private void Select()
    {
        isSelected = true;
        group.alpha = 1.0f;
    }

    public void Unselect()
    {
        isSelected = false;
        group.alpha = 0.5f;

    }

   
    internal void Result(int score, bool final)
    {
        isFinal = final;
        if (!final)
            Unselect();

        UpdateScore(score);
    }

    private void UpdateScore(int score)
    {
        if (scoreLabel)
            scoreLabel.text =  score.ToString() + " Seeds";
    }
}
