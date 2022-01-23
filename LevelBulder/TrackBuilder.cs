using Assets.Scripts.Data;
using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    //добавить генерацию повторных двигающихся платформ с разной амплитудой и скоростью
    class TrackBuilder : MonoBehaviour
    {
        [SerializeField] TrackData _trackData;
        [SerializeField] DefaultSpawner floorWithObstacle;
        [SerializeField] LiftSpawner liftSpawner;
        [SerializeField] TurnSpawner turnSpawner;
        [SerializeField] PitSpawner pitSpawner;
        public TrackData data => _trackData;
        public void StartGenerateWay(Vector3 startPosition, Vector3 direction)
        {
            _trackData.DefaultInitData(startPosition, direction);
            int tmp = _trackData.offsetForExtraAdd;
            for (int i = data.Lastindex; i < data.maxSavedTiles; i++)
            {
                GenerateNextTile();
            }
            _trackData.offsetForExtraAdd += tmp;
        }
        public void ReplaceNextFloor()
        {
            data.ReplaceNextFloor();
        }

        public void GeneratorNext()
        {
            _trackData.MoveNext();
            GenerateNextTile();
        }
        public void TurnCurrentDirection() =>
           _trackData.IsCurrentDirectX = !_trackData.IsCurrentDirectX;
        private void GenerateNextTile()
        {
            if (_trackData.offsetForExtraAdd > 0)
            {
                _trackData.offsetForExtraAdd--;
                return;
            }
            if (turnSpawner.TryGenerate(ref _trackData)) { Debug.Log("1"); }
            else if (liftSpawner.TryGenerate(ref _trackData)) { Debug.Log("2"); }
            else if (pitSpawner.TryGenerate(ref _trackData)) { Debug.Log("3"); }
            else if (floorWithObstacle.TryGenerate(ref _trackData)) { Debug.Log("4"); }
            else { _trackData.SpawnRandomFloor(); Debug.Log("+"); }

            turnSpawner.Update();
            floorWithObstacle.Update();
            liftSpawner.Update();
            pitSpawner.Update();
        }
    }
}
