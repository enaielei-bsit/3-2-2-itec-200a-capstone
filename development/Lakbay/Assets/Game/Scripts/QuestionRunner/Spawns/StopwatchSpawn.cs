/*
 * Date Created: Wednesday, October 13, 2021 10:39 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns
{
    using Core;

    public class StopwatchSpawn : SkillSpawn<Buffs.StopwatchBuff>
    {
        public override void OnTrigger(QRPlayer player)
        {
            base.OnTrigger(player);
            gameObject.SetActive(false);
        }

        public override bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location)
        {
            if (base.OnSpawnCheck(spawner, locations, location))
            {
                return !Session.qrLevel.done;
            }

            return false;
        }
    }
}