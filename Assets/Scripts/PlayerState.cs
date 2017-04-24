using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState : MonoBehaviour {

    public int ScoreMultiplier;
    public int Score;
    public float WaveTime;
    public float RunTime;
    public float PickupSuccForce;
    public UnityEngine.UI.Text UIText;
    public Queue<Powerup> PowerupQueue = new Queue<Powerup>();
    public float InvulnerablilityTime;
    public TrailRenderer TRenderer;
    public UnityEngine.UI.Text DedText;
    public AudioClip GameOver;

    int highestScoreMultiplier;
    bool skipSound;
    bool ded;
    bool tookDamage;
    bool invulnerable;
    DamageEffects effects;
    AudioSource source;
    void Start () {
        effects = GetComponent<DamageEffects>();
        source = GetComponent<AudioSource>();
	}
	
    void NewWave()
    {

        WaveTime = 0;
        if (tookDamage)
        {
            ScoreMultiplier++;
            effects.AddMultiplier(1, false);
        }
        else
        {
            ScoreMultiplier *= 2;
            effects.AddMultiplier(100, true);
        }
        tookDamage = false;
    }

	void Update () {

        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }


        if (ded)
        {
            //I have no shame copy-pasting from stack overflow
            TimeSpan t = TimeSpan.FromSeconds(RunTime);
            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);

            DedText.text = "YOU DIE. TRY AGAIN" + System.Environment.NewLine +
                "POINT: " + Score + System.Environment.NewLine +
                "TIME: " + answer + System.Environment.NewLine +
                "MULTIPLY: " + highestScoreMultiplier + System.Environment.NewLine;

            if (Input.anyKeyDown)
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

        }
        else
        {
            WaveTime += Time.deltaTime;
            RunTime += Time.deltaTime;

            highestScoreMultiplier = (ScoreMultiplier > highestScoreMultiplier) ? ScoreMultiplier : highestScoreMultiplier;

            string targetText = Score.ToString() + System.Environment.NewLine + ScoreMultiplier.ToString() + "x";
            UIText.text = targetText;
            TRenderer.time = ScoreMultiplier / 25f;
        }
	}

    void AddScore(int score)
    {
        Score += score * ScoreMultiplier;
    }

    void AddScoreMultiplier()
    {
        ScoreMultiplier++;
    }

    void TakeDamage(int ammount, bool percentage)
    {
        tookDamage = true;
        source.Play();
        if (percentage)
        {
            Debug.Log((100 - ammount) / 100);
            ScoreMultiplier = (int) (ScoreMultiplier * ((100 - (float)ammount) / 100));
        }
        else
        {
            ScoreMultiplier -= ammount;
        }

        effects.DockMultiplier(ammount, percentage);

        if (ScoreMultiplier <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Time.timeScale = 0;
        DedText.gameObject.SetActive(true);
        UIText.gameObject.SetActive(false);
        source.clip = GameOver;
        source.Play();
        ded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //Debug.Log(collision.gameObject);
            ScoreValue value = collision.gameObject.GetComponent<ScoreValue>();
            AddScore(value.Value);
            ScoreMultiplier += value.MultiplierValue;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 12 || other.gameObject.layer == 14) && !invulnerable)
        {
            DamageStats stats = other.GetComponent<DamageStats>();
            TakeDamage(stats.DamageAmount, stats.IsMultiplierPercentageDown);
            invulnerable = true;
            StopCoroutine("InvulnerabilityCountdown");
            StartCoroutine("InvulnerabilityCountdown");
            effects.CameraShake(other.transform);
            StartCoroutine(effects.TransparentBlinking());
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }

    IEnumerator InvulnerabilityCountdown()
    {
        yield return new WaitForSeconds(InvulnerablilityTime);
        invulnerable = false;
    }

}

public enum Powerup
{
    Bomb,
    Freeze,
    Laser,
    Shield
}

