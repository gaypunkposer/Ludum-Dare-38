using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffects : MonoBehaviour {

    public int NumberOfBlinks;
    public MeshRenderer Guns;
    public MeshRenderer Body;
    public float ShakeStrength;
    [Range(0, 1)]
    public float Alpha;
    public Transform PlayerCamera;
    public Text NegativeText;

    float invulnerablilityTime;

    private void Start()
    {
        invulnerablilityTime = GetComponent<PlayerState>().InvulnerablilityTime;
    }
    //Camera shake, transparency in the character, show docking of multiplier

    public void CameraShake(Transform enemy)
    {
        //Vector3 difference = (transform.position - enemy.position) * ShakeStrength;
        //PlayerCamera.position += difference;
        //transform.position += difference;
    }

    public IEnumerator TransparentBlinking ()
    {
        Color ogColor = Guns.material.GetColor("_BGColor");
        Color newColor = ogColor; newColor.a = Alpha;
        Color a = ogColor, b = newColor;

        float blinkMultiplier = NumberOfBlinks * 2 / invulnerablilityTime;
        float timer = 0;
        
        while (timer < invulnerablilityTime)
        {
            Color currColor = Guns.material.GetColor("_BGColor");
            if (currColor == ogColor)
            {
                a = ogColor; b = newColor;
            }
            else if (currColor == newColor)
            {
                a = newColor; b = ogColor;
            }

            float t = (timer % 1) * blinkMultiplier;
            t = (t > 1) ? t - 1 : t;
            Color transColor = Color.Lerp(a, b, t);

            Guns.material.SetColor("_BGColor", transColor);
            Body.material.SetColor("_BGColor", transColor);

            timer += Time.deltaTime;
            yield return null;
        }
        Guns.material.SetColor("_BGColor", ogColor);
        Body.material.SetColor("_BGColor", ogColor);
    }

    public void DockMultiplier(int amount, bool percentage)
    {
        NegativeText.gameObject.GetComponentInChildren<Text>().color = Color.red;

        NegativeText.text = (percentage) ? "-" + amount.ToString() + "%" : "-" + amount.ToString();

        NegativeText.GetComponent<Animation>().Play();
    }

    public void AddMultiplier(int amount, bool percentage)
    {
        NegativeText.gameObject.GetComponentInChildren<Text>().color = Color.green;

        NegativeText.text = (percentage) ? "+" + amount.ToString() + "%" : "+" + amount.ToString();

        NegativeText.GetComponent<Animation>().Play();
    }
}
