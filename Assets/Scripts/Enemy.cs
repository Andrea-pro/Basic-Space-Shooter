using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 5.0f; //reduced speed to test it slower
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab; //enemy fire
    private float _fireRateEnemy = 5.0f; //enemy fire
    private float _canFire = -1; //enemy fire

    void Start()
    {
        //Adding a start position to avoid spawning on the player
        transform.position = new Vector3(0, -9f, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The player is NULL.");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null.");
        }
    }
  
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRateEnemy = Random.Range(5f, 10f);
            _canFire = Time.time + _fireRateEnemy;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0,-0.6f,0), Quaternion.identity); 
            LaserScript[] lasers = enemyLaser.GetComponentsInChildren<LaserScript>();

         for (int i = 0; i < lasers.Length; i++) //review
            {
                lasers[i].AssignEnemyLaser();
            } 
        } 
    }
     
    void CalculateMovement()
    {

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-9f, 9);
            transform.position = new Vector3(randomX, 7, 0);
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
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 1.0f);
            
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.ScorePlus(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
           
        }

    }
}
