using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public Button Hamburger;
    public GameObject PauseMenuUI;
    public Text MultiplierButtonText;
    int Price = 200 * EdgeCollision.multiplier;

    public void OpenMenu()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void BuyMultiplier()
    {
        if (EdgeCollision.points >= Price)
        {
            EdgeCollision.points -= Price;
            EdgeCollision.multiplier *= 2;
            Price = (int) Math.Round(Price * 2.1);

            MultiplierButtonText.text = $"{EdgeCollision.multiplier*2}x Multiplier - {Price}";
        }
    }
}