/*
 * Date Created: Friday, December 10, 2021 2:50 PM
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
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class PlaySelectionUI : Controller {
        public Player player;
        public MessageBoxUI message;
        public ToggleGroup group;

        [Space]
        public LocalizedString useCheckpoint;

        public new virtual IEnumerator Start() {
            base.Start();
            yield return new WaitUntil(() => Initialization.finished);
            group?.GetFirstActiveToggle().onValueChanged.Invoke(true);
        }

        public virtual void Play() {
            if(Session.checkpointController.IsCheckpointValid()) {
                message?.ShowConfirmation(
                    useCheckpoint,
                    onYes: () => Session.checkpointController.LoadCheckpoint(
                        player
                    ),
                    onNo: () => player?.Play()
                );
            } else {
                player?.Play();
            }
        }
    }
}