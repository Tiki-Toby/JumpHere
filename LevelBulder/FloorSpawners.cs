using Assets.Scripts.Data;
using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Level
{
    [Serializable]
    abstract class FloorSpawner
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] protected float chanceRise;
        [SerializeField] protected float dropDownChanceAfterSpawn = 1;
        protected float counter;
        protected bool IsSpawnReady
        {
            get
            {
                float val = UnityEngine.Random.value;
                return (val < 0.5f + counter && val > 0.5f - counter);
            }
        }
        public void Reset() => counter = Mathf.Clamp(counter - dropDownChanceAfterSpawn, 0, counter);
        public void Update() => counter += chanceRise;
        public bool TryGenerate(ref TrackData data)
        {
            if (!enabled)
                return false;

            bool isSuccess = IsSpawnReady;
            if (isSuccess)
            {
                Generate(ref data);
                Reset();
            }
            return isSuccess;
        }
        public abstract void Generate(ref TrackData data);
    }
    [Serializable]
    class DefaultSpawner : FloorSpawner
    {
        [SerializeField] float chanceForMovingPlatform;
        [SerializeField] float reduceImpactOfCurrentScore;
        [SerializeField] GameObject[] obstacles;
        [SerializeField] GameObject[] floorObstacle;
        GameObject RandomObstacle => obstacles[UnityEngine.Random.Range(0, obstacles.Length)];
        GameObject RandomFloorObstacle => floorObstacle[UnityEngine.Random.Range(0, floorObstacle.Length)];
        public override void Generate(ref TrackData data)
        {
            if (UnityEngine.Random.value + SessionData.path / reduceImpactOfCurrentScore > chanceForMovingPlatform)
            {
                AbstractFloor floor = data.SpawnNextFloor(data.RandomFloor);
                floor.AddObjectOnFloorCenter(GameObject.Instantiate(RandomObstacle).transform);
            }
            else
                data.SpawnNextFloor(RandomFloorObstacle);
        }
    }
    [Serializable]
    class TurnSpawner : FloorSpawner
    {
        [Serializable]
        struct Obstacle
        {
            public GameObject obstacle;
            public Vector3 localNormilizeOffset;
            public Vector3 localOffset;
        }
        [SerializeField] float chanceForObstacleOnTurn;
        [SerializeField] Obstacle[] breakObstacles;
        Obstacle RandomBreakObstacle => breakObstacles[UnityEngine.Random.Range(0, breakObstacles.Length)];
        public override void Generate(ref TrackData data)
        {
            data.SpawnNextFloor(data.RandomFloor);
            data.TurnDirection();
            AbstractFloor floor = data.SpawnNextFloor(data.RandomFloor, true, true);
            Obstacle breakObstacle = RandomBreakObstacle;
            if (chanceForObstacleOnTurn > UnityEngine.Random.value)
                floor.AddObjectOnFloorWithOffetCenter(GameObject.Instantiate(breakObstacle.obstacle).transform, breakObstacle.localNormilizeOffset, Vector3.back * data.distanceJump / 2f + breakObstacle.localOffset);
            bool isNeedMirrowed = data.Direction.x == 0;
            floor.floorObject.localScale = new Vector3(isNeedMirrowed ? -1 : 1, 1, 1);
        }
    }
    [Serializable]
    class LiftSpawner : FloorSpawner
    {
        [SerializeField] GameObject[] lifts;
        GameObject RandomLift => lifts[UnityEngine.Random.Range(0, lifts.Length)];
        public override void Generate(ref TrackData data)
        {
            data.SpawnNextFloor(RandomLift);
        }
    }
    [Serializable]
    class PitSpawner : FloorSpawner
    {
        [SerializeField] int maxSpaceInPit;
        public override void Generate(ref TrackData data)
        {
            for (int i = 0; i < maxSpaceInPit; i++)
            {
                AbstractFloor floor = data.SpawnNextFloor(data.planeForReplacement);
                floor.floorObject.gameObject.SetActive(false);
            }
        }
    }
    [Serializable]
    class GivenSpawner : FloorSpawner
    {
        [SerializeField] Item[] items;
        [SerializeField] GameObject[] floor;
        GameObject RandomFloor => floor[UnityEngine.Random.Range(0, floor.Length)];
        GameObject RandomItem => items[UnityEngine.Random.Range(0, items.Length)].gameObject;
        public override void Generate(ref TrackData data)
        {
            AbstractFloor floor = data.SpawnNextFloor(RandomFloor);
            floor.AddObjectOnFloorCenter(GameObject.Instantiate(RandomItem).transform, data.Direction);
        }
    }
}
