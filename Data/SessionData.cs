using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    static class SessionData
    {
        public static long startTime;
        public static float points;
        public static float velocity;
        public static float path;
        public static float timer;
        public static bool isSecondChance;
        public static int extraPanelsCount { get; private set; }

        public static void Update(float deltaTime)
        {
            timer += deltaTime;
            //velocity = VelocityFromPath();
        }
        public static void AddPath(float deltaPath)
        {
            path += deltaPath;
            points += PathToPoint(deltaPath);
        }
        public static float GetTurnWidth(float width)
        {
            return width + (Mathf.Exp(path / 750f));
        }
        private static float VelocityFromPath()
        {
            return SettingsData.StartVelocity + (0.5f * Mathf.Exp(path / 750f));
        }
        private static float PathToPoint(float path)
        {
            return path;
        }
        public static void TakeExtraPanel() => extraPanelsCount--;
        public static void Init()
        {
            points = 0;
            path = 0;
            timer = 0;
            extraPanelsCount = 0;
            isSecondChance = false;
            velocity = SettingsData.StartVelocity;
            startTime = DateTime.Now.Ticks;
        }
        public static void Init(long points, float velocity, float path, float timer, int extraPanelsCount)
        {
            SessionData.points = points;
            SessionData.velocity = velocity;
            SessionData.path = path;
            SessionData.timer = timer;
            SessionData.extraPanelsCount = extraPanelsCount;
            isSecondChance = false;
        }
    }
}
