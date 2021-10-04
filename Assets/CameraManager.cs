using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public Camera camera;
    private Vector3 position;
    public bool IsFocusedOffCource;
    public bool IsFocusedGameOver;
    public int CameraSize;
    public GameObject offCourceFocusPoint;
    public GameObject GameOverFocusPoint;
    public float size = 5;
    public static CameraManager Instance;
    public GameObject offCourceMenu;
    public GameObject planet;

    private void Awake()
    {
        Instance = this;
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }
        camera = FindObjectOfType<Camera>();
    }

    private void FollowPlayer()
    {
        size = 5;
        // player.OnEnable();
        // if (player.isWithSword && camera.orthographicSize < 4.9)
        // {
        //     position.y += 2;
        //     camera.orthographicSize += Time.deltaTime * 4;
        // }
        // else if (player.isWithSword && camera.orthographicSize > 5.3)
        // {
        //     position.y += 2;
        //     camera.orthographicSize -= Time.deltaTime * 4;
        // }
        // else if (camera.orthographicSize > 3 && !player.isWithSword)
        // {
        //     position.y += 3.5f;
        //     camera.orthographicSize -= Time.deltaTime * 4;
        // }

        position = player.transform.position;
        position.z = -10f;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime*2.5f);

    }

    public void ChangePlanetSize(GameObject planet)
    {
        planet.transform.localScale += new Vector3(0.0005f, 0.0005f, 0.0005f) ;

    }


    public void FocusOnObject(GameObject gameObject, float cameraSize)
    {
        /*         if (camera.orthographicSize < size)
                 {
                     position.y += 2;
                     camera.orthographicSize += Time.deltaTime * 4;
                 }
                 else if (camera.orthographicSize > size+0.3)
                 {
                     position.y += 2;
                     camera.orthographicSize -= Time.deltaTime * 4;
                 }*/
         if(camera.orthographicSize < cameraSize)
            camera.orthographicSize  += 0.05f;
         player.OnDisable();
         transform.position = Vector3.Lerp(this.transform.position, gameObject.transform.position, Time.deltaTime);
         var distance = this.transform.position.magnitude - gameObject.transform.position.magnitude;
        if (distance * distance < 0.2)
        {
            /**/player.OnEnable();
            IsFocusedGameOver = false;
        }
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10f);
        if (IsFocusedGameOver)
        {
            ChangePlanetSize(planet);
        }
     }

    void Update()
    {
        IsFocusedOffCource = offCourceMenu.activeSelf;
        if (IsFocusedOffCource)
        {
            IsFocusedOffCource = true;
            FocusOnObject(offCourceFocusPoint, 40f);
        }
        else if (IsFocusedGameOver)
        {
            FocusOnObject(GameOverFocusPoint, 150f);
            position.z = -10f;
        }
            
        else
            FollowPlayer();
    }
}
