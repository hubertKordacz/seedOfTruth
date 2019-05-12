using UnityEngine;
using System.Collections;
using Rewired;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerMenuSlot : MonoBehaviour
{
    public Transform selectMarker;
    public TextMeshProUGUI scoreLabel;
  
    public InputID player;
    private bool isSelected;

    public bool IsSelected { get => isSelected;}
    private bool isSelectable = false;
    private void Update()
    {
        var rePlayer = ReInput.players.GetPlayer((int)player);
        if (!isSelectable && rePlayer != null && rePlayer.GetButtonDown("select"))
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
        if (selectMarker)
            selectMarker.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        isSelected = false;
        if(selectMarker)
        selectMarker.gameObject.SetActive(false);

    }

   
    internal void Result(int score, bool final)
    {
        isSelectable = false;
       
        UpdateScore(score);
    }

    private void UpdateScore(int score)
    {
        if (scoreLabel)
            scoreLabel.text =  score.ToString() + " Points";
    }
}
