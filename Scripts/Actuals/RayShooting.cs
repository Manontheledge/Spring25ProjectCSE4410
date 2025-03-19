using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayShooter : MonoBehaviour
{
    private Camera cam;
    private AudioSource audioSource;

    public enum Weapon
    {
        Pistol = 0,
        Shotgun = 1,
        Machinegun = 2,
        RocketLauncher = 3,
        ShotgunPump = 4
    }

    public Weapon currentWeapon = Weapon.Pistol;

    [SerializeField] private AudioClip[] weaponSounds; // Assign in Inspector
    public float shotDelay;
    private float shotCooldown;
    private int shotgunPellets = 8;
    public float shotDeviation = 5.0f;
    public bool hasPistol = true;
    public bool hasShotgun = true; //temp enabled
    public bool hasMachineGun = true; //temp enabled
    public bool hasRocketLauncher = true; //temp enabled

    [SerializeField] private GameObject rocketPrefab;
    private GameObject rocket;

    private void Start()
    {
        cam = GetComponent<Camera>();
        audioSource = gameObject.AddComponent<AudioSource>();
        shotCooldown = 0f;
    }

    void PlayWeaponSound()
    {
        if ((int)currentWeapon < weaponSounds.Length && weaponSounds[(int)currentWeapon] != null)
        {
            audioSource.PlayOneShot(weaponSounds[(int)currentWeapon]);
        }
    }

    private void Update()
    {
        shotCooldown -= Time.deltaTime; // Reduce cooldown over time

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && currentWeapon == Weapon.Machinegun)
        {
            if (shotCooldown <= 0f)
            {
                shotDelay = 0.1f;
                PlayWeaponSound();
                shotCooldown = shotDelay; // Set cooldown before firing
                StartCoroutine(Fire());
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (shotCooldown <= 0f)
            {
                shotCooldown = shotDelay; // Prevent immediate re-firing

                switch (currentWeapon)
                {
                    case Weapon.RocketLauncher:
                        shotDelay = 4f;
                        shotCooldown = shotDelay; // Prevent immediate re-firing
                        PlayWeaponSound();
                        StartCoroutine(FireRocket());
                        break;
                    case Weapon.Shotgun:
                        shotDelay = 2f;
                        shotCooldown = shotDelay; // Prevent immediate re-firing
                        PlayWeaponSound();
                        StartCoroutine(FireShotgun());
                        break;
                    case Weapon.Pistol:
                        shotDelay = 0f;
                        shotCooldown = shotDelay; // Prevent immediate re-firing    
                        PlayWeaponSound();
                        StartCoroutine(Fire());
                        break;
                }
            }
        }

        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasPistol) currentWeapon = Weapon.Pistol;
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasShotgun) currentWeapon = Weapon.Shotgun;
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasMachineGun) currentWeapon = Weapon.Machinegun;
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasRocketLauncher) currentWeapon = Weapon.RocketLauncher;
    }

    private IEnumerator Fire()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ReactiveTarget target = hit.transform.gameObject.GetComponent<ReactiveTarget>();
            if (target != null)
            {
                target.ReactToHit();
            }
            else
            {
                StartCoroutine(SphereIndicator(hit.point));
            }
        }

        yield return null; // No need for a delay here since cooldown is handled in Update()
    }

    private IEnumerator FireShotgun()
    {
        for (int i = 0; i < shotgunPellets; i++)
        {
            Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            float offsetX = Random.Range(-shotDeviation, shotDeviation);
            float offsetY = Random.Range(-shotDeviation, shotDeviation);
            point += new Vector3(offsetX, offsetY, 0);

            Ray ray = cam.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ReactiveTarget target = hit.transform.gameObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    target.ReactToHit();
                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }

        yield return new WaitForSeconds(0.5f); // Shotgun pump delay
        currentWeapon = Weapon.ShotgunPump;
        PlayWeaponSound();
        currentWeapon = Weapon.Shotgun;
    }

    private IEnumerator FireRocket()
    {
        if (rocket == null)
        {
            rocket = Instantiate(rocketPrefab);
            rocket.transform.position = transform.TransformPoint(Vector3.forward * 2f);
            rocket.transform.rotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
        }

        yield return null;
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
