using Windows.UI.Xaml.Controls;
using Refit;
using WalkCounterClient.Services;
using WalkCounterClient.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WalkCounterClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WalksViewModel? _viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            var apiClient = RestService.For<IApiService>("https://8db4e63a-527f-4d2d-921c-f869166288c8-europe-west1.apps.astra.datastax.com/api/rest/v2/keyspaces/walks");
            ViewModel = new WalksViewModel(apiClient);
            ViewModel.LoadWalks();
        }

        public WalksViewModel? ViewModel
        {
            get => _viewModel;
            private set
            {
                _viewModel = value;
                DataContext = _viewModel;
            }
        }
    }
}
