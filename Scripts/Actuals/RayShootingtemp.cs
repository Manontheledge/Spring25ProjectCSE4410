using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayShooting : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;

    //Enum with possible weapons/linked variable indicating equipped Weapon(Please don't change the order)
    public enum Weapon
    {
        Pistol,
        Shotgun,
        Machinegun,
        RocketLauncher
    }
    public Weapon weapon;

    //Weapon Parameters
    public float shotdelay;
    private float shotcd;
    private int shots = 8;
    public float shotdeviation = 5.0f; //NOTE: This is measured in pixels
    [SerializeField] GameObject rocketPF;
    private GameObject rocket;

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.CHANGE_SHOTGUN, EquipShotty);
        Messenger.AddListener(GameEvent.CHANGE_RIFLE, EquipRifle);
        Messenger.AddListener(GameEvent.CHANGE_LAUNCHER, EquipLauncher);
        Messenger.AddListener(GameEvent.CHANGE_PISTOL, EquipPistol);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.CHANGE_SHOTGUN, EquipShotty);
        Messenger.RemoveListener(GameEvent.CHANGE_RIFLE, EquipRifle);
        Messenger.RemoveListener(GameEvent.CHANGE_LAUNCHER, EquipLauncher);
        Messenger.RemoveListener(GameEvent.CHANGE_PISTOL, EquipPistol);
    }

    private void EquipShotty()
    {
        if (weapon != Weapon.Shotgun)
            weapon = Weapon.Shotgun;
    }
    private void EquipRifle()
    {
        if (weapon != Weapon.Machinegun)
            weapon = Weapon.Machinegun;
    }
    private void EquipLauncher()
    {
        if (weapon != Weapon.RocketLauncher)
            weapon = Weapon.RocketLauncher;
    }
    private void EquipPistol()
    {
        if (weapon != Weapon.Pistol)
            weapon = Weapon.Pistol;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        weapon = Weapon.Pistol;
        
        //Lock/Hide cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //Initialize shot cooldown
        shotcd = -shotdelay;
    }

    private void OnGUI()
    {
        int SIZE = 200;

        float posX = cam.pixelWidth / 2;
        float posY = cam.pixelHeight / 2;

        //Draws a crosshair
        GUI.Label(new Rect(posX, posY, SIZE, SIZE), "*");
    }

    //Coroutine Sphere
    private IEnumerator SphereIndic(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //Create a sphere at impact position
        sphere.transform.position = pos;
        
        sphere.transform.localScale = Vector3.one * 0.1f;

        //Wait 1 second
        yield return new WaitForSeconds(1);

        Destroy(sphere);   
    }

    //Coroutine Shoot
    IEnumerator Shoot()
    {
        /*
        Create a Vector3 to store the location of the moddle of the screen
        Divide the width & height by 2 to get midpoint and store as x and y, with z being 0.
        */
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

        Ray ray = cam.ScreenPointToRay(point);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            UnityEngine.Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 3f);
            GameObject hitobject = hit.transform.gameObject;
            TargetReaction target = hitobject.GetComponent<TargetReaction>();

            //UnityEngine.Debug.Log("Hit: " + hit.point);

            if (target != null)
            {
                //UnityEngine.Debug.Log("Hit!");
                target.HitReaction();
                Messenger.Broadcast(GameEvent.ENEMY_HIT);
            }
            else
            {
                StartCoroutine(SphereIndic(hit.point));
            }
        }
        yield return null;
    }

    //Coroutine Shoot but add shotgun spread and multiple shots
    IEnumerator Shotgun()
    {
        for (int i = 0; i < shots; i++)
        {
            /*
            Create a Vector3 to store the location of the moddle of the screen
            Divide the width & height by 2 to get midpoint and store as x and y, with z being 0.
            */
            Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

            //Create random offset 
            float offsetx = UnityEngine.Random.Range(-shotdeviation, shotdeviation);
            float offsety = UnityEngine.Random.Range(-shotdeviation, shotdeviation);

            //Ensure circular spread(x^2+y^2 =z^2)
            while (offsetx * offsetx + offsety * offsety > shotdeviation * shotdeviation)
            {
                offsetx = UnityEngine.Random.Range(-shotdeviation, shotdeviation);
                offsety = UnityEngine.Random.Range(-shotdeviation, shotdeviation);
            }

            //Generate random point near the center of the screen
            point = point + new Vector3(offsetx, offsety, 0);

            Ray ray = cam.ScreenPointToRay(point);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                UnityEngine.Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 3f);
                UnityEngine.Debug.Log(point);
                GameObject hitobject = hit.transform.gameObject;
                TargetReaction target = hitobject.GetComponent<TargetReaction>();

                //UnityEngine.Debug.Log("Hit: " + hit.point);

                if (target != null)
                {
                    //UnityEngine.Debug.Log("Hit!");
                    target.HitReaction();
                    Messenger.Broadcast(GameEvent.ENEMY_HIT);
                }
                else
                {
                    StartCoroutine(SphereIndic(hit.point));
                }
            }
        }
        UnityEngine.Debug.Log("B O O M");
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //Shot cooldown increments w/respect to time
        shotcd += Time.deltaTime;

        //If Assault Rifle equipped, automatic fire
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && weapon == Weapon.Machinegun) 
        {
            //When enough time passes between the last shot...
            if (shotcd >= shotdelay)
            {
                //Update shot timer
                shotcd = 0;
                //Fire
                StartCoroutine(Shoot());
            }
        }
        //Otherwise...
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Note the weapon equipped and use that specific firing type(All are semi auto)
            switch(weapon)
            {
                case Weapon.RocketLauncher:
                    if(rocket == null)
                    {
                        rocket = Instantiate(rocketPF) as GameObject;
                        rocket.transform.position = transform.TransformPoint(Vector3.forward * 2f);
                        rocket.transform.rotation = transform.rotation;
                        rocket.transform.rotation *= Quaternion.Euler(90f, 0f, 0f);
                        UnityEngine.Debug.Log("FIRE IN THE HOLE");
                    }
                    break;
                case Weapon.Shotgun:
                    StartCoroutine(Shotgun());
                    break;
                case Weapon.Pistol: 
                    StartCoroutine(Shoot());
                    break;
                default:
                    break;
            }

        }

    }
}
