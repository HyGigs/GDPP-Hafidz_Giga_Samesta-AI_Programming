using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _powerUpDuration;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private int _health;
    [SerializeField] private TMP_Text _healthText;

    private Coroutine _powerUpCoroutine;
    private Rigidbody _rigidBody;
    private bool _isPowerUpActive;

    public Action OnPowerUpStart;
    public Action OnPowerUpStop;

    private void Awake()
	{
	    _rigidBody = GetComponent<Rigidbody>();
	}

    private void Start()
    {
        UpdateUI();
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

    private IEnumerator StartPowerUp()
    {
        _isPowerUpActive = true;

        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }

        yield return new WaitForSeconds(_powerUpDuration);
        _isPowerUpActive = false;

        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isPowerUpActive)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().Dead();
            }
        }
    }

    private void UpdateUI()
    {
        _healthText.text = "Health: " + _health;
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

    public void Dead()
    {
        _health -= 1;

        if (_health > 0)
        {
            transform.position = _respawnPoint.position;
        }
        else
        {
            _health = 0;
            Debug.Log("Lose");
            SceneManager.LoadScene("LoseScreen");
        }

        UpdateUI();
    }
}
