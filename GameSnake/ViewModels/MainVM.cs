using GameSnake.Models;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
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

		public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

		public DelegateCommand StartOrStopCommand { get; }

		private SnakeVM _snake;
		private CellVM _lastFood;
		private MoveSnakeDirection _currentDirection = MoveSnakeDirection.Right;
		private Window _mainWnd;

        private int rowCount = 10;
        private int rowColumn = 10;
		private const int SPEED_START = 500;
		private int _speed = 0;
        public MainVM(Window mainWnd)
		{
			_speed = SPEED_START;
            _mainWnd = mainWnd;
			StartOrStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);			

            for (int row = 0; row <= rowCount; row++)
			{
				var rowCells = new List<CellVM>();
				for (int column = 0; column <= rowColumn; column++)
				{
					var newCell = new CellVM(row, column, Models.CellType.None);
					rowCells.Add(newCell);
				}
				AllCells.Add(rowCells);
            }

            _snake = new SnakeVM(AllCells[rowCount / 2][rowColumn / 2], AllCells, CreateRandomFood);
			CreateRandomFood();
            _mainWnd.KeyDown += KeyClick;
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
			int foodRow = rn.Next(rowCount);
            int foodColumn = rn.Next(rowColumn);

			if (_snake.SnakeCells.Any(x => x.Row == foodRow && x.Column == foodColumn))
			{
				CreateRandomFood();
            }
			_lastFood = AllCells[foodRow][foodColumn];
            _lastFood.CellType = CellType.Food;
			_speed = (int)(_speed * 0.95);
        }
	}
}
