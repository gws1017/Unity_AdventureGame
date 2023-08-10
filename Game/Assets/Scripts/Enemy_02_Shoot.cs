using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_02_Shoot : MonoBehaviour
{
    public Transform ShootingPoint;
    public GameObject Bullet;

    public Character Owner;

    private void Awake()
    {
        Owner = GetComponent<Character>();
    }

    public void ShootBullet()
    {
        Instantiate(Bullet,ShootingPoint.position,Quaternion.LookRotation(ShootingPoint.forward));
    }

    private void Update()
    {
        Owner.RotateToTarget();
    }
}
