using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace MyHTMLParsing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string html = "<div class=\"item\" rating = \"3\" data-pdid=\"5426\"><figure><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/WCM7000WD/Electrolux-700L-\r\nChest-Freezer\"><img alt=\"Electrolux 700L Chest Freezer &amp; Filter\"\r\n\r\nsrc=\"/productimages/thumb/1/5426_5731_4009.jpg\" data-alternate-\r\nimage=\"/productimages/thumb/2/5426_5731_4010.jpg\" class=\"mouseover-\r\nset\"><span class=\"overlay top-horizontal\"><span class=\"sold-out\"><img\r\n\r\nalt=\"Sold Out\"\r\nSrc=\"/Images/Overlay/overlay_1_2_1.png\"></span></span></a></figure><div\r\nclass=\"item-detail\"><h4><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/WCM7000WD/Electrolux-700L-\r\nChest-Freezer\">Electrolux 700L Chest Freezer</a></h4><div class=\"pricing\"\r\n\r\nitemprop=\"offers\" itemscope=\"itemscope\"\r\nitemtype=\"http://schema.org/Offer\"><meta itemprop=\"priceCurrency\"\r\ncontent=\"NZD\"><p class=\"price\"><span class=\"price-display formatted\"\r\nitemprop=\"price\"><span style=\"display: none\">$2,099.00</span>$<span\r\nclass=\"dollars over500\">2,099</span><span class=\"cents\r\nzero\">.00</span></span></p></div><p class=\"style-number\">WCM7000WD</p><p\r\nclass=\"offer\"><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/WCM7000WD/Electrolux-700L-\r\nChest-Freezer\"><span style=\"color:#CC0000;\">WCM7000WD</span></a></p><div\r\n\r\nclass=\"item-asset\"><!--.--></div></div></div>\r\n<div class=\"item\" rating = \"3.6\" data-pdid=\"5862\"><figure><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/E203S/Electrolux-Anti-Odour-\r\nVacuum-Bags\"><img alt=\"Electrolux Anti-Odour Vacuum Bags\"\r\n\r\nsrc=\"/productimages/thumb/1/5862_6182_4541.jpg\"></a></figure><div\r\nclass=\"item-detail\"><h4><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/E203S/Electrolux-Anti-Odour-\r\nVacuum-Bags\">Electrolux Anti-Odour Vacuum Bags</a></h4><div\r\n\r\nclass=\"pricing\" itemprop=\"offers\" itemscope=\"itemscope\"\r\nitemtype=\"http://schema.org/Offer\"><meta itemprop=\"priceCurrency\"\r\ncontent=\"NZD\"><p class=\"price\"><span class=\"price-display formatted\"\r\nitemprop=\"price\"><span style=\"display: none\">$22.99</span>$<span\r\nclass=\"dollars\">22</span><span class=\"cents\">.99</span></span></p></div><p\r\nclass=\"style-number\">E203S</p><p class=\"offer\"><a\r\nhref=\"https://www.100percent.co.nz/Product/E203S/Electrolux-Anti-Odour-\r\n\r\nVacuum-Bags\"><span style=\"color:#CC0000;\">E203S</span></a></p><div\r\nclass=\"item-asset\"><!--.--></div></div></div>\r\n<div class=\"item\" rating = \"8.4\" data-pdid=\"4599\"><figure><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/USK11ANZ/Electrolux-UltraFlex-\r\nStarter-Kit\"><img alt=\"Electrolux UltraFlex Starter &#91; Kit &#93; \"\r\n\r\nsrc=\"/productimages/thumb/1/4599_4843_2928.jpg\"></a></figure><div\r\nclass=\"item-detail\"><h4><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/USK11ANZ/Electrolux-UltraFlex-\r\nStarter-Kit\">Electrolux UltraFlex &#64; Starter Kit</a></h4><div\r\n\r\nclass=\"pricing\" itemprop=\"offers\" itemscope=\"itemscope\"\r\nitemtype=\"http://schema.org/Offer\"><meta itemprop=\"priceCurrency\"\r\ncontent=\"NZD\"><p class=\"price\"><span class=\"price-display formatted\"\r\nitemprop=\"price\"><span style=\"display: none\">$44.99</span>$<span\r\nclass=\"dollars\">44</span><span class=\"cents\">.99</span></span></p></div><p\r\nclass=\"style-number\">USK11ANZ</p><p class=\"offer\"><a\r\n\r\nhref=\"https://www.100percent.co.nz/Product/USK11ANZ/Electrolux-UltraFlex-\r\nStarter-Kit\"><span style=\"color:#CC0000;\">USK11ANZ</span></a></p><div\r\n\r\nclass=\"item-asset\"><!--.--></div></div></div>";
            //version 1
            var list = new List<Dictionary<string, string>>();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var components = doc.DocumentNode.SelectNodes("//div[@class='item']");

            foreach (var component in components)
            {
                var dictionary = new Dictionary<string, string>();

                var products = WebUtility.HtmlDecode(component.SelectSingleNode(".//h4/a").InnerText.Trim());
                var priceNode = component.SelectSingleNode(".//span[@class='price-display formatted']");
                var priceText = priceNode.SelectSingleNode(".//span[@style='display: none']").InnerText.TrimStart('$');
                var price = decimal.Parse(priceText, NumberStyles.Currency);
                var rating = decimal.Parse(component.GetAttributeValue("rating", string.Empty));
                if (rating > 5)
                {
                    rating /= 2;
                }

                dictionary.Add("Product", products);
                dictionary.Add("Price", price.ToString());
                dictionary.Add("Rating", rating.ToString());

                list.Add(dictionary);
            }

            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("\n\n");

            //version 2
            var list2 = new List<Components>();

            foreach (var component in components)
            {
                var products = WebUtility.HtmlDecode(component.SelectSingleNode(".//h4/a").InnerText.Trim());
                var priceNode = component.SelectSingleNode(".//span[@class='price-display formatted']");
                var priceText = priceNode.SelectSingleNode(".//span[@style='display: none']").InnerText.TrimStart('$');
                var price = decimal.Parse(priceText, NumberStyles.Currency);
                var rating = decimal.Parse(component.GetAttributeValue("rating", string.Empty));
                if (rating > 5)
                {
                    rating /= 2;
                }

                var ComponentDetails = new Components
                {
                    Product = products,
                    Price = price,
                    Rating = rating
                };

                list2.Add(ComponentDetails);
            }

            string json2 = JsonConvert.SerializeObject(list2, Formatting.Indented);
            Console.WriteLine(json2);

        }

        public class Components
        {
            public string Product { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public decimal Rating { get; set; }
        }
    }
}
