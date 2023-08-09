using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizonItalInput;
    public float VerticalInput;

    public bool MouseButtonDown;

    void Update()
    {
        if(!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }
        HorizonItalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        MouseButtonDown = false;
        HorizonItalInput = 0;
        VerticalInput = 0;
    }
}
