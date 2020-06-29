using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    Button ButtonComponent;
    Text TextComponent;
    // Start is called before the first frame update
    void Start()
    {
        ButtonComponent = gameObject.transform.parent.gameObject.GetComponent<Button>();
        TextComponent = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ButtonComponent.interactable)
        {
            TextComponent.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        }
        else 
        {
            TextComponent.color = Color.white;
        }
    }
}
