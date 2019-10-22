using System;
using System.Collections.Generic;
using System.Globalization;

namespace CurrencyConverterClassLibrary
{
    public class CurrencyConverterClassLibrary
    {
        public Dictionary<string, Product> ParseProductsCSV(string csv)
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>();
            var splittedFileContent = SplitCSV(csv, 3);

            foreach (var line in splittedFileContent)
            {
                var splittedLine = line.Split(',');
                var product = new Product()
                {
                    Description = splittedLine[0],
                    Currency = splittedLine[1],
                    Price = decimal.Parse(splittedLine[2], CultureInfo.InvariantCulture)
                };
                products.Add(product.Description, product);
            }

            return products;

        }

        public Dictionary<string, ExchangeRate> ParseExchangeRatesCSV(string csv)
        {
            Dictionary<string, ExchangeRate> exchangeRates = new Dictionary<string, ExchangeRate>();
            var splittedFileContent = SplitCSV(csv, 2);

            foreach (var line in splittedFileContent)
            {
                var splittedLine = line.Split(',');

                var exchangeRate = new ExchangeRate()
                {
                    Currency = splittedLine[0],
                    Rate = decimal.Parse(splittedLine[1], CultureInfo.InvariantCulture),
                };
                exchangeRates.Add(exchangeRate.Currency, exchangeRate);
            }

            exchangeRates.Add("EUR", new ExchangeRate() { Currency = "EUR", Rate = 1 });


            return exchangeRates;

        }

        public decimal GetPriceInTargetCurrency(string targetCurrency, Product product, Dictionary<string, ExchangeRate> exchangeRates)
        {
            var originCurrency = product.Currency;
            var euroRate = exchangeRates[originCurrency].Rate;
            var priceInEuro = product.Price / euroRate;
            var targetCurrencyRate = exchangeRates[targetCurrency].Rate;
            var priceInTargetCurrency = priceInEuro * targetCurrencyRate;
            return Math.Round(priceInTargetCurrency, 2);
        }

        private string[] SplitCSV(string csv, int expectedLength)
        {
            var fileContent = csv.Replace("\r", "").Split('\n');
            var helperList = new List<string>(fileContent);
            helperList.RemoveAt(0);
            helperList.RemoveAll(line => line.Split(',').Length != expectedLength);
            fileContent = helperList.ToArray();

            return fileContent;
        }
    }
}
