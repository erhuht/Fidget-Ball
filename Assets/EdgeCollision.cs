using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Things to do:
- Check vibrations
- Refactor the update function if statements

- (Update --> Fixed physics time) tried once, couldn't get it woring :(
*/


public class EdgeCollision : MonoBehaviour {
    Vector3 translation;
    float aspect;
    float height, width;
    bool is_dragging;
    float radius;
    public static int points;
    public static int multiplier;
    public Text text;
    float mx, my;
    int id;
    public float friction;
    public static FrictionState frictionLevel;
    public float bounce;
    public MenuScript Menu;
    public enum FrictionState
    {
        Low,
        Medium,
        High,
        ExtraHigh
    }

    public void UpdateFriction() {
        switch (frictionLevel)
        {
            case FrictionState.Low:
                friction = 0.95f;
                break;
            case FrictionState.Medium:
                friction = 0.97f;
                break;
            case FrictionState.High:
                friction = 0.99f;
                break;
            case FrictionState.ExtraHigh:
                friction = 0.999f;
                break;
            default:
                Debug.Log("Invalid frictionLevel");
                break;
        }
    }

    public void UpdatePoints() {
        text.text = points.ToString();
    }

    void Awake() // PlayerPrefs are loaded in Awake, so the Start order doesn't matter
    {
        points = PlayerPrefs.GetInt("Points", 0);
        multiplier = PlayerPrefs.GetInt("Multiplier", 1);
        switch (PlayerPrefs.GetInt("FrictionLevel", 0))
        {
            case 0:
                frictionLevel = FrictionState.Low;
                break;
            case 1:
                frictionLevel = FrictionState.Medium;
                break;
            case 2:
                frictionLevel = FrictionState.High;
                break;
            case 3:
                frictionLevel = FrictionState.ExtraHigh;
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;

        translation = new Vector3(0.0f, 0.0f, 0.0f);

        aspect = (float)Screen.width / (float)Screen.height;
        height = 5.0f;
        width = height * aspect;
        
        is_dragging = false;
        radius = 0.5f;

        
        UpdatePoints();

    
        UpdateFriction();
        bounce = 1f;
	}

    // Check if touch has ended
    void TouchEnded()
    {
        try
        {
            if (Input.GetTouch(id).phase == TouchPhase.Ended)
            {
                is_dragging = false;
            }
        } catch (System.ArgumentException) // no touch id
        {
            // This might not be the right way to do this
        }
        #if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            is_dragging = false;
        }
        #endif
    }

    // Check if touch has moved
    void TouchMoved()
    {
        #if !UNITY_EDITOR
        if (is_dragging && Input.GetTouch(id).phase == TouchPhase.Moved) 
        {
            translation.x = (Input.GetTouch(id).position.x * (width / Screen.width) * 2 - width) - mx;
            translation.y = (Input.GetTouch(id).position.y * (height / Screen.height) * 2 - height) - my;

            mx = (Input.GetTouch(id).position.x * (width / Screen.width) * 2 - width);
            my = (Input.GetTouch(id).position.y * (height / Screen.height) * 2 - height);
        }
        #endif
        #if UNITY_EDITOR
        if (is_dragging)
        {
            translation.x = (Input.mousePosition.x * (width / Screen.width) * 2 - width) - mx;
            translation.y = (Input.mousePosition.y * (height / Screen.height) * 2 - height) - my;

            mx = (Input.mousePosition.x * (width / Screen.width) * 2 - width);
            my = ((Input.mousePosition.y * (height / Screen.height) * 2 - height));
        }
        #endif
    }

    // Check if a new touch has begun
    void TouchBegun()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                float dx = (Input.GetTouch(i).position.x * (width / Screen.width) * 2 - width) - gameObject.transform.position.x;
                float dy = (Input.GetTouch(i).position.y * (height / Screen.height) * 2 - height) - gameObject.transform.position.y;
                if (Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2)) < 0.6f)
                {
                    id = Input.GetTouch(i).fingerId;
                    Debug.Log(id);

                    is_dragging = true;
                    
                    mx = (Input.GetTouch(i).position.x * (width / Screen.width) * 2 - width);
                    my = (Input.GetTouch(i).position.y * (height / Screen.height) * 2 - height);

                    translation = Vector3.zero;

                }
            }
        }
        #if UNITY_EDITOR
        // The distances from the mouse to the ball
        float dx_m = (Input.mousePosition.x * (width / Screen.width) * 2 - width) - gameObject.transform.position.x;
        float dy_m = (Input.mousePosition.y * (height / Screen.height) * 2 - height) - gameObject.transform.position.y;
        if (Input.GetMouseButtonDown(0))
        {
            if (Mathf.Sqrt(Mathf.Pow(dx_m, 2) + Mathf.Pow(dy_m, 2)) < radius)// Is the mouse in the ball, with Pythagoras
            {
                is_dragging = true;
                mx = (Input.mousePosition.x * (width / Screen.width) * 2 - width);
                my = ((Input.mousePosition.y * (height / Screen.height) * 2 - height));
            }
        }
        #endif
    }

    // Run when collided
    void Collide()
    {
        Handheld.Vibrate();
        if (is_dragging == false)
        {
            points += multiplier;
            UpdatePoints();
            Menu.UpdateButtons();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For changing window size in the editor
        #if UNITY_EDITOR
        aspect = (float)Screen.width / (float)Screen.height;
        width = height * aspect;
        #endif


        // Check if touch has ended
        TouchEnded();

        if (!MenuScript.GameIsPaused)
        {
            // Check if touch has moved
            TouchMoved();

            // Check if a new touch has begun
            TouchBegun();
        }

        // Collide to walls and translate the ball
        float threshold = 0.001f;
        float correction = 0.01f;
        if (gameObject.transform.position.x > width-radius)
        {
            if (Mathf.Abs(translation.x) < threshold)
            {
                translation.x = -correction;
            }
            translation.x = -Mathf.Abs(translation.x)*bounce;
            Collide();
        }
        if (gameObject.transform.position.x < -width+radius)
        {
            if (Mathf.Abs(translation.x) < threshold)
            {
                translation.x = correction;
            }
            translation.x = Mathf.Abs(translation.x)*bounce;
            Collide();
        }
        if (gameObject.transform.position.y > height - radius)
        {
            if (Mathf.Abs(translation.y) < threshold)
            {
                translation.y = -correction;
            }
            translation.y = -Mathf.Abs(translation.y)*bounce;
            Collide();
        }
        if (gameObject.transform.position.y < -height + radius)
        {
            if (Mathf.Abs(translation.y) < threshold)
            {
                translation.y = correction;
            }
            translation.y = Mathf.Abs(translation.y)*bounce;
            Collide();
        }

        gameObject.transform.Translate(translation);
        translation *= friction;
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.SetInt("Multiplier", multiplier);
        PlayerPrefs.SetInt("FrictionLevel", (int) frictionLevel);
    }
}