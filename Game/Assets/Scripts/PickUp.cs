using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Heal,Coin
    }

    public PickUpType Type;
    public int Value = 20;

    public ParticleSystem CollectedParticle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            if(CollectedParticle != null)
                Instantiate(CollectedParticle,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
