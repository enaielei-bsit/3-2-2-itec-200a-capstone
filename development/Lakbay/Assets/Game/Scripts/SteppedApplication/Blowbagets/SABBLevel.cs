/*
 * Date Created: Sunday, December 19, 2021 8:46 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Blowbagets
{
    using Core;
    using UnityEngine.Localization;
    using Utilities;

    [CreateAssetMenu(
        fileName = "BlowbagetsLevel",
        menuName = "Game/Stepped Application/Blowbagets Level"
    )]
    public class SABBLevel : Asset, ILocalizable
    {
        public LocalizedAsset<TextAsset> batteryFile;
        public BlowbagetsInfo battery;
        public LocalizedAsset<TextAsset> lightsFile;
        public BlowbagetsInfo lights;
        public LocalizedAsset<TextAsset> oilFile;
        public BlowbagetsInfo oil;
        public LocalizedAsset<TextAsset> waterFile;
        public BlowbagetsInfo water;
        public LocalizedAsset<TextAsset> brakesFile;
        public BlowbagetsInfo brakes;
        public LocalizedAsset<TextAsset> airFile;
        public BlowbagetsInfo air;
        public LocalizedAsset<TextAsset> gasFile;
        public BlowbagetsInfo gas;
        public LocalizedAsset<TextAsset> engineFile;
        public BlowbagetsInfo engine;
        public LocalizedAsset<TextAsset> tiresFile;
        public BlowbagetsInfo tires;
        public LocalizedAsset<TextAsset> selfFile;
        public BlowbagetsInfo self;

        public virtual void Localize(Localizer localizer)
        {
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                batteryFile, (a) => LoadBlowbagetsInfo(ref battery, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                lightsFile, (a) => LoadBlowbagetsInfo(ref lights, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                oilFile, (a) => LoadBlowbagetsInfo(ref oil, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                waterFile, (a) => LoadBlowbagetsInfo(ref water, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                brakesFile, (a) => LoadBlowbagetsInfo(ref brakes, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                airFile, (a) => LoadBlowbagetsInfo(ref air, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                gasFile, (a) => LoadBlowbagetsInfo(ref gas, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                engineFile, (a) => LoadBlowbagetsInfo(ref engine, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                tiresFile, (a) => LoadBlowbagetsInfo(ref tires, a));
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                selfFile, (a) => LoadBlowbagetsInfo(ref self, a));
        }

        public virtual BlowbagetsInfo LoadBlowbagetsInfo(TextAsset asset)
        {
            return asset?.text.DeserializeAsYaml<BlowbagetsInfo>();
        }

        public virtual void LoadBlowbagetsInfo(ref BlowbagetsInfo info, TextAsset asset)
        {
            if (info != null && asset != null)
            {
                var newInfo = LoadBlowbagetsInfo(asset);
                info.title = newInfo.title;
                info.image = newInfo.image;

                if (info.content != null && info.content.Count == newInfo.content.Count)
                {
                    foreach (var entry in newInfo.content.Enumerate())
                    {
                        info.content[entry.Key] = entry.Value;
                    }
                }
                else info.content = newInfo.content;

                info.description = newInfo.description;
                info.source = newInfo.source;
            }
            else if (info == null && asset)
            {
                info = LoadBlowbagetsInfo(asset);
            }
        }

        public virtual void Load()
        {
            LoadBlowbagetsInfo(ref battery, batteryFile.LoadAsset());
            LoadBlowbagetsInfo(ref lights, lightsFile.LoadAsset());
            LoadBlowbagetsInfo(ref oil, oilFile.LoadAsset());
            LoadBlowbagetsInfo(ref water, waterFile.LoadAsset());
            LoadBlowbagetsInfo(ref brakes, brakesFile.LoadAsset());
            LoadBlowbagetsInfo(ref air, airFile.LoadAsset());
            LoadBlowbagetsInfo(ref gas, gasFile.LoadAsset());
            LoadBlowbagetsInfo(ref engine, engineFile.LoadAsset());
            LoadBlowbagetsInfo(ref tires, tiresFile.LoadAsset());
            LoadBlowbagetsInfo(ref self, selfFile.LoadAsset());
        }
    }
}