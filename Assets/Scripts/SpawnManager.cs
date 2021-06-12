using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Variables")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

   // New Enemy Type
    [SerializeField] private GameObject _enemy2Prefab;
    public bool _enemy2Exists = false;
    
    [Header("Powerup Variables")]
    [SerializeField] private GameObject[] powerups;
   
    [Header("General Spawn Manager")]
    private bool _stopSpawning = false;
    


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemy2Routine());
    }

    IEnumerator SpawnEnemy2Routine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(2.0f);
            if (_enemy2Exists == false)
            { Vector3 spawnPosition = new Vector3(-7, 2, 0);
                //GameObject newEnemy2 = 
                Instantiate(_enemy2Prefab, spawnPosition, Quaternion.identity);
                //newEnemy2.transform.parent = _enemyContainer.transform;
                _enemy2Exists = true;
                yield return new WaitForSeconds(2.0f);
            }
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5.0f); 
        while (_stopSpawning == false)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(10.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f); 
        while (_stopSpawning == false)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 7); // if int the last number never gets called  
            Instantiate(powerups[randomPowerUp], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void Enemy2Exists()
    {
        _enemy2Exists = true;
    }

    public void Enemy2Dead()
    {
        //Debug.LogError("Enemy 2 Dead - Bool Triggered."); 
        _enemy2Exists = false;
    }
    


}
