using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using WalkCounterClient.Models;
using WalkCounterClient.Services;

namespace WalkCounterClient.ViewModels
{
    public class WalksViewModel:INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private string _info;
        private float _done;
        private string _distance;
        private bool _isBusy;
        private const string Token= "AstraCS:vXPdmZCpqkWqvmXfkUaRnQRQ:d8103d0ce5bce3bb4996a7a24eabf28c6eec4b7a0e3a235f50477232d8bca860";

        public WalksViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task LoadWalks()
        {
            var response = await _apiService.GetYearWalks(Year, Token);
            Done = (float)Math.Round(response.Data.Sum(f => double.Parse(f.Distance)),2);
        }

        public int Year => DateTime.Now.Year;

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public float Done
        {
            get => _done;
            set
            {
                SetProperty( ref _done, value);
                OnPropertyChanged(nameof(ToGo));
            }
        }

        public string Distance
        {
            get => _distance;
            set => SetProperty(ref _distance, value);
        }

        public ICommand SaveCommand => new AsyncCommand( SaveWalk);

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private async Task SaveWalk()
        {
            if (IsBusy)
            {
                return;
            }
            if (!double.TryParse(Distance, out var distance))
            {
                Info = "Check distance!!";
                return;
            }
            var now = DateTime.Now;
            var walk = new Walk
            {
                Year = now.Year.ToString(),
                Id = Guid.NewGuid().ToString(),
                Description = "",
                Distance = distance.ToString(),
                Date = $"{now.Year}-{now.Month}-{now.Day}"
            };
            try
            {
                IsBusy = true;
                await _apiService.SaveWalk(walk, Token);
            }
            catch (Exception e)
            {
                Info = e.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public float ToGo =>(float)Math.Round(2022-_done);


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(backingStore, value))
                return;
            backingStore = value;
            OnPropertyChanged(propertyName);
        }
    }
}
