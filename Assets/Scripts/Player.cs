using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _powerUpDuration;


    private Coroutine _powerUpCoroutine;
    private Rigidbody _rigidBody;

    public Action OnPowerUpStart;
    public Action OnPowerUpStop;

    private void Awake()
	{
	    _rigidBody = GetComponent<Rigidbody>();
	}

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 horizontalDirection = horizontal * _camera.right;
        Vector3 verticalDirection = vertical * _camera.forward;
        verticalDirection.y = 0;
        horizontalDirection.y = 0;
        Vector3 movementDirection = horizontalDirection + verticalDirection;
        _rigidBody.velocity = movementDirection * _speed * Time.fixedDeltaTime;
    }

    public void PickPowerUp()
    {
        if (_powerUpCoroutine != null)
        {
            StopCoroutine(_powerUpCoroutine);
        }
        _powerUpCoroutine = StartCoroutine(StartPowerUp());

        Debug.Log("Pick Power Up");
    }

    private IEnumerator StartPowerUp()
    {
        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }

        yield return new WaitForSeconds(_powerUpDuration);

        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
    }

}
