using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfig;
    [SerializeField]int startingWave = 0;
    [SerializeField] bool looping = false;

     IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
    }
    private IEnumerator SpawnAllWaves()
    {
        for(int waveIndex=startingWave; waveIndex < waveConfig.Count; waveIndex++)
        {
            var currentWave = waveConfig[waveIndex];
            yield return StartCoroutine(SpawnAlEnemiesInWave(currentWave));
        }
    }
    private IEnumerator SpawnAlEnemiesInWave(WaveConfig waveConfig)
    {
        for(int enemyCount = 0; enemyCount< waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
          var newEnemy=Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPath>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        

    }



}
