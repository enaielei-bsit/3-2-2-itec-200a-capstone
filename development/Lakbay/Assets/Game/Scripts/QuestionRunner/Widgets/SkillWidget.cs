/*
 * Date Created: Thursday, October 14, 2021 11:40 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Widgets
{
    [ExecuteInEditMode]
    public class SkillWidget : Core.Controller
    {
        protected Skill _skill;
        protected Buff _buff;

        public Image image;
        public Image cooldown;
        public Slider buffProgress;
        public TextMeshProUGUI label;
        public TextMeshProUGUI description;
        public TextMeshProUGUI instances;
        public Animator instancesChangeEffect;
        public virtual Button button => GetComponentInChildren<Button>();

        public override void Update()
        {
            base.Update();
            if (_skill != null)
            {
                if (image && image.sprite != _skill.image)
                {
                    image.sprite = _skill.image;
                }

                if (cooldown && cooldown.sprite != _skill.image)
                {
                    cooldown.sprite = _skill.image;
                }

                if (label && label.text != _skill.label)
                    label.SetText(_skill.label);

                if (description && description.text != _skill.description)
                    description.SetText(_skill.description);

                if (_skill.instances >= 0)
                {
                    string inst = Mathf.Clamp(_skill.instances, 0, 99).ToString();
                    if (instances && instances.text != inst)
                    {
                        if (instancesChangeEffect
                            && _skill.instances > int.Parse(instances.text))
                        {
                            instancesChangeEffect.SetFloat("timeScale", 2.25f);
                            instancesChangeEffect.SetTrigger("changed");
                        }
                        instances.SetText(inst);
                    }
                }

                if (_skill.cooldownProgress > 0.0f)
                {
                    if (cooldown)
                    {
                        if (!cooldown.gameObject.activeSelf)
                            cooldown.gameObject.SetActive(true);

                        cooldown.fillAmount = 1 - _skill.cooldownProgress;
                    }
                }
                else
                {
                    if (cooldown)
                    {
                        if (cooldown.gameObject.activeSelf)
                            cooldown.gameObject.SetActive(false);

                        cooldown.fillAmount = 1.0f;
                    }
                }

                if (buffProgress)
                {
                    // var pimg = buffProgress.fillRect?.GetComponent<Image>();
                    // if(pimg) pimg.color = _skill.color;
                    if (_buff)
                    {
                        if (_buff.progress > 0.0f)
                        {
                            if (!buffProgress.gameObject.activeSelf)
                                buffProgress.gameObject.SetActive(true);
                            buffProgress.value = 1.0f - _buff.progress;
                        }
                        else
                        {
                            if (buffProgress.gameObject.activeSelf)
                                buffProgress.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if (buffProgress.gameObject.activeSelf)
                            buffProgress.gameObject.SetActive(false);
                        buffProgress.value = 0.0f;
                    }
                }
            }
        }

        public virtual void Set(Caster caster, Buffable target, Skill skill)
        {
            _skill = skill;
            if (Application.isPlaying)
            {
                button?.onClick.RemoveAllListeners();
                button?.onClick.AddListener(() =>
                {
                    var buff = skill.Cast(caster, target);
                    if (buff) _buff = buff;
                });
            }
        }
    }
}