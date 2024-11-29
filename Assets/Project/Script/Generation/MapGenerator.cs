using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Transform plane;
        [SerializeField, Range(0f, 1f)] private float removalChance = 0.3f; // Ймовірність видалення комірки
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
                ProcessMap(); // Виклик обробки мапи
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

            // Calculate center offset if needed
            Vector2Int centerOffset = settings.centerMap
                ? new Vector2Int(-size / 2, -size / 2)
                : Vector2Int.zero;

            // Generate the map grid
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Cell selectedPrefab = validPrefabs[Random.Range(0, validPrefabs.Count)];

                    // Apply offsets
                    Vector3 position = new Vector3(
                        x + centerOffset.x + settings.offset.x,
                        0,
                        y + centerOffset.y + settings.offset.y
                    );

                    Cell cellInstance = Instantiate(selectedPrefab, position, Quaternion.identity);

                    Cells[x, y] = cellInstance;
                }
            }

            plane.localScale = new Vector3(size, plane.localScale.y, size);
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
        }

        /// <summary>
        /// Обробляє мапу, видаляючи комірки з певною ймовірністю,
        /// та додаючи кілька комірок, сусідніх до випадкових крайових.
        /// </summary>
        public void ProcessMap(int numberOfPaths = 5)
        {
            if (Cells == null)
            {
                Debug.LogWarning("Map is not generated yet.");
                return;
            }

            List<Vector2Int> allPath = new List<Vector2Int>();

            // Початкова точка для першого шляху
            Vector2Int start = GetRandomInnerCell();

            // Генерація шляхів
            for (int i = 0; i < numberOfPaths; i++)
            {
                // Отримуємо новий напрямок для шляху
                Vector2Int? newDirection = GetRandomValidDirection(start);

                // Генерація шляху в обраному напрямку
                List<Vector2Int> newPath = GetPathInDirection(start, newDirection.Value);
                allPath.AddRange(newPath);

                // Оновлюємо стартову точку для наступного шляху
                start = newPath.Last();
            }

            // Видаляємо комірки в усіх шляхах
            RemoveCellsAtCoordinates(allPath);
        }

        /// <summary>
        /// Видаляє клітинки на заданих координатах.
        /// </summary>
        /// <param name="coordinates">Список координат для видалення клітинок.</param>
        public void RemoveCellsAtCoordinates(List<Vector2Int> coordinates)
        {
            foreach (var coord in coordinates)
            {
                if (IsWithinBounds(coord))
                {
                    // Перевіряємо, чи є клітинка на цій позиції
                    if (Cells[coord.x, coord.y] != null)
                    {
                        Destroy(Cells[coord.x, coord.y].gameObject);
                        Cells[coord.x, coord.y] = null;
                    }
                    else
                    {
                        Debug.LogWarning($"No cell found at {coord} to remove.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Coordinate {coord} is out of bounds. Skipping removal.");
                }
            }
        }


        /// <summary>
        /// Спавнить клітинки на заданих координатах.
        /// </summary>
        /// <param name="coordinates">Список координат для спавну клітинок.</param>
        /// <param name="cellPrefab">Префаб клітинки для спавну.</param>
        public void SpawnCellsAtCoordinates(List<Vector2Int> coordinates, Cell cellPrefab)
        {
            if (cellPrefab == null)
            {
                Debug.LogError("Cell prefab is null!");
                return;
            }

            foreach (var coord in coordinates)
            {
                if (!IsWithinBounds(coord))
                {
                    Debug.LogWarning($"Coordinate {coord} is out of bounds, skipping.");
                    continue;
                }

                // Перевіряємо, чи на цій позиції вже є клітинка
                if (Cells[coord.x, coord.y] != null)
                {
                    Debug.LogWarning($"Cell already exists at {coord}, skipping.");
                    continue;
                }

                // Визначаємо позицію для спавну
                Vector3 position = new Vector3(
                    coord.x + settings.offset.x,
                    0,
                    coord.y + settings.offset.y
                );

                // Створюємо клітинку
                Cell cellInstance = Instantiate(cellPrefab, position, Quaternion.identity);
                Cells[coord.x, coord.y] = cellInstance;
            }
        }


        /// <summary>
        /// Рекурсивно рухається в заданому напрямку від стартової координати,
        /// збираючи координати клітинок, поки не наткнеться на крайню клітинку.
        /// </summary>
        /// <param name="startCoord">Стартова координата.</param>
        /// <param name="direction">Напрямок руху.</param>
        /// <param name="path">Список для збору пройдених координат.</param>
        /// <returns>Список координат шляху, виключаючи крайню клітинку.</returns>
        public List<Vector2Int> GetPathInDirection(Vector2Int startCoord, Vector2Int direction, List<Vector2Int> path = null)
        {
            if (path == null)
            {
                path = new List<Vector2Int>();
            }

            // Розрахунок наступної координати
            Vector2Int nextCoord = startCoord + direction;

            // Перевірка, чи наступна координата в межах
            if (!IsWithinBounds(nextCoord))
            {
                Debug.LogWarning("Out of bounds detected, stopping recursion.");
                return path;
            }

            // Якщо наступна клітинка є крайньою, завершити рекурсію
            if (IsOnEdge(nextCoord))
            {
                Debug.Log($"Edge reached at {nextCoord}, stopping recursion.");
                return path;
            }

            // Додати поточну координату до шляху
            path.Add(nextCoord);

            // Рекурсивно продовжити рух
            return GetPathInDirection(nextCoord, direction, path);
        }


        /// <summary>
        /// Повертає випадковий напрямок на основі валідних напрямків від заданої координати.
        /// Напрямок вважається валідним, якщо він веде до внутрішньої клітинки.
        /// </summary>
        /// <param name="currentCoord">Поточна координата.</param>
        /// <returns>Вектор напрямку або null, якщо напрямки недоступні.</returns>
        public Vector2Int? GetRandomValidDirection(Vector2Int currentCoord)
        {
            // Потенційні напрямки: вгору, вниз, вправо, вліво
            List<Vector2Int> directions = new List<Vector2Int>()
            {
        new Vector2Int(0, 1),  // Вгору
        new Vector2Int(0, -1), // Вниз
        new Vector2Int(1, 0),  // Вправо
        new Vector2Int(-1, 0)  // Вліво
            };

            List<Vector2Int> validDirections = new List<Vector2Int>();

            directions.Shuffle();

            // Перевіряємо кожен напрямок
            foreach (var dir in directions)
            {
                Vector2Int neighborCoord = currentCoord + dir;

                // Перевірка чи координата в межах
                if (IsWithinBounds(neighborCoord))
                {
                    // Якщо сусідня клітинка існує та не є крайньою
                    if (Cells[neighborCoord.x, neighborCoord.y] != null && !IsOnEdge(neighborCoord))
                    {
                        validDirections.Add(dir);
                    }
                }
            }

            // Якщо валідних напрямків немає
            if (validDirections.Count == 0)
            {
                Debug.LogWarning($"No valid directions found for coordinate: {currentCoord}");
                return null;
            }

            // Вибір випадкового валідного напрямку
            return validDirections[Random.Range(0, validDirections.Count)];
        }


        /// <summary>
        /// Рекурсивно знаходить випадкову клітинку, яка не є крайньою.
        /// </summary>
        /// <returns>Координати клітинки, яка не є крайньою.</returns>
        public Vector2Int GetRandomInnerCell()
        {
            // Генеруємо випадкові координати, які виключають крайні значення
            int x = Random.Range(1, settings.mapSize - 1);
            int y = Random.Range(1, settings.mapSize - 1);

            Vector2Int randomCoord = new Vector2Int(x, y);

            // Якщо клітинка існує, повертаємо її
            if (Cells[randomCoord.x, randomCoord.y] != null)
            {
                return randomCoord;
            }

            // Інакше, викликаємо метод ще раз
            return GetRandomInnerCell();
        }


        private void DeleteInternalCells()
        {
            // Видалення внутрішніх комірок
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (!IsOnEdge(new Vector2Int(x, y)))
                    {
                        if (Cells[x, y] != null)
                        {
                            Destroy(Cells[x, y].gameObject);
                            Cells[x, y] = null;
                        }
                    }
                }
            }
        }

        private bool IsOnEdge(Vector2Int coord)
        {
            return coord.x == 0 || coord.y == 0 || coord.x == settings.mapSize - 1 || coord.y == settings.mapSize - 1;
        }

        private Vector2Int GetRandomEdgeCoord()
        {
            List<Vector2Int> edgeCoords = new List<Vector2Int>();

            // Додаємо всі координати краю
            for (int x = 0; x < settings.mapSize; x++)
            {
                edgeCoords.Add(new Vector2Int(x, 0)); // Верхній край
                edgeCoords.Add(new Vector2Int(x, settings.mapSize - 1)); // Нижній край
            }

            for (int y = 1; y < settings.mapSize - 1; y++) // Лівий і правий край без кутів
            {
                edgeCoords.Add(new Vector2Int(0, y)); // Лівий край
                edgeCoords.Add(new Vector2Int(settings.mapSize - 1, y)); // Правий край
            }

            // Вибір випадкової координати
            return edgeCoords[Random.Range(0, edgeCoords.Count)];
        }

        private Vector2Int? GetInnerNeighbor(Vector2Int edgeCoord)
        {
            // Потенційні зміщення до сусідів
            List<Vector2Int> directions = new List<Vector2Int>()
    {
        new Vector2Int(0, 1),  // Вгору
        new Vector2Int(0, -1), // Вниз
        new Vector2Int(1, 0),  // Вправо
        new Vector2Int(-1, 0)  // Вліво
    };

            // Розрахунок кількості доступних клітинок у кожному напрямку
            Dictionary<Vector2Int, int> directionWeights = new Dictionary<Vector2Int, int>();
            foreach (var dir in directions)
            {
                Vector2Int neighbor = edgeCoord + dir;
                if (IsWithinBounds(neighbor) && !IsOnEdge(neighbor))
                {
                    int count = CountAvailableCellsInDirection(neighbor, dir);
                    directionWeights[dir] = count;
                }
            }

            // Сортуємо напрями за кількістю доступних клітинок (або за випадковим порядком)
            var sortedDirections = directionWeights.OrderByDescending(x =>
            {
                // Інколи більше клітинок, інколи менше (з невеликою випадковістю)
                return UnityEngine.Random.value > 0.5f ? x.Value : -x.Value;
            }).Select(x => x.Key).ToList();

            // Перевірка напрямків у пріоритезованому порядку
            foreach (var dir in sortedDirections)
            {
                Vector2Int neighbor = edgeCoord + dir;

                if (!IsOnEdge(neighbor) && IsWithinBounds(neighbor))
                {
                    return neighbor;
                }
            }

            // Якщо жодного сусіда не знайдено
            return null;
        }

        // Підрахунок доступних клітинок у заданому напрямку
        private int CountAvailableCellsInDirection(Vector2Int start, Vector2Int direction)
        {
            int count = 0;
            Vector2Int current = start;

            while (IsWithinBounds(current) && !IsOnEdge(current))
            {
                count++;
                current += direction;
            }

            return count;
        }

        private bool IsWithinBounds(Vector2Int coord)
        {
            return coord.x >= 0 && coord.x < settings.mapSize && coord.y >= 0 && coord.y < settings.mapSize;
        }

        public Vector2Int? GetRandomEdgeNeighborToCenter()
        {
            Vector2Int edgeCoord = GetRandomEdgeCoord();

            // Пошук внутрішнього сусіда
            Vector2Int? innerNeighbor = GetInnerNeighbor(edgeCoord);

            if (innerNeighbor.HasValue)
            {
                Debug.Log($"Edge Coord: {edgeCoord}, Inner Neighbor: {innerNeighbor.Value}");
            }
            else
            {
                Debug.LogWarning($"No inner neighbor found for edge coordinate: {edgeCoord}");
            }

            return innerNeighbor;
        }
    }
}


public static class ListExtensions
{
    private static System.Random random = new System.Random();

    /// <summary>
    /// Перемішує елементи списку випадковим чином.
    /// </summary>
    /// <typeparam name="T">Тип елементів у списку.</typeparam>
    /// <param name="list">Список для перемішування.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            // Обмінюємо місцями елементи
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}