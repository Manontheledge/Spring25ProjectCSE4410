using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float spd = 10f;
    public float blast; // Blast radius variable
    public float lifetime = 10f; // Lifetime of the rocket in seconds

    private float timeAlive = 0f; // Timer to track the rocket's lifetime

    // Update is called once per frame
    void Update()
    {
        // Move the rocket forward
        transform.Translate(0, spd * Time.deltaTime, 0);

        // Track time and destroy the rocket after its lifetime expires(This is a failsafe in case the rocket does not detonate on contact)
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime)
        {
            AudioClip explosion = Resources.Load<AudioClip>("Game sounds/weapon sounds/rocketLauncher/rocketLauncherExplosion");
            AudioSource.PlayClipAtPoint(explosion, transform.position);
            Destroy(gameObject); // Destroy the rocket after lifetime ends
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect objects within the blast radius
        Collider[] victims = Physics.OverlapSphere(transform.position, blast);
        foreach (Collider victim in victims)
        {
            EnemyWander enemy = victim.GetComponent<EnemyWander>();
            TargetReaction target = victim.GetComponent<TargetReaction>();
            if (enemy != null && target != null)
            {
                target.HitReaction(); // React to hit
            }
        }

        // Destroy the rocket and play explosion sound upon collision
        AudioClip explosion = Resources.Load<AudioClip>("Game sounds/weapon sounds/rocketLauncher/rocketLauncherExplosion");
        AudioSource.PlayClipAtPoint(explosion, transform.position);
        Destroy(gameObject);
    }
}
