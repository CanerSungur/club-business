using UnityEngine;

namespace ClubBusiness
{
    public class ColoredBoxController : MonoBehaviour
    {
        [Header("-- SETUP --")]
        [SerializeField] private GameObject coloredBoxPrefab;

        private readonly float _xOffset = 1.045f;
        private readonly float _zOffset = 1.04f;
        private int _coloredBoxCount;
        private int _currentRowCount;
        private int _currentColumnCount;
        private readonly int _rowCount = 9;
        private readonly int _columnCount = 9;

        public void Init(DanceFloor danceFloor)
        {

            _currentRowCount = _currentColumnCount = _coloredBoxCount = 0;
            for (int i = 0; i < 81; i++)
            {
                if (_currentRowCount == _rowCount)
                {
                    _currentColumnCount++;
                    _currentRowCount = 0;
                }
                ColoredBox coloredBox = Instantiate(coloredBoxPrefab).GetComponent<ColoredBox>();
                //coloredBox.transform.SetParent(transform);
                coloredBox.Init(this);
                coloredBox.transform.localPosition = new Vector3(_currentRowCount * _xOffset, 0f, _currentColumnCount * -_zOffset);
                _currentRowCount++;
            }
        }
    }
}
