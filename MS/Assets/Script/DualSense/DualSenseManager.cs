using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniSense;

namespace DualSenseSample.Inputs
{
    public class DualSenseManager : AbstractDualSenseBehaviour
    {
       



        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
           

            
        }

        void SetTriggerForce()
        {
            GetComponent<DualSenseTrigger>().LeftContinuousForce = (byte)(DualSense.leftTrigger.value * 255);
            GetComponent<DualSenseTrigger>().RightContinuousForce = (byte)(DualSense.rightTrigger.value * 255);
        }

    }
}