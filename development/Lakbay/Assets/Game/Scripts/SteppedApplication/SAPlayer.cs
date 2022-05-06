/*
 * Date Created: Saturday, December 18, 2021 4:32 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication
{
    using Core;

    public class SAPlayer : Player
    {
        [Header("Level")]
        public PrePlayUI prePlayUI;
        public GameMenuUI gameMenuUI;
        public HelpUI helpUI;

        public override void Build()
        {
            base.Build();
            prePlayUI?.gameObject.SetActive(true);
        }

        public virtual void Pause(bool screen)
        {
            gameMenuUI?.gameObject.SetActive(screen);
            Pause();
        }

        public virtual void Resume(bool screen)
        {
            gameMenuUI?.gameObject.SetActive(screen);
            Resume();
        }

        public virtual void Play(bool screen)
        {
            prePlayUI?.gameObject.SetActive(screen);
            if(!screen) {
                helpUI?.LaunchCurrent();
            }
        }
    }
}