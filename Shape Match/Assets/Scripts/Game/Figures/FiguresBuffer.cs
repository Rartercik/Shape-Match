using System.Linq;
using UnityEngine;
using Game.States;

namespace Game.Figures
{
    public class FiguresBuffer : MonoBehaviour
    {
        [SerializeField] private GameStatesHandler _statesHandler;
        [SerializeField] private FiguresBufferCell[] _cells;

        private int _groupsSize;
        private int _freeCell;

        public FigureContent[] NotDefaultContents => _cells.Where(cell => !cell.IsDefault).Select(cell => cell.FigureContent).ToArray();

        public void Initialize(uint groupsSize)
        {
            foreach (var cell in _cells)
            {
                cell.Initialize();
            }
            _groupsSize = (int)groupsSize;
        }

        public void AddFigure(FigureContent content)
        {
            if (_groupsSize == 1) return;

            _cells[_freeCell].SetContent(content);
            _freeCell++;
            DeleteGroups(_cells, _groupsSize, ref _freeCell);
            OrderCells(_cells);
            if (_freeCell >= _cells.Length)
            {
                _statesHandler.ReportDefeat();
            }
        }

        public void DeleteGroups(FiguresBufferCell[] cells, int groupsSize, ref int freeCell)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].IsDefault) continue;

                var identicalCells = new int[groupsSize - 1];
                var index = 0;
                for (int j = i + 1; j < cells.Length; j++)
                {
                    if (cells[j].IsDefault) continue;

                    if (cells[i].FigureContent.Equals(cells[j].FigureContent) && index < identicalCells.Length)
                    {
                        identicalCells[index] = j;
                        index++;
                    }
                }

                if (index + 1 >= groupsSize)
                {
                    cells[i].SetDefault();
                    freeCell--;
                    foreach (var cell in identicalCells)
                    {
                        cells[cell].SetDefault();
                        freeCell--;
                    }
                }
            }
        }

        public void ResetState()
        {
            foreach (var cell in _cells)
            {
                cell.SetDefault();
            }
            _freeCell = 0;
        }

        private void OrderCells(FiguresBufferCell[] cells)
        {
            for (int i = 1; i < cells.Length; i++)
            {
                if (cells[i].IsDefault) continue;

                for (int j = 0; j < i; j++)
                {
                    if (cells[j].IsDefault)
                    {
                        cells[j].SetContent(cells[i].FigureContent);
                        cells[i].SetDefault();
                        break;
                    }
                }
            }
        }
    }
}
