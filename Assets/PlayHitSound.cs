using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHitSound : MonoBehaviour {

    AudioSource source;
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
    void Update () {
		
	}

    void PlaySound()
    {
        source.Play();
    }
}
