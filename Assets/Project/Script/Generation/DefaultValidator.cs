using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(menuName = "Grower/Validators/DefaultValidator")]
    public class DefaultValidator : GenerationValidatorAbstract
    {
        public override List<Cell> ValidateAndSelect(List<Cell> cellPrefabs)
        {
            // Filter out invalid prefabs (e.g., those missing required properties)
            List<Cell> validPrefabs = new List<Cell>();
            foreach (var prefab in cellPrefabs)
            {
                if (prefab != null && prefab.CellType != CellType.Body)
                {
                    validPrefabs.Add(prefab);
                }
            }

            return validPrefabs;
        }
    }
}
