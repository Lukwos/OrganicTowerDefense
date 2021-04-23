using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    public StepColor _stepColor;
    public int _health;
    public Transform _target;
    public int _rewardMoney;
    public float _detonationCooldown;
    public int _firePower;
    int _currentHealth;
    NavMeshAgent _agent;
    GameSystem _gameSystem;

    [Header("UI")]
    public Gradient _barColor;
    public Slider _bar;
    public Image _barFill;

    void Start()
    {
        _gameSystem = FindObjectOfType<GameSystem>();
        _currentHealth = _health;
        _agent = GetComponent<NavMeshAgent>();
        UpdateVisual();
        UpdateUI();
    }

    private void Update()
    {
        if (_target)
        {
            _agent.SetDestination(_target.position);
            _agent.CalculatePath(_target.position, _agent.path);
            if (Vector3.Distance(_target.position, transform.position) < _agent.stoppingDistance)
            {
                StartCoroutine(DetonateCoroutine());
            }
        }
    }

    IEnumerator DetonateCoroutine()
    {
        yield return new WaitForSecondsRealtime(_detonationCooldown);
        _gameSystem.TakeDamage(_firePower);
        Destroy(gameObject);
    }

    public void TakeDamage(StepColor stepColor, int damage)
    {
        if (stepColor == _stepColor)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _gameSystem.AddMoney(_rewardMoney);
                Destroy(gameObject);
            }
            else
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        _bar.value = (float)_currentHealth / _health;
        _barFill.color = _barColor.Evaluate(_bar.value);
    }

    void UpdateVisual()
    {
        GetComponent<Renderer>().material.color = Utils.GetColorFromStepColor(_stepColor);
    }
}
