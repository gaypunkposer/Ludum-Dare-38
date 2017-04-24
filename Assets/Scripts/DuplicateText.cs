using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DuplicateText : MonoBehaviour {

    Text thisText;
    public Text DuplicateTarget;

	void Start () {
        thisText = GetComponent<Text>();	
	}
	
	void Update () {
        DuplicateTarget.text = thisText.text;
	}
}
