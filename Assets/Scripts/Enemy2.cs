using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{  
    //enemy var
    [SerializeField] private float _enemy2Speed = 5.0f; 
   
    //references
    private Player _player;
    private Enemy _enemy1;
    private Animator _anim;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
   
    //Enemy2 Mine
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] public int _mineCount = 0;
    private Vector3 _enemy2Pos;
 
  

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = GetComponent<Animator>();

        #region Null Checks are collapsed here
        if (_player == null)
        { 
            Debug.LogError("The player is NULL."); 
        }
       
        if (_anim == null)
        { 
            Debug.LogError("Animator is Null."); 
        }

        if (_spawnManager == null)
        {  
            Debug.LogError("The Spawn Manager is NULL:"); 
        }
        #endregion

        StartCoroutine(DropMine());           
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {

        transform.Translate(Vector3.right * _enemy2Speed * Time.deltaTime);

        if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10, 2, 0);
        }

    }

     IEnumerator DropMine()  
    {   
        yield return new WaitForSeconds(3.0f);
        while (_mineCount <= 3)
      
        {
            yield return new WaitForSeconds(1.0f);
            float randomY = Random.Range(-2f, -4f); 
            Instantiate(_minePrefab, (transform.position + new Vector3(0, randomY, 0)), Quaternion.identity);
            _mineCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _enemy2Speed = 0;
            _audioSource.Play();
            _spawnManager.Enemy2Dead();
            Destroy(this.gameObject, 1.0f);
        }

        if (other.tag == "Laser" ) // Laser is player laser
        {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.ScorePlus(10);
                }
                _anim.SetTrigger("OnEnemyDeath");
                _audioSource.Play();

                _spawnManager.Enemy2Dead();
                Destroy(this.gameObject);
         }


        if (other.tag == "Firewall")
        {
            _anim.SetTrigger("OnEnemyDeath");
            _enemy2Speed = 0;
            _audioSource.Play();
            _spawnManager.Enemy2Dead();
            Destroy(this.gameObject);
        }

    }

    public void MineDead()
    {
        _mineCount--;
    }

}
