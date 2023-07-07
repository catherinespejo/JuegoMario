using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    public Text text;
    private string textToWrite;
    private int characterIndex;
    private float timePerChar;
    private float timer;


    public void AddWriter(Text text, string textToWrite)
    {
        this.text = text;
        this.textToWrite = textToWrite;
        characterIndex = 0;
        timePerChar = 0.4f;
    }

  
    // Update is called once per frame
    void Update()
    {
      
    }

    public Text getText()
    {
        return text;
    }
}
