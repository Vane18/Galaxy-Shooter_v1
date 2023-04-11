using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _ShieldPrefab;
    [SerializeField]
    private GameObject _ExplosionPrefab;
    public bool canTripleShot = false;
    public bool canSpeedUp = false;
    public bool IsShieldOn = false;
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;
    public int health = 3;
    [SerializeField]
    private float _speed = 5.0f;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject[] _engines;
    private int hitCount = 0;
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_gameManager.isCoopMode == false)
        {
            //current pos = new position
            transform.position = new Vector3(0, 0, 0);
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(health);
        }
        _audioSource = GetComponent<AudioSource>();
        hitCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isPlayerOne == true)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)))
            {
                Shoot();
            }
            Movement();
        }
        if(isPlayerTwo == true)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                Shoot();
            }
            PlayerTwoMovement();
        }
    }
    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (canTripleShot == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            }
            _canFire = Time.time + _fireRate;
        }
    }
    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // new Vector3(1,0,0) * 1 * 5 > new Vector3(5,0,0)
        if(canSpeedUp == true)
        {
            transform.Translate(Vector3.right * _speed * 2.0f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 2.0f * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.3f)
        {
            transform.position = new Vector3(transform.position.x, -4.3f, 0);
        }
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }
    private void PlayerTwoMovement()
    {
        if (canSpeedUp == true)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * 2.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * 2.0f *Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * 2.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * 2.0f * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.3f)
        {
            transform.position = new Vector3(transform.position.x, -4.3f, 0);
        }
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (IsShieldOn)
        {
            IsShieldOn = false;
            _ShieldPrefab.SetActive(false);
            return;
        }
        else
        {
            health--;
            _uiManager.UpdateLives(health);

            if (health < 1)
            {
                Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
                _gameManager.gameOver = true;
                _uiManager.ShowTitleScreen();
                Destroy(this.gameObject);
            }
        }
        hitCount++;
        int randomEngine = Random.Range(0, 2);
        if (hitCount == 1)
        {
            _engines[randomEngine].SetActive(true);
        }
        else if (hitCount == 2)
        {
            if (randomEngine == 1)
            {
                _engines[0].SetActive(true);
            }
            else if (randomEngine == 0)
            {
                _engines[1].SetActive(true);
            }
        }
    }
    public void TripleShotPowerUpOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }
    public void SpeedUpPowerUpOn()
    {
        canSpeedUp = true;
        StartCoroutine(SpeedUpDownRoutine());
    }
    public IEnumerator SpeedUpDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canSpeedUp = false;
    }

    public void ShieldPowerUpOn()
    {
        _ShieldPrefab.SetActive(true);
        IsShieldOn = true;
    }
}
