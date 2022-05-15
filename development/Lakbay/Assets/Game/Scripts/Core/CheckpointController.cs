/*
 * Date Created: Wednesday, January 12, 2022 10:12 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Ph.CoDe_A.Lakbay.Core
{
    [Serializable]
    public class Checkpoint
    {
        public GameMode mode;
        public BuiltScene scene;

        public Checkpoint() { }

        public Checkpoint(
            GameMode mode,
            BuiltScene scene)
        {
            this.mode = mode;
            this.scene = scene;
        }

        public static Checkpoint Create()
        {
            var cp = new Checkpoint();
            cp.mode = Session.mode;
            cp.scene = SceneController.GetCurrent();
            return cp;
        }
    }

    public class CheckpointController : Controller
    {
        [SerializeField]
        protected string _filePath = "checkpoint.yaml";
        public virtual string filePath =>
            $"{Application.persistentDataPath}/{_filePath}";

        public Checkpoint checkpoint = new Checkpoint();

        public override void Awake()
        {
            base.Awake();
        }

        [ContextMenu("Save Settings")]
        public virtual void Save()
        {
            Helper.WriteFile(filePath, checkpoint.SerializeAsYaml());
        }

        [ContextMenu("Load Settings")]
        public virtual void Load()
        {
            if (File.Exists(filePath))
            {
                printLog($"`{filePath}` was found. Trying to load it...");
                try
                {
                    checkpoint = Helper.ReadFile(filePath).DeserializeAsYaml<Checkpoint>();
                    printLog($"Sucessfully loaded the file.");
                }
                catch
                {
                    printLog($"There was something wrong with the file. Resetting to default instead...");
                    checkpoint = GetDefault();
                }
            }
            else
            {
                printLog($"`{filePath}` was not found. Creating it first using the default...");
                Helper.WriteFile(filePath, GetDefault().SerializeAsYaml());
                printLog("Successfully created the file.");
                Load();
            }
        }

        public virtual bool IsCheckpointValid()
        {
            Load();

            // QuestionRunner
            var minScene = 1;
            // ParallelParking
            var maxScene = 9;
            int scene = (int)checkpoint.scene;

            return scene >= minScene && scene <= maxScene;
        }

        public static Checkpoint GetDefault()
        {
            return new Checkpoint(GameMode.NonPro, BuiltScene.MainMenu);
        }

        public virtual void LoadCheckpoint(Player player)
        {
            if (IsCheckpointValid())
            {
                player.SetMode(checkpoint.mode);
                player.LoadScene(checkpoint.scene);
            }
        }

        public virtual void SaveCheckpoint(Checkpoint checkpoint)
        {
            this.checkpoint = checkpoint;
            Save();
        }

        public virtual void SaveCheckpoint() => SaveCheckpoint(
            Checkpoint.Create());

        public virtual void Clear() => checkpoint = GetDefault();
    }
}