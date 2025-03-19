using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherPUP : Collectible
{
    public RayShooting rayShooter;  // Reference to RayShooting
    protected override void Touched()
    {
        base.Touched();
    }

    private void Start()
    {
        // Get the RayShooting component attached to the GameObject
        rayShooter = GetComponent<RayShooting>();
    }
    private void OnTriggerEnter(Collider recipient)
    {
        PlayerCharacter playerChar = recipient.GetComponent<PlayerCharacter>();
        if (playerChar != null)
        {
            //rayShooter.hasRocketLauncher = true;
            Touched();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
