/*
 * Date Created: Thursday, November 4, 2021 6:22 PM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Widgets {
    using Core;

    public class ChoiceWidget : Content {
        public virtual Button button => GetComponentInChildren<Button>();

        public virtual void Build(QuestionWidget questionWidget, Choice choice) {
            if(choice == null) return;
            Build(choice.content);
            if(button) {
                button.onClick.RemoveAllListeners();
                if(Application.isPlaying) button.onClick.AddListener(
                    () => questionWidget.Answer(choice)
                );
            }
        }
    }
}