using GameSnake.Models;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GameSnake.ViewModels
{
	internal class MainVM : BindableBase
    {
		private bool _continueGame = false;
		public bool ContinueGame
        {
			get => _continueGame;
			set 
			{ 
				_continueGame = value; 
				RaisePropertyChanged(nameof(ContinueGame));
				if (_continueGame)
				{
					SnakeGo();
                }
			}
		}

        private double _cellD = 50;
		public double CellD
		{
			get => _cellD;
			set
			{
				_cellD = value;
				RaisePropertyChanged(nameof(CellD));
			}
		}

        public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

		public DelegateCommand StartOrStopCommand { get; }

		private SnakeVM _snake;
		private CellVM _lastFood;
		private MoveSnakeDirection _currentDirection = MoveSnakeDirection.Right;
		private MainWindow _mainWnd;

        private const int SPEED_START = 500;

        private int _rowCount = 20;
        private int _columnCount = 20;
		private int _speed = 0;
        public MainVM(MainWindow mainWnd)
		{
			_speed = SPEED_START;
            _mainWnd = mainWnd;
			StartOrStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);			

            for (int row = 0; row <= _rowCount; row++)
			{
				var rowCells = new List<CellVM>();
				for (int column = 0; column <= _columnCount; column++)
				{
					var newCell = new CellVM(row, column, Models.CellType.None);
					rowCells.Add(newCell);
				}
				AllCells.Add(rowCells);
            }

            _snake = new SnakeVM(AllCells[_rowCount / 2][_columnCount / 2], AllCells, CreateRandomFood);
			CreateRandomFood();
            _mainWnd.KeyDown += KeyClick;

			_mainWnd.Loaded += (s, e) => UpdateCell();
            _mainWnd.SizeChanged += (s, e) => UpdateCell();
        }

		private void UpdateCell()
		{
            if (_mainWnd.IsLoaded)
                CellD = (_mainWnd.Width - 150) / _columnCount;
        }

        private void KeyClick(object sender, KeyEventArgs e)
        {
            switch (e.Key)
			{
				case Key.D:
					if (_currentDirection != MoveSnakeDirection.Left)
						_currentDirection = MoveSnakeDirection.Right;
					break;
                case Key.A:
                    if (_currentDirection != MoveSnakeDirection.Right)
                        _currentDirection = MoveSnakeDirection.Left;
                    break;
                case Key.W:
                    if (_currentDirection != MoveSnakeDirection.Down)
                        _currentDirection = MoveSnakeDirection.Up;
                    break;
                case Key.S:
                    if (_currentDirection != MoveSnakeDirection.Up)
                        _currentDirection = MoveSnakeDirection.Down;
                    break;

                default:
					break;
			}
		}

        private async Task SnakeGo()
		{
			while (ContinueGame)
			{
				await Task.Delay(_speed);
				try
				{
					_snake.Move(_currentDirection);
				}
				catch
				{
                    ContinueGame = false;
					_lastFood.CellType = CellType.None;
                    MessageBox.Show("Game over");
                    _snake.Restart();
					_speed = SPEED_START;
					CreateRandomFood();
                }
            }
		}

		private void CreateRandomFood()
		{
			var rn = new Random();
			int foodRow = rn.Next(_rowCount);
            int foodColumn = rn.Next(_columnCount);

			_lastFood = AllCells[foodRow][foodColumn];

			if (_snake.SnakeCells.Contains(_lastFood))
			{
				CreateRandomFood();
            }

            _lastFood.CellType = CellType.Food;
			_speed = (int)(_speed * 0.95);
        }
	}
}
