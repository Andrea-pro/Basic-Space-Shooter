using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public Enemy2 _enemy2;
   
    void Start()
    {
        

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy2");
        _enemy2 = enemy.GetComponent<Enemy2>();  

        if (_enemy2 == null)
        {
            Debug.LogError("_enemy2 is NULL.");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")  // If the player rams into the mine
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _enemy2.MineDead();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser") //This is the laser the player is shooting.
        {
            Destroy(other.gameObject);
            _enemy2.MineDead();
            Destroy(this.gameObject);
        }


      

    }
}


