using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    /// <summary>
    /// Prefab de la bola que va a spawnear
    /// </summary>
    public GameObject ball;

    /// <summary>
    /// Bola ya spawneada
    /// </summary>
    public GameObject SpawnedBall;

    /// <summary>
    /// Punto de spawn
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// Animator del jugador
    /// </summary>
    Animator playerAnimator;

    /// <summary>
    /// Método para cambiar el estado del jugador
    /// </summary>
    /// <param name="state"></param>
    void AnimatePlayer(int state)
    {
        playerAnimator.SetInteger("State", state);
    }

    /// <summary>
    /// Spawn del combo
    /// </summary>
    public void SuperComboAttack()
    {
        if (!SpawnedBall)
        {
            SpawnedBall = Instantiate(ball, spawnPoint.position, Quaternion.identity);
            SpawnedBall.GetComponent<BallSprite>().spawner = this.gameObject;
        }
        else
        {
            SpawnedBall.transform.position = spawnPoint.position;
            SpawnedBall.SetActive(true);
        }
    }

    /// <summary>
    /// Método para volver a idle
    /// </summary>
    public void ReturnToIdle()
    {
        playerAnimator.SetInteger("State", 0);
    }

    /// <summary>
    /// Método para esperar a que termine la accion
    /// </summary>
    public void WaitForEnd()
    {
        makingAnimation = false;
    }

}
