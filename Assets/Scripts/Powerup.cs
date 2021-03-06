using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;
    [SerializeField]
    private int powerupId; // 0 Triple shot, 1 Speed, 2 Shield
    [SerializeField]
    private AudioClip _clip;
    
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
                       
           if (player != null)
           {
               switch (powerupId)
                {
                    case 0:
                    player.TripleShotActive();
                    break;

                    case 1:
                    player.SpeedBoostActive();
                    break;

                    case 2:
                    player.ShieldActive();
                    break;

                    default:
                    Debug.Log("Default Value");
                    break;
                }
            }

            Destroy(this.gameObject);
        }
    }    



}
