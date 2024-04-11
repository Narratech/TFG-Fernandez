using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace LiquidSnake.Utils
{
    public class HeatMapRenderer : MonoBehaviour
    {
        public string csvFilePath;
        [Range(0.1f, 4f)]
        public float regionSize = 1.0f;
        [Range(10, 10000)]
        public int maxVisitCount = 1;
        public Gradient gradient;

        private Dictionary<Vector3, CubeData> cubesData = new Dictionary<Vector3, CubeData>();

        [System.Serializable]
        private class CubeData
        {
            public Vector3 center;
            public int visitCount;
            public Color color;
        }

        private void OnValidate()
        {
            cubesData.Clear();

            if (string.IsNullOrEmpty(csvFilePath))
            {
                Debug.LogWarning("CSV file path is not set.");
                return;
            }

            LoadDataFromCSV();
        }

        private void OnDrawGizmos()
        {
            foreach (var cubeData in cubesData.Values)
            {
                Gizmos.color = cubeData.color;
                Gizmos.DrawCube(cubeData.center, Vector3.one * regionSize);
            }
        }

        private void LoadDataFromCSV()
        {
            if (!File.Exists(csvFilePath))
            {
                Debug.LogError("CSV file not found: " + csvFilePath);
                cubesData.Clear();
                return;
            }

            string[] lines = File.ReadAllLines(csvFilePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] values = line.Split(',');

                if (values.Length >= 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);

                    Vector3 point = new Vector3(x, y, z);

                    UpdateCubeData(point);
                }
            }
        }

        private void UpdateCubeData(Vector3 point)
        {
            Vector3 roundedPosition = RoundToNearestMultiple(point, regionSize);
            bool cubeFound = false;

            foreach (var cubeData in cubesData.Values)
            {
                float distance = Vector3.Distance(roundedPosition, cubeData.center);

                if (distance <= regionSize)
                {
                    cubeData.visitCount++;
                    cubeData.visitCount = Mathf.Clamp(cubeData.visitCount, 1, maxVisitCount);

                    UpdateCubeColor(cubeData);
                    cubeFound = true;
                    break;
                }
            }

            if (!cubeFound)
            {
                CubeData newCube = new CubeData
                {
                    center = roundedPosition,
                    visitCount = 1
                };

                UpdateCubeColor(newCube);
                cubesData.Add(roundedPosition, newCube);
            }
        }

        private void UpdateCubeColor(CubeData cubeData)
        {
            float normalizedVisitCount = Mathf.InverseLerp(1, maxVisitCount, Mathf.Clamp(cubeData.visitCount, 1, maxVisitCount));
            cubeData.color = gradient.Evaluate(normalizedVisitCount);
        }

        private Vector3 RoundToNearestMultiple(Vector3 input, float multiple)
        {
            float x = Mathf.Floor(input.x / multiple) * multiple;
            float y = Mathf.Floor(input.y / multiple) * multiple;
            float z = Mathf.Floor(input.z / multiple) * multiple;

            return new Vector3(x, y, z);
        }
    }

} // namespace LiquidSnake.Utils
