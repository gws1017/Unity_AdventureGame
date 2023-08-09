using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizonItalInput;
    public float VerticalInput;

    void Update()
    {
        HorizonItalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        HorizonItalInput = 0;
        VerticalInput = 0;
    }
}
