using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] public int charge;
    int maxCharge;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void use(int amount) {
        charge -= amount;
    }
}
