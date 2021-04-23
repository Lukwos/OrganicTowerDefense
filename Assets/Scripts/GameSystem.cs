using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public int _money;
    public int _health;

    [Header("UI")]
    public Text _moneyText;
    public Text _healthText;

    private void OnValidate()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        _moneyText.text = $"Money : {_money}";
        _healthText.text = $"Health : {_health}";
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Debug.Log("GAME OVER");
        }
        UpdateUI();
    }

    public void AddMoney(int rewardMoney)
    {
        _money += rewardMoney;
        UpdateUI();
    }

    public bool TakeMoney(int value)
    {
        if(_money - value >= 0)
        {
            _money -= value;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }
}
