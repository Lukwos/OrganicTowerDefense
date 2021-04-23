using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhost : MonoBehaviour
{
    public GameObject _towerPrefab;
    public float _spacing;
    int _price;
    Material _material;
    bool _valid;
    GameSystem _gameSystem;


    void Start()
    {
        _gameSystem = FindObjectOfType<GameSystem>();
        _price = _towerPrefab.GetComponent<Node>()._buyPrice;
        _material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        bool validDistance = GetMinDistanceFromTowers() > _spacing;
        bool validCost = _gameSystem._money > _price;
        _valid = validDistance & validCost;
        _material.color = _valid ? Color.green : Color.red;
    }

    public void Spawn()
    {
        if (_valid)
        {
            if (_gameSystem.TakeMoney(_price))
            {
                Instantiate(_towerPrefab, transform.position, transform.rotation);
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    float GetMinDistanceFromTowers()
    {
        float distance = Mathf.Infinity;
        foreach (Node node in FindObjectsOfType<Node>())
        {
            float d = Vector3.Distance(transform.position, node.transform.position);
            distance = Mathf.Min(d, distance);
        }
        return distance;
    }
}
