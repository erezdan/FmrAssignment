using CommunityToolkit.Mvvm.ComponentModel;
using FmrModels.Models;

namespace FmrAssignment.ViewModels;

public partial class ShareViewModel : ObservableObject
{
    public Share Model { get; }

    public ShareViewModel(Share model)
    {
        Model = model;

        // Initialize properties from the model
        BidPrice = model.BidPrice;
        BidQuantity = model.BidQuantity;
        AskPrice = model.AskPrice;
        AskQuantity = model.AskQuantity;
        LastPrice = model.LastPrice;
        UpdateTime = model.UpdateTime;
    }

    // Static model properties
    public int Id => Model.Id;
    public string Name => Model.Name;
    public double BasePrice => Model.BasePrice;

    // Calculated fields
    public double TotalBid => BidPrice * BidQuantity;
    public double TotalAsk => AskPrice * AskQuantity;

    public double PercentageChange =>
        BasePrice != 0 && LastPrice != 0
            ? ((LastPrice - BasePrice) / BasePrice) * 100
            : 0;

    // Dynamic properties bound to UI and updated from server
    [ObservableProperty]
    private double bidPrice;

    [ObservableProperty]
    private int bidQuantity;

    [ObservableProperty]
    private double askPrice;

    [ObservableProperty]
    private int askQuantity;

    [ObservableProperty]
    private double lastPrice;

    [ObservableProperty]
    private DateTime updateTime;

    // Notify UI when PercentageChange should be recalculated
    partial void OnLastPriceChanged(double oldValue, double newValue)
    {
        OnPropertyChanged(nameof(PercentageChange));
    }

    // Notify UI when TotalBid should be recalculated
    partial void OnBidPriceChanged(double oldValue, double newValue)
    {
        OnPropertyChanged(nameof(TotalBid));
    }

    partial void OnBidQuantityChanged(int oldValue, int newValue)
    {
        OnPropertyChanged(nameof(TotalBid));
    }

    // Notify UI when TotalAsk should be recalculated
    partial void OnAskPriceChanged(double oldValue, double newValue)
    {
        OnPropertyChanged(nameof(TotalAsk));
    }

    partial void OnAskQuantityChanged(int oldValue, int newValue)
    {
        OnPropertyChanged(nameof(TotalAsk));
    }
}
