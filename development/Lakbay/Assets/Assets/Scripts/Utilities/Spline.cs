/*
 * Date Created: Saturday, October 9, 2021 9:38 PM
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
    public class Spline : Pixelplacement.Spline {
        public List<SplineFollower> splineFollowers = new List<SplineFollower>();

        protected bool _anchorsChanged;
        protected int _previousChildCount;
        protected bool _previousLoopChoice;
        protected int _previousLength;
        protected SplineDirection _previousDirection;
        protected bool _lengthDirty = true;
        public new event System.Action OnSplineChanged;

        public void HangleLengthChange()
        {
            _lengthDirty = true;

            //fire event:
            OnSplineChanged?.Invoke();
        }

        public virtual void Update() {
            var followers = this.splineFollowers.ToArray();
            if (followers != null && followers.Length > 0 && Anchors.Length >= 2)
            {
                bool needToUpdate = false;

                //was anything else changed?
                if (_anchorsChanged || _previousChildCount != transform.childCount || direction != _previousDirection || loop != _previousLoopChoice)
                {
                    _previousChildCount = transform.childCount;
                    _previousLoopChoice = loop;
                    _previousDirection = direction;
                    _anchorsChanged = false;
                    needToUpdate = true;
                }

                //were any followers moved?
                for (int i = 0; i < followers.Length; i++)
                {
                    if(!followers[i]) continue;
                    if (followers[i].WasMoved || needToUpdate)
                    {
                        followers[i].UpdateOrientation(this);
                    }
                }
            }

            //manage anchors:
            bool anchorChanged = false;
            if (Anchors.Length > 1)
            {
                for (int i = 0; i < Anchors.Length; i++)
                {
                    //if this spline has changed notify and wipe cached percentage:
                    if (Anchors[i].Changed)
                    {
                        anchorChanged = true;
                        Anchors[i].Changed = false;
                        _anchorsChanged = true;
                    }

                    //if this isn't a loop then the first and last tangents are unnecessary:
                    if (!loop)
                    {
                        //turn first tangent off:
                        if (i == 0)
                        {
                            Anchors[i].SetTangentStatus(false, true);
                            continue;
                        }

                        //turn last tangent off:
                        if (i == Anchors.Length - 1)
                        {
                            Anchors[i].SetTangentStatus(true, false);
                            continue;
                        }

                        //turn both tangents on:
                        Anchors[i].SetTangentStatus(true, true);

                    }
                    else
                    {
                        //all tangents are needed in a loop:
                        Anchors[i].SetTangentStatus(true, true);
                    }
                }

            }

            //length changed:
            if (_previousLength != Anchors.Length || anchorChanged)
            {
                HangleLengthChange();
                _previousLength = Anchors.Length;
            }
        }
    }
}