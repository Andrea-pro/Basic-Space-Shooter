using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //translater laser up
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 8f)
        {
            // checks if object has a parent
            if (transform.parent != null)
            {
                //then destroy the parent, child will be destroyed as well
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
