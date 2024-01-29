using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprite : MonoBehaviour
{
    /// <summary>
    /// Velocidad de movimiento de la bola
    /// </summary>
    public float speed;

    /// <summary>
    /// Referencia al player
    /// </summary>
    public GameObject spawner;

    void Update()
    {
        //Movimiento de la bola
        transform.position += (Vector3.right * speed * Time.deltaTime);
    }

    /// <summary>
    /// Destruccion de la bola
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        spawner.GetComponent<Player>().ReturnToIdle();
        spawner.GetComponent<Player>().WaitForEnd();
        gameObject.SetActive(false);
        
    }
}
