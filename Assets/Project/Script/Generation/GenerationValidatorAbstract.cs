using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    public abstract class GenerationValidatorAbstract : ScriptableObject
    {
        public abstract List<Cell> ValidateAndSelect(List<Cell> cellPrefabs);
    }
}
