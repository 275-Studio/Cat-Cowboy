using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Data/WaveData")]
public class WaveData : ScriptableObject
{
    public List<BarrelData> barrels = new List<BarrelData>();
    public List<ChestData> chests; 
    public float spawnSpeed;
    public float barrelSpeed;
}
