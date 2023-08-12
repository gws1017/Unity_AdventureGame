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

        //if(!SpaceDown && Time.timeScale != 0)
        //{
        //    SpaceDown = Input.GetKeyDown(KeyCode.Space);
        //}
        //HorizonItalInput = Input.GetAxisRaw("Horizontal");
        //VerticalInput = Input.GetAxisRaw("Vertical");
    }

    public void AttackClick()
    {
        if(!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = true;
        }
    }

    public void SlideClick ()
    {
        if (!SpaceDown && Time.timeScale != 0)
        {
            SpaceDown = true;
        }
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
