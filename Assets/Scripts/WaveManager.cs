using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public UnityEngine.UI.Text WaveText;

    public int TotalWaves = 15;
    public SpawnEnemies[] Spawners;

    List<GameObject> Enemies = new List<GameObject>();
    int currentWave;
    int nextWave;
    bool debug;
    bool triggerActivated;

    void Start () {
#if UNITY_EDITOR
        debug = true;
#endif
    }

    void Update () {
		if (debug)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                TriggerNextWave(true);

            }
        }

        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !triggerActivated)
        {
            if (nextWave == 0)
            {
                TriggerNextWave(false);
            }

            else
            {
                TriggerNextWave(true);
                
            }
            triggerActivated = true;
        }

    }

    void TriggerNextWave(bool sendMessage)
    {
        if (nextWave < TotalWaves)
        {
            foreach (SpawnEnemies s in Spawners)
            {
                s.TriggerWave(nextWave);
            }
            currentWave = nextWave;
            nextWave++;
            UpdateText();
            if (sendMessage) { GameObject.Find("Player Parent").SendMessage("NewWave"); }
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    void UpdateText ()
    {
        WaveText.text = "Wave " + (currentWave + 1).ToString();
        WaveText.GetComponent<Animation>().Play();
    }

    public void AddEnemy(GameObject go)
    {
        Enemies.Add(go);
    }

    public void RemoveEnemy(GameObject go)
    {
        triggerActivated = false;
        Enemies.Remove(go);
        if (Enemies.Count <= 0)
        {
            //TriggerNextWave();
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] Enemies;
}