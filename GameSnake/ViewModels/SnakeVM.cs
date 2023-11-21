using GameSnake.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSnake.ViewModels
{
    internal class SnakeVM
    {
        public Queue<CellVM> SnakeCells { get; } = new Queue<CellVM>();

        public List<List<CellVM>> Cells { get; }

        private CellVM _start;
        private Action _createFood;

        public SnakeVM(CellVM start, List<List<CellVM>> cells, Action createFood)
        {
            _start = start;
            _start.CellType = CellType.Snake;
            Cells = cells;
            _createFood = createFood;
            SnakeCells.Enqueue(_start);
        }

        public void Move(MoveSnakeDirection direction)
        {
            var leaderCell = SnakeCells.Last();

            var nextPositionRow = leaderCell.Row;
            var nextPositionColumn = leaderCell.Column;

            switch (direction)
            {
                case MoveSnakeDirection.Left:
                    nextPositionColumn--;
                    break;
                case MoveSnakeDirection.Right:
                    nextPositionColumn++;
                    break;
                case MoveSnakeDirection.Up:
                    nextPositionRow--;
                    break;
                case MoveSnakeDirection.Down:
                    nextPositionRow++;
                    break;
                default:
                    break;
            }

            var nextCell = Cells[nextPositionRow][nextPositionColumn];

            switch (nextCell?.CellType)
            {
                case CellType.None:
                    nextCell.CellType = CellType.Snake;
                    SnakeCells.Dequeue().CellType = CellType.None;
                    SnakeCells.Enqueue(nextCell);
                    break;

                case CellType.Food:
                    nextCell.CellType = CellType.Snake;
                    SnakeCells.Enqueue(nextCell);
                    _createFood?.Invoke();
                    break;

                default:
                    throw new Exception("Game over");
            }
        }

        public void Restart()
        {
            foreach (var snake in SnakeCells)
            {
                snake.CellType = CellType.None;
            }

            SnakeCells.Clear();
            _start.CellType = CellType.Snake;
            SnakeCells.Enqueue(_start);
        }
    }
}
