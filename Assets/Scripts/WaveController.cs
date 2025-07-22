using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveData> waves;
    private int currentWaveIndex = 0;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public Transform playerPoint;

    [Header("UI")]
    public GameObject waveClearPanel;
    public Button continueButton;
    private bool waitingForContinue = false;

    private List<GameObject> spawnedBarrels = new List<GameObject>();

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        waveClearPanel.SetActive(false);
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Semua wave selesai!");
            yield break;
        }

        WaveData currentWave = waves[currentWaveIndex];

        foreach (BarrelData barrel in currentWave.barrels)
        {
            for (int i = 0; i < barrel.jumlah; i++)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform spawn = spawnPoints[randomIndex];

                GameObject barrelInstance = Instantiate(barrel.barrelPrefab, spawn.position, Quaternion.identity);
                spawnedBarrels.Add(barrelInstance);

                Vector2 direction = (playerPoint.position - spawn.position).normalized;
                Rigidbody2D rb = barrelInstance.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * currentWave.barrelSpeed;
                }

                yield return new WaitForSeconds(currentWave.spawnSpeed);
            }
        }

        yield return StartCoroutine(WaitForBarrelsDestroyed());
        ItemStat.instance.stats.homeStats.currentHealthHome = ItemStat.instance.stats.homeStats.maxHealthHome;
        ItemStat.instance.stats.playerStats.coin += 100;
        
        waveClearPanel.SetActive(true);
        waitingForContinue = true;
        yield return null;
        Time.timeScale = 0f;
        yield return new WaitUntil(() => !waitingForContinue);
        Time.timeScale = 1f;
        waveClearPanel.SetActive(false);
        currentWaveIndex++;
        StartCoroutine(SpawnWave());
    }

    IEnumerator WaitForBarrelsDestroyed()
    {
        while (spawnedBarrels.Exists(barrel => barrel != null))
        {
            yield return null;
        }

        spawnedBarrels.Clear();
    }

    void OnContinueButtonClicked()
    {
        waitingForContinue = false;
    }
}
