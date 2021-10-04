using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public Camera camera;
    private Vector3 position;
    public bool isFocused;
    public int CameraSize;
    public GameObject offCourceFocusPoint;
    public GameObject GameOverFocusPoint;
    public float size = 5;
    public static CameraManager Instance;
    public GameObject offCourceMenu;

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
            camera.orthographicSize  += 0.03f;
         player.OnDisable();
         transform.position = Vector3.Lerp(this.transform.position, offCourceFocusPoint.transform.position, Time.deltaTime);
         var distance = this.transform.position.magnitude - offCourceFocusPoint.transform.position.magnitude;
         /*if (distance*distance < 0.2)
         {
             player.OnEnable();
             isFocused = false;
         }*/
         position.z = -10f;
     }

    void Update()
    {
        isFocused = offCourceMenu.activeSelf;
        if (isFocused)
             FocusOnObject(offCourceFocusPoint, 40f);
        else
            FollowPlayer();
    }
}
