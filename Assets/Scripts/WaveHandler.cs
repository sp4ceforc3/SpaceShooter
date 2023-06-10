using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveHandler : MonoBehaviour
{
    // enemy prefabs
    [SerializeField] SpriteRenderer [] enemies;
    [SerializeField] GameObject [] bosses;
    
    [SerializeField] GameObject bossSpawnArea;
    [SerializeField] GameObject enemySpawnArea;

    // UI
    [SerializeField] TextMeshProUGUI waveDisplay;
    
    public int currentwave = 0;
    public int enemiesLeft = 0;
    
    int bossIndex = 0;
    
    int numberOfEnemies = 0;
    const int MAXNUMBEROFENEMIES = 8;

    bool StartofWave = false;

        private Vector3 GetSpawnPos(GameObject spawnArea)
    {
        Transform sat = spawnArea.transform;
        float rangeXBounds = (sat.lossyScale.x - 1) / 2;
        float rangeYBounds = (sat.lossyScale.y - 1) / 2;

        float x = Random.Range(0, rangeXBounds);
        float y = Random.Range(0, rangeYBounds);
        Vector3 spawnPos = new Vector3(x, y, 0); 
        return spawnPos;        
    }

    private int calculateNumberOfEnemies()
    {
        int newNumber = (int) Mathf.Ceil(currentwave / 2);

        if(newNumber <= numberOfEnemies)
        {
            // spawn at least one more enemy per wave 
            newNumber = numberOfEnemies + 1; 
        }
        
        return Mathf.Min(newNumber, MAXNUMBEROFENEMIES);
    }

    public IEnumerator spawnBoss()
    {
        StartofWave = true;
        yield return new WaitForSeconds(2f);
        Vector3 spawnPos = GetSpawnPos(bossSpawnArea);
        GameObject enemy = Instantiate(bosses[bossIndex], spawnPos, bossSpawnArea.transform.rotation);

        if(bossIndex == 1)
        {
            BossLinusHandling script = (enemy.GetComponent<BossLinusHandling>());
            script.waveHandlerScript = this;
        }
        else
        {
            BossDomai script = (enemy.GetComponent<BossDomai>());
            script.waveHandlerScript = this;
        }
            
        // iterate over bosses
        bossIndex = (bossIndex + 1) % bosses.Length;
        enemiesLeft += 1;

        StartofWave = false;
    }

    public IEnumerator spawnNextWave(int numberOfEnemies)
    {
        StartofWave = true;
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPos = GetSpawnPos(enemySpawnArea);
            SpriteRenderer enemy = Instantiate(enemies[i % enemies.Length], spawnPos, enemySpawnArea.transform.rotation);
            EnemyHandling script = enemy.GetComponent<EnemyHandling>();
            script.waveHandlerScript = this;
            enemiesLeft += 1;
        }   
        StartofWave = false;     
    }

    public void StartWave()
    {
        currentwave +=1;
        numberOfEnemies = calculateNumberOfEnemies();

        if (((currentwave % 5) == 0) && currentwave != 0)
            StartCoroutine(spawnBoss());
        else
            StartCoroutine(spawnNextWave(numberOfEnemies));
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if(enemiesLeft <= 0 && StartofWave != true)
            StartWave();
        
    }
}
