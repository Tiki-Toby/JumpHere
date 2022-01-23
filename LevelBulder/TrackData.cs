using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Level
{
    [Serializable]
    struct TrackData
    {
        [HideInInspector] public Vector3 Direction;
        [HideInInspector] public bool IsCurrentDirectX;
        public AbstractFloor PrevFloor { get; private set; }
        public AbstractFloor CurrentFloor { get; private set; }
        public AbstractFloor NextFloor { get; private set; }
        public int Lastindex => lastIndex;

        public int maxSavedTiles;
        public int offsetForDeleteTile;
        public float distanceJump;
        [HideInInspector] public int offsetForExtraAdd;
        [SerializeField] GameObject[] floors;
        [SerializeField] internal GameObject planeForReplacement;
        public GameObject RandomFloor => floors[UnityEngine.Random.Range(0, floors.Length)];
        private AbstractFloor[] _spawnedFloor;
        private int currentindex;
        private int lastIndex;
        public void DefaultInitData(Vector3 startPosition, Vector3 direction)
        {
            Direction = direction;
            IsCurrentDirectX = Direction.z == 0;

            currentindex = 0;
            lastIndex = 0;
            offsetForExtraAdd = offsetForDeleteTile;
            //инициализация или очистка предыдущей генерации
            if (_spawnedFloor == null)
                _spawnedFloor = new AbstractFloor[maxSavedTiles];
            ClearSpawnedPanel();
            _spawnedFloor[0] = AbstractFloor.InitFloor(SpawnFloor(RandomFloor), startPosition);
            SpawnNextFloor(RandomFloor);
            SpawnNextFloor(RandomFloor);
            PrevFloor = _spawnedFloor[0];
            CurrentFloor = _spawnedFloor[0];
            NextFloor = _spawnedFloor[0];
        }
        public AbstractFloor SpawnNextFloor(GameObject floorPrefab, bool isExtra = false, bool isJump = false)
        {
            int index = NextLastIndex(lastIndex);
            if (_spawnedFloor[index] != null)
            {
                _spawnedFloor[index].DeleteFloor();
            }
            _spawnedFloor[index] = _spawnedFloor[lastIndex].GetNextTile(SpawnFloor(floorPrefab), Direction, isJump ? SessionData.GetTurnWidth(distanceJump) : 0f);
            if (isExtra)
                offsetForExtraAdd++;
            lastIndex = index;
            return _spawnedFloor[lastIndex];
        }
        public void SpawnRandomFloor()
        {
            SpawnNextFloor(RandomFloor);
        }
        public void ReplaceNextFloor()
        {
            NextFloor.ReplaceFloorBySize(SpawnFloor(planeForReplacement));
        }
        public void MoveNext()
        {
            currentindex = NextLastIndex(currentindex);
            PrevFloor = CurrentFloor;
            CurrentFloor = NextFloor;
            NextFloor = _spawnedFloor[currentindex];
        }
        public int NextLastIndex(int index) => (index + 1) % maxSavedTiles;
        public void TurnDirection() => Direction = new Vector3(Direction.z, Direction.y, Direction.x);
        private void ClearSpawnedPanel()
        {
            for (int i = 0; i < maxSavedTiles; i++)
                if (_spawnedFloor[i] != null)
                {
                    _spawnedFloor[i].DeleteFloor();
                    _spawnedFloor[i] = null;
                }
        }
        private Transform SpawnFloor(GameObject floorPrefab)
        {
            Transform floorTransform = GameObject.Instantiate(floorPrefab).transform;
            floorTransform.rotation = Quaternion.Euler(0, 90 * Direction.x, 0);
            return floorTransform;
        }
    }
}
