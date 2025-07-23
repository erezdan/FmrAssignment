using CommunityToolkit.Mvvm.ComponentModel;
using FmrModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using FmrClient.ViewModels;
using FmrModels.Models;
using System.Net.Http;
using FmrAssignment.ViewModels;

namespace FmrClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly CancellationTokenSource _cts = new();
    private Task? _backgroundTask;
    private DateTime _lastUpdateTime = DateTime.Now;

    public ObservableCollection<ShareViewModel> Shares { get; } = new();

    public MainViewModel()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5153/")
        };

        LoadInitialDataAsync();

        _backgroundTask = Task.Run(() => RunBackgroundUpdateLoopAsync(_cts.Token));
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

    private async Task RunBackgroundUpdateLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RefreshUpdatedSharesAsync();
                await Task.Delay(3000, cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Expected when token is cancelled
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
            string url = $"api/shares/updates?since={_lastUpdateTime.ToUniversalTime():yyyy-MM-ddTHH:mm:ss.fffffffZ}";
            var updates = await _httpClient.GetFromJsonAsync<List<Share>>(url);
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
        finally
        {
            // Ensure the last update time is set even if no updates were received
            _lastUpdateTime = DateTime.Now;
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        try
        {
            _backgroundTask?.Wait();
        }
        catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
        {
            // Ignored
        }
        _cts.Dispose();
    }
}
