using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public Button Hamburger; // Icon with three horizontal lines
    public GameObject PauseMenuUI;
    public Text MultiplierButtonText;
    public Text FrictionButtonText;
    public Button FrictionButton;
    public EdgeCollision Ball;
    int MultiplierPrice = 200; // Initial prices, should be fixed when scores are changed to be persistent
    int FrictionPrice = 500;

    void Start()
    {
        Resume();
    }
    
    public void OpenMenu() // Called when hamburger icon is pressed
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

    public void BuyMultiplier() // Called when button to buy multiplier is pressed
    {
        if (EdgeCollision.points >= MultiplierPrice)
        {
            EdgeCollision.points -= MultiplierPrice;
            Ball.UpdatePoints();
            EdgeCollision.multiplier *= 2;
            MultiplierPrice = (int) Math.Round(MultiplierPrice * 2.1);

            MultiplierButtonText.text = $"{EdgeCollision.multiplier*2}x Multiplier - {MultiplierPrice}";
        }
    }

    public void BuyDecreaseFriction() // --> 0.95 --> 0.97 --> 0.99
    {
        if (EdgeCollision.points >= FrictionPrice)
        {
            switch (EdgeCollision.frictionLevel)
            {
                case EdgeCollision.FrictionState.Low:
                    EdgeCollision.points -= FrictionPrice;
                    Ball.UpdatePoints();
                    EdgeCollision.frictionLevel = EdgeCollision.FrictionState.Medium;
                    Ball.UpdateFriction();
                    FrictionPrice = 2500;

                    FrictionButtonText.text = $"Decrease Friction More - {FrictionPrice}";
                    break;
                case EdgeCollision.FrictionState.Medium:
                    EdgeCollision.points -= FrictionPrice;
                    Ball.UpdatePoints();
                    EdgeCollision.frictionLevel = EdgeCollision.FrictionState.High;
                    Ball.UpdateFriction();

                    FrictionButton.interactable = false;
                    break;
            }
        }
    }
}