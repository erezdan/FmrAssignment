using CommunityToolkit.Mvvm.ComponentModel;
using FmrModels.Models;

namespace FmrAssignment.ViewModels;

public partial class ShareViewModel : ObservableObject
{
    public Share Model { get; }

    public ShareViewModel(Share model)
    {
        Model = model;
    }

    public int Id => Model.Id;
    public string Name => Model.Name;
    public double BasePrice => Model.BasePrice;

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

    public double PercentageChange =>
        BasePrice != 0 && LastPrice != 0 ? ((LastPrice - BasePrice) / BasePrice) * 100 : 0;

    // 🔄 This method is called automatically when LastPrice changes
    partial void OnLastPriceChanged(double value)
    {
        OnPropertyChanged(nameof(PercentageChange));
    }
}
