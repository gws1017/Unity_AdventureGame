using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizonItalInput;
    public float VerticalInput;

    public bool MouseButtonDown;
    public bool SpaceDown;

    void Update()
    {
        if(!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }

        if(!SpaceDown && Time.timeScale != 0)
        {
            SpaceDown = Input.GetKeyDown(KeyCode.Space);
        }
        HorizonItalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        MouseButtonDown = false;
        SpaceDown = false;
        HorizonItalInput = 0;
        VerticalInput = 0;
    }
}
