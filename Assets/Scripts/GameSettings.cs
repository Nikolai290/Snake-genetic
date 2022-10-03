using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Snake/Game settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public float FixedDeltaTime = 0.5f;
        public int Foods = 3;
        public int Height = 10;
        public int Width = 15;
        public int Snakes = 1;
        public int StarterLenght = 2;

        public GameObject FoodPrefab;
        public GameObject HeadPrefab;
        public GameObject WallPrefab;

        public bool ManualControl = true;
    }
}