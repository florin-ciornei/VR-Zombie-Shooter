using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public event Action<float> OnHealthChanged = delegate { };

    [SerializeField]
    private int maxHealth = 100;

    private int _currentHealth;


    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;
        var pct = (float) _currentHealth / (float) maxHealth;
        OnHealthChanged(pct);
        
        if (_currentHealth <= 0)
        {
            SceneManager.LoadScene("GameTemp");
        }
    }
}
