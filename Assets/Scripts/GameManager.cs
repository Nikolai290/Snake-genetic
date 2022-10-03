using System.Collections.Generic;
using System.Linq;
using Snake;
using UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private UiManager _uiManager;

        private List<GameObject> _foods;
        private List<GameObject> _heads;

        private void Awake()
        {
            _uiManager = this.GetComponentInChildren<UiManager>();
            Time.fixedDeltaTime = gameSettings.FixedDeltaTime;
            _foods = new List<GameObject>();
            _heads = new List<GameObject>();
            GenerateFood();
            GenerateSnakes();
            BuildWalls();
            TransformCamera();
        }

        private void TransformCamera()
        {
            Camera.main.transform.position = new Vector3(
                gameSettings.Width / 2,
                gameSettings.Height / 2,
                Camera.main.transform.position.z);
            var max = gameSettings.Width > gameSettings.Height ? gameSettings.Width : gameSettings.Height;
            Camera.main.orthographicSize = max / 2 + 2;
        }

        private void OnEatingHandler(GameObject food)
        {
            food.transform.position = GenerateFreeVector3();
        }

        /// <summary>
        /// Check collision with any food
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns>False if collisions not found</returns>
        private bool CheckFoodCollision(Vector3 vector3)
        {
            return _foods.Any() && _foods.Any(food => food.transform.position == vector3);
        }

        private void GenerateSnakes()
        {
            for (int i = 0; i < gameSettings.Snakes; i++)
            {
                var head = Instantiate(
                    gameSettings.HeadPrefab,
                    GenerateFreeVector3(),
                    Quaternion.identity);
                var headMoving = head.GetComponent<HeadMoving>();
                headMoving.OnEating += OnEatingHandler;
                headMoving.ManualControl = gameSettings.ManualControl;
                headMoving.StarterLength = gameSettings.StarterLenght;
                if (gameSettings.Snakes == 1)
                {
                    headMoving.OnScoreUp += _uiManager.OnScoreChangeHandler;
                }

                _heads.Add(head);
            }
        }

        private void GenerateFood()
        {
            for (int i = 0; i < gameSettings.Foods; i++)
            {
                var food = Instantiate(
                    gameSettings.FoodPrefab,
                    GenerateFreeVector3(),
                    quaternion.identity);
                _foods.Add(food);
            }
        }

        private Vector3 GenerateFreeVector3()
        {
            Vector3 result;
            do
            {
                result = GenerateRandomVector3Position();
            } while (CheckFoodCollision(result) &&
                     _heads.Any() &&
                     _heads.Any(head => head != null && head.GetComponent<HeadMoving>().CheckCollision(result)));

            return result;
        }

        private Vector3 GenerateRandomVector3Position()
        {
            return new Vector3(
                Random.Range(0, gameSettings.Width),
                Random.Range(0, gameSettings.Height),
                0);
        }

        private void BuildWalls()
        {
            for (int i = -1; i <= gameSettings.Width + 1; i++)
            {
                var position1 = new Vector3(i, -1, 1);
                var position2 = new Vector3(i, gameSettings.Height + 1, 1);
                Instantiate(gameSettings.WallPrefab, position1, Quaternion.identity);
                Instantiate(gameSettings.WallPrefab, position2, Quaternion.identity);
            }

            for (int i = 0; i <= gameSettings.Height; i++)
            {
                var position1 = new Vector3(-1, i, 1);
                var position2 = new Vector3(gameSettings.Width + 1, i, 1);
                Instantiate(gameSettings.WallPrefab, position1, Quaternion.identity);
                Instantiate(gameSettings.WallPrefab, position2, Quaternion.identity); 
            }
        }
    }
}