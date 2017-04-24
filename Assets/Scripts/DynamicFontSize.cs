using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicFontSize : MonoBehaviour {

    public float LargeTime;
    public Text BrotherText;

    Text myText;
    string previousText;
	void Start () {
        myText = GetComponent<Text>();
	}
	
	void Update () {
		if (myText.text != previousText)
        {
            StopCoroutine(ResetFontSize());
            myText.fontSize = 2;
            BrotherText.fontSize = 2;
            previousText = myText.text;
            StartCoroutine(ResetFontSize());
        }

	}

    IEnumerator ResetFontSize()
    {
        yield return new WaitForSeconds(LargeTime);
        myText.fontSize = 1;
        BrotherText.fontSize = 1;
    }
}
