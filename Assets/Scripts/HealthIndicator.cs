using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    private Material _material;
    private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        FindObjectOfType<PlayerHealth>().OnHealthChanged += HealthIndicator_OnHealthChanged;
    }

    private void HealthIndicator_OnHealthChanged(float pct)
    {
        _material.SetFloat(Cutoff, 1f - pct);
    }
}
