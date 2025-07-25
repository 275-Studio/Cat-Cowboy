using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    private List<WaveData> waves = new List<WaveData>();
    private int currentWaveIndex = 0;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public Transform playerPoint;
    private List<GameObject> spawnedBarrels = new List<GameObject>();
    private List<GameObject> spawnedChests = new List<GameObject>();

    [Header("Skill Settings")]
    private bool isTimeFreezeActive = false;
    private float timeFreezeDuration = 5f;
    private float originalBarrelSpeed;
    private float currentBarrelSpeed;

    [Header("Dependencies")]
    public UIManager uiManager;
    public PlayerController playerController;

    private void Start()
    {
        LoadWaves();
        StartCoroutine(StarterThenSpawnWave());
    }

    private void Update()
    {
        CheckGameOver();

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(HandleTimeFreeze());
        }
    }
    private IEnumerator StarterThenSpawnWave()
    {
        yield return StartCoroutine(uiManager.ShowStarterPanel());
        StartCoroutine(SpawnWave());
    }
    void LoadWaves()
    {
        currentWaveIndex = 0;
        WaveData[] loadedWaves = Resources.LoadAll<WaveData>("Waves");
        waves = new List<WaveData>(loadedWaves);
        waves.Sort((a, b) => a.name.CompareTo(b.name));
    }

    IEnumerator SpawnWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Semua wave selesai!");
            yield break;
        }

        WaveData currentWave = waves[currentWaveIndex];
        currentBarrelSpeed = currentWave.barrelSpeed;

        int pattern = Random.Range(0, 3);

        if (pattern == 0) yield return StartCoroutine(SpawnChests(currentWave));

        int halfway = Mathf.CeilToInt(currentWave.barrels.Count / 2f);

        for (int i = 0; i < currentWave.barrels.Count; i++)
        {
            yield return StartCoroutine(SpawnBarrelBatch(currentWave.barrels[i], currentWave.spawnSpeed));

            if (pattern == 1 && i == halfway - 1)
            {
                yield return StartCoroutine(SpawnChests(currentWave));
            }
        }

        if (pattern == 2) yield return StartCoroutine(SpawnChests(currentWave));

        yield return StartCoroutine(WaitForBarrelsDestroyed());

        EndWave();
        yield return StartCoroutine(uiManager.WaitForContinue());
        playerController.ResetAmmoToMax();
        currentWaveIndex++;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnBarrelBatch(BarrelData barrel, float spawnDelay)
    {
        int spawnBatchSize = 2;

        for (int i = 0; i < barrel.jumlah; i += spawnBatchSize)
        {
            int numSpawns = Random.Range(3, 6);
            Transform[] selectedPoints = GetRandomSpawnPoints(numSpawns);

            for (int j = 0; j < spawnBatchSize && i + j < barrel.jumlah; j++)
            {
                Transform spawn = selectedPoints[j % selectedPoints.Length];
                GameObject barrelInstance = Instantiate(barrel.barrelPrefab, spawn.position, Quaternion.identity);
                spawnedBarrels.Add(barrelInstance);

                Vector2 direction = (playerPoint.position - spawn.position).normalized;
                Rigidbody2D rb = barrelInstance.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    float speedToUse = isTimeFreezeActive ? currentBarrelSpeed * 0.05f : currentBarrelSpeed;
                    rb.linearVelocity = direction * speedToUse;
                }

                if (isTimeFreezeActive)
                {
                    SpriteRenderer sr = barrelInstance.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.color = Color.yellow;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnChests(WaveData wave)
    {
        foreach (ChestData chest in wave.chests)
        {
            for (int i = 0; i < chest.jumlah; i++)
            {
                Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject chestInstance = Instantiate(chest.chestPrefab, spawn.position, Quaternion.identity);
                spawnedChests.Add(chestInstance);

                Vector2 direction = (playerPoint.position - spawn.position).normalized;
                Rigidbody2D rb = chestInstance.GetComponent<Rigidbody2D>();

                if (rb != null)
                    rb.linearVelocity = direction * wave.barrelSpeed * 0.5f;

                yield return new WaitForSeconds(wave.spawnSpeed);
            }
        }
    }

    IEnumerator WaitForBarrelsDestroyed()
    {
        while (spawnedBarrels.Exists(barrel => barrel != null) && spawnedChests.Exists(chest => chest != null))
        {
            yield return null;
        }

        spawnedChests.Clear();
        spawnedBarrels.Clear();
    }

    Transform[] GetRandomSpawnPoints(int count)
    {
        List<Transform> shuffled = new List<Transform>(spawnPoints);

        for (int i = 0; i < shuffled.Count; i++)
        {
            Transform temp = shuffled[i];
            int rand = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[rand];
            shuffled[rand] = temp;
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count)).ToArray();
    }

    void EndWave()
    {
        ItemStat.instance.stats.homeStats.currentHealthHome = ItemStat.instance.stats.homeStats.maxHealthHome;
        ItemStat.instance.stats.playerStats.coin += 100;
        uiManager.ShowWaveClear();
    }

    public void ResetWaves()
    {
        StopAllCoroutines();
        currentWaveIndex = 0;
        spawnedBarrels.Clear();
        spawnedChests.Clear();
        uiManager.HideWaveClear();
        StartCoroutine(SpawnWave());
    }

    public void CheckGameOver()
    {
        if (ItemStat.instance.stats.homeStats.currentHealthHome <= 0)
        {
            uiManager.inGameUI.SetActive(false);
            uiManager.ShowGameOverUI();
        }
    }

    public void ActivateTimeFreeze()
    {
        Debug.Log("Test");
        StartCoroutine(HandleTimeFreeze());
    }

    private IEnumerator HandleTimeFreeze()
    {
        isTimeFreezeActive = true;
        Debug.Log("Time Freeze ACTIVE");

        float slowedSpeed = currentBarrelSpeed * 0.05f;
        int affected = 0;

        foreach (var barrel in spawnedBarrels)
        {
            if (barrel != null)
            {
                Rigidbody2D rb = barrel.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = rb.linearVelocity.normalized;
                    rb.linearVelocity = direction * slowedSpeed;

                    SpriteRenderer sr = barrel.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.color = Color.yellow;

                    affected++;
                }
            }
        }

        Debug.Log("Barrels slowed: " + affected);
        yield return new WaitForSeconds(timeFreezeDuration);

        affected = 0;

        foreach (var barrel in spawnedBarrels)
        {
            if (barrel != null)
            {
                Rigidbody2D rb = barrel.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = rb.linearVelocity.normalized;
                    rb.linearVelocity = direction * currentBarrelSpeed;

                    SpriteRenderer sr = barrel.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.color = Color.white;

                    affected++;
                }
            }
        }

        isTimeFreezeActive = false;
        Debug.Log("Barrels restored: " + affected);
        Debug.Log("Time Freeze ENDED");
    }

    public void ActivateBombSkill()
    {
        int destroyedCount = 0;

        foreach (GameObject barrel in spawnedBarrels)
        {
            if (barrel != null)
            {
                Destroy(barrel);
                destroyedCount++;
            }
        }
        ItemStat.instance.stats.playerStats.destroyedBarrel += destroyedCount;
        spawnedBarrels.Clear();
    }
}
