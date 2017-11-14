using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CJProductCatalog.Controllers
{
    public class CJProductSearchCatalogController : Controller
    {

        private const string _CJWebsiteId = "YOUR_WEBSITE_ID"; // Website Id provided by CJ
        private const string _CJAuthKey = "YOUR_AUTH_KEY"; // Authentication Id provided by CJ
        private const string _Advertiser = "joined"; // result for all joined retailers
        private const string _ProductCatalogUri = "https://product-search.api.cj.com/v2/product-search?website-id={0}&advertiser-ids={1}&keywords={2}"; // Url for product search catalog

        #region "Generic Call"

        private string GetResponse(string url, string method, string siteForAuth)
        {
            WebRequest req = WebRequest.Create(@url);
            req.Method = method;
            req.Headers["Authorization"] = siteForAuth;

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            WebHeaderCollection header = resp.Headers;

            var encoding = ASCIIEncoding.ASCII;
            string responseText = string.Empty;
            using (var reader = new StreamReader(resp.GetResponseStream(), encoding))
            {
                responseText = reader.ReadToEnd();
            }
            resp.Close();
            return responseText;
        }

        #endregion

        #region "CJ"

        public ProductCatalogCJ ProductCatalogSearch(string keyword)
        {
            var url = string.Format(_ProductCatalogUri, _CJWebsiteId, _Advertiser, keyword);

            var responseText = GetResponse(url, "GET", _CJAuthKey);

            ProductCatalogCJ result = new ProductCatalogCJ();

            XmlSerializer serializer = new XmlSerializer(typeof(ProductCatalogCJ));
            using (TextReader reader = new StringReader(@responseText))
            {
                result = (ProductCatalogCJ)serializer.Deserialize(reader);
            }

            return result;
        }

        #endregion
    }


    [XmlRoot("cj-api")]
    public class ProductCatalogCJ
    {
        [XmlElement("products")]
        public Products Products { get; set; }
    }

    public class Products
    {
        [XmlElement("product")]
        public List<ProductList> ProductList { get; set; }
    }

    public class ProductList
    {
        [XmlElement("ad-id")]
        public string AdId { get; set; }

        [XmlElement("advertiser-id")]
        public string AdvertiserId { get; set; }

        [XmlElement("advertiser-name")]
        public string AdvertiserName { get; set; }

        [XmlElement("advertiser-category")]
        public string AdvertiserCatagory { get; set; }

        [XmlElement("buy-url")]
        public string BuyUrl { get; set; }

        [XmlElement("catalog-id")]
        public string CatalogId { get; set; }

        [XmlElement("currency")]
        public string Currency { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("image-url")]
        public string ImageUrl { get; set; }

        [XmlElement("in-stock")]
        public string InStock { get; set; }

        [XmlElement("isbn")]
        public string Isbn { get; set; }

        [XmlElement("manufacturer-name")]
        public string ManufacturerName { get; set; }

        [XmlElement("manufacturer-sku")]
        public string ManufacturerSku { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }

        [XmlElement("retail-price")]
        public string RetailPrice { get; set; }

        [XmlElement("sale-price")]
        public string SalePrice { get; set; }

        [XmlElement("sku")]
        public string Sku { get; set; }

        [XmlElement("upc")]
        public string Upc { get; set; }

    }
}