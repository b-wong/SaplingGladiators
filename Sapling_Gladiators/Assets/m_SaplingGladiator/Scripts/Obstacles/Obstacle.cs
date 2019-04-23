using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Obstacle : MonoBehaviour
{

    public enum ObstacleType {Rock,Worm,Root}
    public ObstacleType myObstacleType;
    private bool justHit;

    public void OnHit()
    {
        if (!justHit)
        {
            switch (myObstacleType)
            {
                case ObstacleType.Rock: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/RockHit"); break;
            case ObstacleType.Worm: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/WormHit"); break;
            case ObstacleType.Root: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/RootHit"); break;
            }
            justHit = true;
        }
    }

    public void OnBreak()
    {
        switch (myObstacleType)
        {
            case ObstacleType.Rock: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/RockBroke"); break;
            case ObstacleType.Worm: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/WormHit"); break;
            case ObstacleType.Root: FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/RootBroke"); break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Twig"))
        {
          OnHit();  
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Twig"))
        {
          OnHit();  
        }
    }
}
