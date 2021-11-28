/*
 * Date Created: Tuesday, November 23, 2021 7:01 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class Spawn : Controller {
        public virtual bool OnSpawn(
            Spawner spawner, Transform[] locations, Transform location
        ) {
            return true;
        }
    }
}