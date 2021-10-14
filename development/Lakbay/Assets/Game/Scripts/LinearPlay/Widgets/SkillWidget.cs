/*
 * Date Created: Thursday, October 14, 2021 11:40 AM
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

using TMPro;

namespace Ph.CoDe_A.Lakbay.LinearPlay.Widgets {
    public class SkillWidget : Core.Widget {
        public Image image;
        public TextMeshProUGUI label;
        public TextMeshProUGUI description;

        public virtual Button button => GetComponentInChildren<Button>();

        public virtual void Set(
            Sprite image, string label, string description
        ) {
            if(this.image) this.image.sprite = image;
            this.label?.SetText(label);
            this.description?.SetText(description);
        }

        public virtual void Set(Buffable buffable, Skill skill) {
            Set(skill.image, skill.label, skill.description);
            button?.onClick.RemoveAllListeners();
            button?.onClick.AddListener(() => skill.Cast(buffable));
        }
    }
}