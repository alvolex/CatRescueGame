using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEvent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CatCopCar copCar))
        {
            copCar.IsHelping();
            Destroy(gameObject);
        }
    }
}
