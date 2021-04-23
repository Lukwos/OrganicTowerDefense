using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : Node
{
    [Header("Turret")]
    public List<Enemy> _targets;
    public float _cooldown;

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Enemy enemy = GetMainTarget();
            if (enemy)
            {
                Shoot(enemy);
                yield return new WaitForSecondsRealtime(_cooldown);
            }
            else
            {
                yield return null;
            }
        }
    }

    void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    void Shoot(Enemy enemy)
    {
        enemy.TakeDamage(_stepColor, 1);
    }

    Enemy GetMainTarget()
    {
        _targets.RemoveAll(t => t == null);
        return _targets.Count > 0 ? _targets[0] : null;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            _targets.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            _targets.Remove(enemy);
        }
    }
}

[CustomEditor(typeof(Turret))]
public class TurretEditor : NodeEditor
{

}