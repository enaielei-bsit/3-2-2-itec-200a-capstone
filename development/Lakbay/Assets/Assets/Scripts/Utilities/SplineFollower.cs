/*
 * Date Created: Saturday, October 9, 2021 9:40 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Pixelplacement;

namespace Ph.CoDe_A.Lakbay.Utilities {
    public class SplineFollower : MonoBehaviour {
        //Public Variables:
        public float percentage = -1;
        public bool faceDirection;
        public bool positionX = true;
        public bool positionY = true;
        public bool positionZ = true;

        //Public Properties:
        public bool WasMoved
        {
            get
            {
                if (percentage != _previousPercentage || faceDirection != _previousFaceDirection) {
                    _previousPercentage = percentage;
                    _previousFaceDirection = faceDirection;
                    return true;
                } else {
                    return false;	
                }
            }
        }

        //Private Variables:
        float _previousPercentage;
        bool _previousFaceDirection;
        bool _detached;

        //Public Methods:
        public void UpdateOrientation (Spline spline)
        {
            var target = transform;

            //clamp percentage:
            if (!spline.loop) percentage = Mathf.Clamp01 (percentage);

            //look in direction of spline?
            if (faceDirection)
            {
                if (spline.direction == SplineDirection.Forward)
                {
                    target.LookAt (target.position + spline.GetDirection (percentage));
                }else{
                    target.LookAt (target.position - spline.GetDirection (percentage));
                }
            }

            var position = spline.GetPosition (percentage);
            position.x = positionX ? position.x : target.position.x;
            position.y = positionY ? position.y : target.position.y;
            position.z = positionZ ? position.z : target.position.z;
            target.position = position;
        }
    }
}