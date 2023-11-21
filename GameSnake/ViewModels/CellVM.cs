using GameSnake.Models;
using Prism.Mvvm;

namespace GameSnake.ViewModels
{
    internal class CellVM : BindableBase
    {
        public int Column { get; }

        public int Row { get; }

        private CellType _cellType = CellType.None;
        public CellType CellType 
        {
            get => _cellType;
            set
            {
                _cellType = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column;
            CellType = cellType;
        }
    }
}
