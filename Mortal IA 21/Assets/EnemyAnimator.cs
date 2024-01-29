using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class IA : MonoBehaviour
{
    Animator enemyAnim;

  

    public void ReturnToIdle()
    {
        AnimateEnemy(0);
        Guess();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("ENTRA BOLA");
            AnimateEnemy(4);

        }
        else if (collision.gameObject.CompareTag("Player") )
        {
       
            if ( !defended)
            {
                Debug.Log("ENTRA PLAYER");
                AnimateEnemy(3);

            }
            else
                ReturnToIdle();




        }

    }

    void AnimateEnemy(int state)
    {
        enemyAnim.SetInteger("State", state);
    }

}
