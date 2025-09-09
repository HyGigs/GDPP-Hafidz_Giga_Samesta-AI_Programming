using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private PickableType _pickableType;

    public Action<Pickable> OnPicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPicked?.Invoke(this);
        }
    }
}
