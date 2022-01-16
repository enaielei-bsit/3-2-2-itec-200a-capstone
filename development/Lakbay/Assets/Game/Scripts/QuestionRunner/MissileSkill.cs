/*
 * Date Created: Saturday, October 23, 2021 1:11 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.QuestionRunner
{
    [CreateAssetMenu(
        fileName = "MissileSkill",
        menuName = "Game/Question Runner/MissileSkill"
    )]
    public class MissileSkill : Skill
    {
        public float travelDistance = 200.0f;
    }
}