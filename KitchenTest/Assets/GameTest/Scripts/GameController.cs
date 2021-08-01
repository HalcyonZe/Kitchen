using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerPause()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false;
        GameObject.Find("Main Camera").GetComponent<CameraController>().CanCamera = false;
        GameObject.Find("Main Camera").GetComponent<MouseControl>().CanClick = false;
    }

    public void PlayerPlay()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = true;
        GameObject.Find("Main Camera").GetComponent<CameraController>().CanCamera = true;
        GameObject.Find("Main Camera").GetComponent<MouseControl>().CanClick = true;
    }

}
