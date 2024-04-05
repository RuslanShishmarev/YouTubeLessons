using Prism.Commands;
using Prism.Mvvm;

using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
    public class CellBtnVM : BindableBase
    {
		private CellStatus _status = CellStatus.Empty;
		public CellStatus Status
		{
			get => _status;

            set 
			{ 
				_status = value;
				RaisePropertyChanged(nameof(Status));
			}
		}

		public DelegateCommand<object> SetStatusCommand { get; }

		private Action<CellBtnVM> _updateAppStatus;

        public int Row { get; }
        public int Column { get; }

        public CellBtnVM(int row, int column, Action<CellBtnVM> updateAppStatus)
        {
			Row = row;
			Column = column;
			_updateAppStatus = updateAppStatus;
			SetStatusCommand = new DelegateCommand<object>(SetStatus);
        }


        private void SetStatus(object status)
		{

			if (this.Status != CellStatus.Empty) 
				return;
			Status = (CellStatus)status;
			_updateAppStatus?.Invoke(this);
        }
	}
}
