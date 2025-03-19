using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object class: DO NOT attach this to an object!!!
public class Collectible : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;
    //Base behaviour for all pickups on contact w/player
    //TODO: Add float up and down anim
    private bool hasBeenCollected = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    protected virtual void Touched()
    {
        if (hasBeenCollected) return;
        hasBeenCollected = true;

        if (pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickupSound.length); // Wait for the sound to finish
        Destroy(gameObject);
    }
}