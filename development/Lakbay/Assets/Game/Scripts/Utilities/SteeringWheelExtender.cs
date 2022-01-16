/*
 * Date Created: Tuesday, December 28, 2021 9:40 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using SimpleInputNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    [RequireComponent(typeof(SteeringWheel))]
    public class SteeringWheelExtender : MonoBehaviour
    {
        public virtual SteeringWheel wheel => GetComponent<SteeringWheel>();
        public UnityEvent<float> onSteer = new UnityEvent<float>();

        public virtual void Update()
        {
            if (wheel)
            {
                onSteer?.Invoke(wheel.Value);
            }
        }
    }
}