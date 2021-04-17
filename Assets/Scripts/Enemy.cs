using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
   
    void Start()
    {
        //Adding a start position to avoid spawning on the player
        transform.position = new Vector3(0, -9f, 0);
    }

  
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-9f, 9);
            transform.position = new Vector3(randomX, 7, 0);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Create a variable for the player getting the component of player
            Player player = other.transform.GetComponent<Player>();

            // checks if the player exists and thereby avoids errors
            if (player != null)
            {
                //then apply the damage method from the player script
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}
