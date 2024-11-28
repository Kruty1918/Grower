using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(menuName = "Grower/GenerationSettings")]
    public class GenerationSettings : ScriptableObject
    {
        public int mapSize = 10; // Map size NxN
        public GenerationType generationType = GenerationType.Start;
        public GenerationValidatorAbstract validator;

        [SerializeField] private List<Cell> cellPrefabs;
        public List<Cell> CellsPrefabs { get => cellPrefabs; protected set { cellPrefabs = value; } }
    }
}
