/*
 * Date Created: Wednesday, January 5, 2022 2:25 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System.Linq;

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Tailgating
{
    using Core;
    using Utilities;

    public class LandmarkTrigger : Trigger
    {
        public virtual bool triggered
        {
            get
            {
                return GetComponentsInChildren<Collider>().All((c) => !c.enabled);
            }
            set
            {
                foreach (var col in GetComponentsInChildren<Collider>())
                {
                    col.enabled = value;
                }
            }
        }
    }
}