using System;
using System.ComponentModel;
using System.Globalization;
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
        private const string Token= "AstraCS:FhGHkLrdZRxdfoQnfoHZIezl:777454f8cb0709d68159933e7d74b41b928f7d479e841212eec0cfd78aae15f1";

        public WalksViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task LoadWalks()
        {
            try
            {
                var response = await _apiService.GetYearWalks(Year, Token);
                Done = (float)Math.Round(response.Data.Sum(f => double.Parse(f.Distance, NumberStyles.Any, CultureInfo.InvariantCulture)),2);
            }
            catch (Exception e)
            {
                Info = e.Message;
            }
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
                OnPropertyChanged(nameof(Progress));
            }
        }

        public float ToGo =>(float)Math.Round(2022-_done,2);

        public float Progress => Done / Year;

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

            var distanceString = Distance.Replace(",", ".");
            if (!double.TryParse(distanceString, NumberStyles.Float, CultureInfo.InvariantCulture, out var distance))
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
                Distance = distance.ToString(CultureInfo.InvariantCulture),
                Date = $"{now.Year}-{now.Month}-{now.Day}"
            };
            try
            {
                IsBusy = true;
                await _apiService.SaveWalk(walk, Token);
                Distance = "0";
                await LoadWalks();
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
