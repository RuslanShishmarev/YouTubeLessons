using Prism.Mvvm;

using System.Windows;

using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private CellStatus _currentPlayerStatus;
        public CellStatus CurrentPlayerStatus
        {
            get => _currentPlayerStatus;

            private set
            {
                _currentPlayerStatus = value;
                RaisePropertyChanged(nameof(CurrentPlayerStatus));
            }
        }

        public List<List<CellBtnVM>> AllCells { get; } = new List<List<CellBtnVM>>();

        private int _cellRowColumnCount = 3;
        public MainWindowViewModel()
        {
            CurrentPlayerStatus = CellStatus.Cross;

            for (int row = 0; row < _cellRowColumnCount; row++)
            {
                var newRow = new List<CellBtnVM>();
                for (int column = 0; column < _cellRowColumnCount; column++)
                {
                    var newCell = new CellBtnVM(row, column, CheckGameStatus);
                    newRow.Add(newCell);
                }
                AllCells.Add(newRow);
            }
        }

        private void CheckGameStatus(CellBtnVM lastClickBtn)
        {
            // column check
            var columnCheck = new List<CellBtnVM>();
            for (int i = 0; i <= _cellRowColumnCount / 2; i++)
            {
                int column = lastClickBtn.Column + 1 + i;
                if (column < _cellRowColumnCount)
                {
                    columnCheck.Add(AllCells[lastClickBtn.Row][column]);
                }
                else
                {
                    columnCheck.Add(AllCells[lastClickBtn.Row][column - _cellRowColumnCount]);
                }
            }

            if (columnCheck.All(x => x.Status == lastClickBtn.Status))
            {
                StopGame(lastClickBtn.Status);
                return;
            }

            var rowCheck = new List<CellBtnVM>();
            for (int i = 0; i <= _cellRowColumnCount / 2; i++)
            {
                int row = lastClickBtn.Row + 1 + i;
                if (row < _cellRowColumnCount)
                {
                    rowCheck.Add(AllCells[row][lastClickBtn.Column]);
                }
                else
                {
                    rowCheck.Add(AllCells[row - _cellRowColumnCount][lastClickBtn.Column]);
                }
            }

            if (rowCheck.All(x => x.Status == lastClickBtn.Status))
            {
                StopGame(lastClickBtn.Status);
                return;
            }

            bool checkByDiagonal = true;
            int checkCrossNum = Math.Abs(lastClickBtn.Column - lastClickBtn.Row);

            if (checkCrossNum != 0 && checkCrossNum != _cellRowColumnCount - 1)
            {
                checkByDiagonal = false;
            }

            if (checkByDiagonal && lastClickBtn.Column == 0 && lastClickBtn.Row != _cellRowColumnCount - 1)
            {
                var crossCheck1 = new List<CellBtnVM>();
                for (int i = 0; i <= _cellRowColumnCount / 2; i++)
                {
                    int column = lastClickBtn.Column + 1 + i;
                    int row = lastClickBtn.Row + 1 + i;
                    if (column < _cellRowColumnCount)
                    {
                        crossCheck1.Add(AllCells[row][column]);
                    }
                    else
                    {
                        crossCheck1.Add(AllCells[row - _cellRowColumnCount][column - _cellRowColumnCount]);
                    }
                }

                if (crossCheck1.All(x => x.Status == lastClickBtn.Status))
                {
                    StopGame(lastClickBtn.Status);
                    return;
                }
            }

            if (checkByDiagonal && lastClickBtn.Column != 0 && lastClickBtn.Row != 0)
            {
                var crossCheck2 = new List<CellBtnVM>();
                for (int i = 0; i <= _cellRowColumnCount / 2; i++)
                {
                    int column = lastClickBtn.Column + 1 + i;
                    int row = lastClickBtn.Row - (1 + i);
                    if (row < 0)
                    {
                        row = row + _cellRowColumnCount;
                    }
                    if (column >= _cellRowColumnCount)
                    {
                        column = column - _cellRowColumnCount;
                    }

                    crossCheck2.Add(AllCells[row][column]);
                }

                if (crossCheck2.All(x => x.Status == lastClickBtn.Status))
                {
                    StopGame(lastClickBtn.Status);
                    return;
                }
            }

            CurrentPlayerStatus = lastClickBtn.Status == CellStatus.Cross ? CellStatus.Circle : CellStatus.Cross;
        }

        private void StopGame(CellStatus winnerStatus)
        {
            MessageBox.Show($"Winner: {winnerStatus}");
            foreach (var row in AllCells)
            {
                foreach (var cell in row)
                {
                    cell.Status = CellStatus.Empty;
                }
            }
        }
    }
}
