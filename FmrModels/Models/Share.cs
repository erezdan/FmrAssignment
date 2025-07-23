namespace FmrModels.Models;
public class Share
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double BasePrice { get; set; }

    public double BidPrice { get; set; }
    public int BidQuantity { get; set; }
    public double AskPrice { get; set; }
    public int AskQuantity { get; set; }
    public double LastPrice { get; set; }
    public DateTime UpdateTime { get; set; }
}
