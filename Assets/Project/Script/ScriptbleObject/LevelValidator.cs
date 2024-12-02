using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "NewLevelValidator", menuName = "Grower/Level Validator ")]
    public class LevelValidator : ScriptableObject
    {
        [SerializeField] protected int needCellToFill = 9;

        public bool LevelComplete(int fillCell)
        {
            return fillCell >= needCellToFill;
        }
    }
}
