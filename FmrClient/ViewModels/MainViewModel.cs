using CommunityToolkit.Mvvm.ComponentModel;
using FmrModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using FmrClient.ViewModels;
using FmrAssignment.ViewModels;
using FmrAssignment;
using FmrModels.Models;
using System.Net.Http;

namespace FmrClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly System.Timers.Timer _timer;

    public ObservableCollection<ShareViewModel> Shares { get; } = new();

    public MainViewModel()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5153/")
        };

        LoadInitialDataAsync();

        _timer = new System.Timers.Timer(3000);
        _timer.Elapsed += async (_, _) => await RefreshUpdatedSharesAsync();
        _timer.Start();
    }

    private async void LoadInitialDataAsync()
    {
        try
        {
            var shares = await _httpClient.GetFromJsonAsync<List<Share>>("api/shares");
            if (shares != null)
            {
                foreach (var share in shares)
                    Shares.Add(new ShareViewModel(share));
            }
        }
        catch (Exception ex)
        {
            // TODO: Handle error (logging / UI alert)
        }
    }

    private async Task RefreshUpdatedSharesAsync()
    {
        try
        {
            var updates = await _httpClient.GetFromJsonAsync<List<Share>>("api/shares/updates");
            if (updates == null) return;

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var updated in updates)
                {
                    var local = Shares.FirstOrDefault(s => s.Id == updated.Id);
                    if (local != null)
                    {
                        local.BidPrice = updated.BidPrice;
                        local.BidQuantity = updated.BidQuantity;
                        local.AskPrice = updated.AskPrice;
                        local.AskQuantity = updated.AskQuantity;
                        local.LastPrice = updated.LastPrice;
                        local.UpdateTime = updated.UpdateTime;
                    }
                }
            });
        }
        catch (Exception ex)
        {
            // TODO: Handle error (logging / UI alert)
        }
    }
}
