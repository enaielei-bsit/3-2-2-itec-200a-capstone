/*
 * Date Created: Tuesday, November 23, 2021 7:01 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class Spawn : Controller
    {
        public virtual bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location
        )
        {
            return true;
        }

        public virtual void OnSpawn(Spawner spawner)
        {
        }
    }
}