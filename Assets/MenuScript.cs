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
    int MultiplierPrice;
    int FrictionPrice;

    void Awake() // PlayerPrefs are loaded in Awake, so the Start order doesn't matter
    {
        MultiplierPrice = PlayerPrefs.GetInt("MultiplierPrice", 200);
        FrictionPrice = PlayerPrefs.GetInt("FrictionPrice", 500);
    }

    void Start()
    {
        Resume();

        UpdateButtons();
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

    void Resume() // Resumes the game
    {
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    void Pause() // Pauses the game
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

            UpdateButtons();
        }
    }

    public void BuyDecreaseFriction() // Called when friction decrease is bought
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

                    UpdateButtons();
                    break;
                case EdgeCollision.FrictionState.Medium:
                    EdgeCollision.points -= FrictionPrice;
                    Ball.UpdatePoints();
                    EdgeCollision.frictionLevel = EdgeCollision.FrictionState.High;
                    Ball.UpdateFriction();
                    
                    UpdateButtons();
                    FrictionButton.interactable = false; // Makes the button greyed out
                    break;
            }
        }
    }

    public void UpdateButtons()
    {
        UpdateMultiplier();
        UpdateDecreaseFriction();
    }
    void UpdateMultiplier() // Updates button in shop
    {
        if (EdgeCollision.points >= MultiplierPrice)
        {
            MultiplierButtonText.text = $"{EdgeCollision.multiplier*2}x Multiplier - <color=lime>{MultiplierPrice}</color>";
        }
        else
        {
            MultiplierButtonText.text = $"{EdgeCollision.multiplier*2}x Multiplier - <color=#ff6666>{MultiplierPrice}</color>";
        }
    }

    void UpdateDecreaseFriction() // Updates button in shop
    {
        if (EdgeCollision.points >= FrictionPrice)
        {
            switch (EdgeCollision.frictionLevel)
            {
                case EdgeCollision.FrictionState.Low:
                    FrictionButtonText.text = $"Decrease Friction - <color=lime>{FrictionPrice}</color>";
                    break;
                case EdgeCollision.FrictionState.Medium:
                    FrictionButtonText.text = $"Decrease Friction More - <color=lime>{FrictionPrice}</color>";
                    break;
                case EdgeCollision.FrictionState.High:           
                    FrictionButtonText.text = "Already Minimum Friction";
                    FrictionButton.interactable = false;
                    break;
            }
        }
        else
        {
            switch (EdgeCollision.frictionLevel)
            {
                case EdgeCollision.FrictionState.Low:
                    FrictionButtonText.text = $"Decrease Friction - <color=#ff6666>{FrictionPrice}</color>";
                    break;
                case EdgeCollision.FrictionState.Medium:
                    FrictionButtonText.text = $"Decrease Friction More - <color=#ff6666>{FrictionPrice}</color>";
                    break;
                case EdgeCollision.FrictionState.High:           
                    FrictionButtonText.text = "Already Minimum Friction";
                    FrictionButton.interactable = false;
                    break;
            }
        }
    }


    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("MultiplierPrice", MultiplierPrice);
        PlayerPrefs.SetInt("FrictionPrice", FrictionPrice);
    }
}