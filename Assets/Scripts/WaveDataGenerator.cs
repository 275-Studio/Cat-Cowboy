using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaveDataGenerator : MonoBehaviour
{
    public GameObject barrelPrefab1;
    public GameObject barrelPrefab2;
    public GameObject barrelPrefab3;
    public GameObject chestPrefab;

    [ContextMenu("Generate Waves")]
    public void GenerateWaves()
    {
        for (int i = 1; i <= 30; i++)
        {
            WaveData wave = ScriptableObject.CreateInstance<WaveData>();
            wave.name = $"Wave_{i:00}";
            wave.spawnSpeed = Mathf.Max(0.2f, 3f - (i * 0.15f));
            wave.barrelSpeed = 2f + (i * 0.25f);
            wave.barrels = new List<BarrelData>();
            wave.chests = new List<ChestData>();

            int baseJumlah = 50 + i * 2;

            if (i < 5)
            {
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab1, jumlah = baseJumlah });
            }
            else if (i < 10)
            {
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab1, jumlah = Mathf.FloorToInt(baseJumlah * 0.6f) });
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab2, jumlah = Mathf.FloorToInt(baseJumlah * 0.4f) });
            }
            else
            {
                int extra = (i - 10) * 3;
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab1, jumlah = Mathf.FloorToInt(baseJumlah * 0.4f) + extra });
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab2, jumlah = Mathf.FloorToInt(baseJumlah * 0.35f) + extra });
                wave.barrels.Add(new BarrelData { barrelPrefab = barrelPrefab3, jumlah = Mathf.FloorToInt(baseJumlah * 0.25f) + extra });
            }

            int chestCount = Random.Range(1, 3); 
            wave.chests.Add(new ChestData { chestPrefab = chestPrefab, jumlah = chestCount });

            AssetDatabase.CreateAsset(wave, $"Assets/Resources/Waves/Wave_{i:00}.asset");
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Waves generated.");
    }
}
