using FmrModels;
using FmrModels.Models;

namespace FmrServer.Services;

public class ShareService
{
    private readonly List<Share> _shares = new();
    private readonly List<int> _updatedShareIds = new();
    private readonly System.Timers.Timer _timer;
    private readonly Random _random = new();

    public ShareService()
    {
        for (int i = 1001; i <= 1010; i++)
        {
            double randPrice = _random.Next(100, 200);
            _shares.Add(new Share
            {
                Id = i,
                Symbol = $"SYM{i}",
                Name = $"נייר {i}",
                BasePrice = randPrice,
                LastPrice = randPrice,
                BidPrice = 0,
                AskPrice = 0,
                BidQuantity = 0,
                AskQuantity = 0,
                UpdateTime = DateTime.Now
            });
        }

        _timer = new System.Timers.Timer(3000);
        _timer.Elapsed += (_, _) => UpdateRandomShares();
        _timer.Start();
    }

    private void UpdateRandomShares()
    {
        lock (_shares)
        {
            _updatedShareIds.Clear();

            int count = _random.Next(1, 6);
            var toUpdate = _shares.OrderBy(_ => _random.Next()).Take(count);

            foreach (var share in toUpdate)
            {
                // Initialize last price if needed
                if (share.LastPrice == 0)
                    share.LastPrice = share.BasePrice * (0.98 + _random.NextDouble() * 0.04); // Start near base

                // Apply small fluctuation (+/-2%)
                double fluctuation = 1 + (_random.NextDouble() * 0.04 - 0.02);
                share.LastPrice *= fluctuation;
                share.LastPrice = Math.Round(Math.Max(0.01, share.LastPrice), 2);

                // Ask price = LastPrice + small spread
                share.AskPrice = Math.Round(share.LastPrice * (1 + _random.NextDouble() * 0.01), 2);

                // Bid price = LastPrice - small spread
                share.BidPrice = Math.Round(share.LastPrice * (1 - _random.NextDouble() * 0.01), 2);

                // Quantities
                share.AskQuantity = _random.Next(10, 500);
                share.BidQuantity = _random.Next(10, 500);

                // Update time
                share.UpdateTime = DateTime.Now;

                _updatedShareIds.Add(share.Id);
            }
        }
    }

    public List<Share> GetAllShares()
    {
        lock (_shares)
        {
            return _shares.Select(s => CloneShare(s)).ToList();
        }
    }

    public List<Share> GetUpdatedShares()
    {
        lock (_shares)
        {
            return _shares
                .Where(s => _updatedShareIds.Contains(s.Id))
                .Select(s => CloneShare(s))
                .ToList();
        }
    }

    private Share CloneShare(Share s) => new()
    {
        Id = s.Id,
        Symbol = s.Symbol,
        Name = s.Name,
        BasePrice = s.BasePrice,
        BidPrice = s.BidPrice,
        BidQuantity = s.BidQuantity,
        AskPrice = s.AskPrice,
        AskQuantity = s.AskQuantity,
        LastPrice = s.LastPrice,
        UpdateTime = s.UpdateTime
    };
}
