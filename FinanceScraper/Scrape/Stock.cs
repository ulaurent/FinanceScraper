using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceScraper.Scrape
{
    public class Stock
    {
        public string StockSymbol;
        public string LastPrice;
        public string MarketTime;
        public string PriceChange;
        public string PercentChange;

        public Stock()
        {
        }
    }
}
