using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceScraper.Scrape;
using practicescrape.ViewModels;

namespace FinanceScraper.Controllers
{
    public class ScrapeController : Controller
    {
            // GET: Scrape
            public IActionResult Stocks()
            {
                Scraping yahoo = new Scraping();
                yahoo.Scrapington();

                var stockList = yahoo.myList;

                var viewModel = new userStockListViewModel()
                {
                    userStocks = stockList
                };

                return View(viewModel);
            }

            public IActionResult Stockinitial()
            {
                return View();
            }

        }
    }
 