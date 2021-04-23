using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemyPrefab;
    public Transform _destination;
    public List<Transform> _spawnPoints;
    public float _cooldown;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Transform spawn = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            GameObject enemy = Instantiate(_enemyPrefab, spawn.position, spawn.rotation);
            enemy.GetComponent<Enemy>()._target = _destination;
            enemy.GetComponent<Enemy>()._stepColor = Utils.RandomStepColor(StepColor.Red, StepColor.Green, StepColor.Blue);
            enemy.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            yield return new WaitForSecondsRealtime(_cooldown);
        }
    }
}
