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
    [ExecuteInEditMode]
    public class SkillWidget : Core.Widget {
        protected Skill _skill;

        public Image image;
        public Image cooldown;
        public TextMeshProUGUI label;
        public TextMeshProUGUI description;
        public TextMeshProUGUI instances;
        public virtual Button button => GetComponentInChildren<Button>();

        public override void Update() {
            base.Update();
            if(_skill != null) {
                if(image && image.sprite != _skill.image) {
                    image.sprite = _skill.image;
                }

                if(cooldown && cooldown.sprite != _skill.image) {
                    cooldown.sprite = _skill.image;
                }

                if(label && label.text != _skill.label)
                    label.SetText(_skill.label); 

                if(description && description.text != _skill.description)
                    description.SetText(_skill.description); 

                if(_skill.instances >= 0) {
                    string inst = Mathf.Clamp(_skill.instances, 0, 99).ToString();
                    if(instances && instances.text != inst)
                        instances.SetText(inst);
                }

                if(_skill.cooldownProgress > 0.0f) {
                    if(cooldown) {
                        if(!cooldown.gameObject.activeSelf)
                            cooldown.gameObject.SetActive(true);

                        cooldown.fillAmount = 1 - _skill.cooldownProgress;
                    }
                } else {
                    if(cooldown) {
                        if(cooldown.gameObject.activeSelf)
                            cooldown.gameObject.SetActive(false);

                        cooldown.fillAmount = 1.0f;
                    }
                }
            }
        }

        public virtual void Set(Caster caster, Buffable target, Skill skill) {
            _skill = skill;
            if(Application.isPlaying) {
                button?.onClick.RemoveAllListeners();
                button?.onClick.AddListener(() => {
                    skill.Cast(caster, target);
                });
            }
        }
    }
}