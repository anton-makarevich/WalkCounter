using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using WalkCounterClient.Services;
using WalkCounterClient.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WalkCounterXF
{
    public partial class MainPage : ContentPage
    {
        private WalksViewModel _viewModel;
        BackgroundWorker setFocus = new BackgroundWorker();
        public MainPage()
        {
            InitializeComponent();
            setFocus.DoWork += SetFocus_DoWork;
            var apiClient = RestService.For<IApiService>("https://8db4e63a-527f-4d2d-921c-f869166288c8-europe-west1.apps.astra.datastax.com/api/rest/v2/keyspaces/walks");
            ViewModel = new WalksViewModel(apiClient);
            ViewModel.LoadWalks();
        }

        private void SetFocus_DoWork(object sender, DoWorkEventArgs e)
        {
            bool worked = false;
            while (!worked)//will keep trying until it can set focus (when MyEntry is rendered)
            {
                Thread.Sleep(1);
                MainThread.InvokeOnMainThreadAsync(()=> worked = distanceEntry.Focus());
            }
        }

        public WalksViewModel ViewModel
        {
            get => _viewModel;
            private set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!setFocus.IsBusy)
            {
                setFocus.RunWorkerAsync();
            }
        }
    }
}
