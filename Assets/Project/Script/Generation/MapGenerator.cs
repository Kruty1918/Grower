using System.Collections.Generic;
using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    public enum GenerationType
    {
        Awake,
        Start,
        Script
    }

    public class MapGenerator : MonoSingleton<MapGenerator>
    {
        [SerializeField] protected GenerationSettings settings;
        public Cell[,] Cells { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            CheckGenerationType(GenerationType.Awake);
        }

        private void Start()
        {
            CheckGenerationType(GenerationType.Start);
        }

        private void CheckGenerationType(GenerationType type)
        {
            if (settings.generationType == type)
            {
                GenerateMap();
            }
        }

        public void GenerateMap()
        {
            if (settings == null || settings.CellsPrefabs == null || settings.validator == null)
            {
                Debug.LogError("Generation settings or validator are not set!");
                return;
            }

            // Validate and filter prefabs
            List<Cell> validPrefabs = settings.validator.ValidateAndSelect(settings.CellsPrefabs);
            if (validPrefabs == null || validPrefabs.Count == 0)
            {
                Debug.LogError("No valid cell prefabs found after validation.");
                return;
            }

            int size = settings.mapSize;
            Cells = new Cell[size, size];

            // Generate the map grid
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Cell selectedPrefab = validPrefabs[Random.Range(0, validPrefabs.Count)];

                    Cell cellInstance = Instantiate(selectedPrefab, new Vector3(x, 0, y), Quaternion.identity);

                    Cells[x, y] = cellInstance;
                }
            }

            Debug.Log("Map generation complete!");
        }

        public void ClearMap()
        {
            if (Cells != null)
            {
                foreach (var cell in Cells)
                {
                    if (cell != null)
                    {
                        Destroy(cell.gameObject);
                    }
                }
            }

            Cells = null;
            Debug.Log("Map cleared.");
        }
    }
}