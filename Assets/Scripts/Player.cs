using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 6.5f;
    private float _speedBooster = 2.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private SpawnManager _spawnManager;

    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldActive = false;

    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;

    [SerializeField] private int _score;

    private UIManager _uiManager;
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    // Thruster Speed with Shift key
    private float _speedShift = 12.0f; // var for speed when shift key is pressed
    private bool _speedShiftActive = false;
    private float _standardSpeed = 6.5f;

    //Thruster HUD Stamina Bar
    [SerializeField] public Slider _thrusterStaminaBar;
    private float _thrusterStaminaMax = 100f;
    private float _thrusterStaminaCurrent;
    private float _thrusterStaminaChangeRate;

    // Shield Strength
    private int _shieldCounter = 0;

    //Ammo Count
    [SerializeField] private int _ammoCount = 15;
    private int _maxAmmo = 15;

    //2nd Fire Firewall
    [SerializeField] private GameObject _firewallObject;
    private bool _firewallActive = false;
    private int _firewallCounter = 0;

    //CameraShake
    [SerializeField] public CameraShake cameraShake; //Camera in inspector goes here

    //CheatActive
    public bool _cheatActive = false;
   
    void Start()
    {
        transform.position = new Vector3(0, -3f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    
        //ThrusterStaminaBar Start
        _thrusterStaminaCurrent = 100f;
        _thrusterStaminaChangeRate = 20f;


        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    void Update()
    {
        CalculateMovement();
        SpeedShiftThurster();
        _thrusterStaminaBar.value = _thrusterStaminaCurrent;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount >= 1)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _cheatActive = true;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _cheatActive = false;
        }

    }

    public void SpeedShiftThurster()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_thrusterStaminaCurrent > 0)
            {
                _speedShiftActive = true;
                _speed = _speedShift;
                _thrusterStaminaCurrent -= _thrusterStaminaChangeRate * Time.deltaTime;
            }

            else if (_thrusterStaminaCurrent <= 0)
            {
                _speed = _standardSpeed;
            }

        }
        else 
        {
            _speed = _standardSpeed;

            if (_thrusterStaminaCurrent < _thrusterStaminaMax)
            {
                _thrusterStaminaCurrent += _thrusterStaminaChangeRate * Time.deltaTime;
            }
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //alternative solution transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);


        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }


        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        if (_firewallActive == false)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audioSource.Play();
            UpdateAmmoCount();
        }
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldCounter--;
            SetShieldColor();

            if (_shieldCounter < 1)
            {
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
            }
            return;
        }

        _lives -= 1;
        StartCoroutine(cameraShake.Shake(0.15f, 0.2f));
       
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
       
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive() //This is for the powerup
    {
        _speedBoostActive = true;
        _speed *= _speedBooster;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostActive = false;
        _speed /= _speedBooster;
    }
    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldCounter++;
        SetShieldColor();

        if (_shieldCounter > 3) //fixes getting more than 3 shields
        {
            _shieldCounter = 3;
        }

    }

    public void SetShieldColor()
    {
        switch (_shieldCounter)
        {
            case 3:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;

            case 2:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)0.5);
                break;

            case 1:
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)0.2);
                break;
            case 0:
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
                break;

        }


    }

    public void ScorePlus(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void UpdateAmmoCount()
    {
         _ammoCount -= 1;
         _uiManager.UpdateAmmo(_ammoCount, _maxAmmo);
    }

    public void AmmoFill()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount, _maxAmmo);
    }

    public void HealthUp()
    {
        if(_lives<3)
        {
          _lives++;
          _uiManager.UpdateLives(_lives);

            if(_lives == 3)
            {
              _rightEngine.SetActive(false);
              _leftEngine.SetActive(false);
            }
        }


    }
    public void FirewallUp()
    {
        _firewallCounter++; //a quick solution to have the firewall spawn slowly :)
        _uiManager.UpdateFirewallCounter(_firewallCounter);

        if (_firewallCounter == 3)
        {
            _firewallActive = true;
            _firewallObject.SetActive(true);
            StartCoroutine(FirewallPowerDownRoutine());
            _firewallCounter = 0;
            _uiManager.UpdateFirewallCounter(_firewallCounter);
        }
    }

    IEnumerator FirewallPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _firewallActive = false;
        _firewallObject.SetActive(false);
    }

    public void NegativeUp()
    {
        _thrusterStaminaCurrent = 20f; //Reduces the thruster stamina to 20
    }
}
