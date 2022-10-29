using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjGetShopee_v2._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSmallType_Click(object sender, EventArgs e)
        {
            iSpanProjectEntities dbContext = new iSpanProjectEntities();
            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            EdgeDriver driver = new EdgeDriver(edgeOptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://shopee.tw/all_categories");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var bigTypes = dbContext.BigTypes.Select(i => i).ToList();
            var bigTypeNames = bigTypes.Select(i => i.BigTypeName).ToList();
            var bigTypeTags = driver.FindElements(By.CssSelector("a.category-grid"));
            List<string> bigTypeUrlList = new List<string>();
            foreach (var i in bigTypeTags)
            {
                try
                {
                    if (bigTypeNames.Contains(i.Text))
                    {
                        bigTypeUrlList.Add(i.GetAttribute("href"));
                    }

                }
                catch
                {
                    continue;
                }
            }
            foreach (var a in bigTypeUrlList)
            {
                driver.Navigate().GoToUrl(a);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                string bigTypeName = driver.FindElement(By.CssSelector(".shopee-category-list__main-category__link")).Text;
                int bigTypeID = dbContext.BigTypes.Where(i => i.BigTypeName == bigTypeName).Select(i => i.BigTypeID).FirstOrDefault();
                if (bigTypeID < 1)
                {
                    continue;
                }
                try
                {
                    driver.FindElement(By.CssSelector("div.shopee-category-list__toggle-btn")).Click();
                }
                catch
                {

                }
                var smallTypeTaqs = driver.FindElements(By.CssSelector("a.shopee-category-list__sub-category"));
                List<string> smallTypeNameList = new List<string>();
                foreach (var j in smallTypeTaqs)
                {
                    string smallTypeName = j.Text;
                    if (smallTypeName == "")
                    {
                        continue;
                    }
                    var smallType = dbContext.SmallTypes.Where(i => i.BigTypeID == bigTypeID && i.SmallTypeName == smallTypeName).FirstOrDefault();
                    if (smallType == null)
                    {
                        SmallType smallType1 = new SmallType
                        {
                            SmallTypeName = smallTypeName,
                            BigTypeID = bigTypeID,
                        };
                        dbContext.SmallTypes.Add(smallType1);
                    }
                }
            }
            dbContext.SaveChanges();
        }

        private async void btnGetPanasonic_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            iSpanProjectEntities dbContext = new iSpanProjectEntities();
            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            EdgeDriver driver = new EdgeDriver(edgeOptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://shopee.tw/panasonic.tw");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            List<string> itemUrlList = new List<string>();
            int totalPage = Convert.ToInt32(driver.FindElement(By.CssSelector(".shopee-mini-page-controller__total")).Text);
            for (int i = 1; i <= totalPage; i++)
            {
                var itemUrls = driver.FindElements(By.CssSelector(".shop-search-result-view__item>a"));
                foreach (var j in itemUrls)
                {
                    itemUrlList.Add(j.GetAttribute("href"));
                }
                driver.FindElement(By.CssSelector(".shopee-mini-page-controller__next-btn")).Click();
            }

            foreach (var itemUrl in itemUrlList)
            {
                driver.Navigate().GoToUrl(itemUrl);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var bigTypeName = driver.FindElements(By.CssSelector("a.CyVtI7"))[1].Text;
                var smallTypeName = driver.FindElements(By.CssSelector("a.CyVtI7"))[2].Text;
                int smallTypeID = dbContext.SmallTypes.Where(i => i.BigType.BigTypeName == bigTypeName && i.SmallTypeName == smallTypeName).Select(i => i.SmallTypeID).FirstOrDefault();
                if (smallTypeID > 0)
                {
                    var productName = driver.FindElement(By.CssSelector("div._2rQP1z")).Text;
                    listBox1.Items.Add($"{productName}");
                    var description = driver.FindElement(By.CssSelector("._2jrvqA")).Text;
                    int productID = dbContext.Products.Where(i => i.ProductName == productName && i.SmallTypeID == smallTypeID && i.Description == description).Select(i=>i.ProductID).FirstOrDefault();
                    if (productID < 1)
                    {
                        Product product = new Product
                        {
                            ProductName = productName,
                            SmallTypeID = smallTypeID,
                            MemberID = 11,
                            RegionID = 37,
                            Description = description,
                            ProductStatusID = 0,
                            EditTime = DateTime.Now,
                            CustomizedCategoryID = 1,
                        };
                        dbContext.Products.Add(product);
                        dbContext.SaveChanges();
                        productID = dbContext.Products.OrderByDescending(i => i.ProductID).Select(i => i.ProductID).FirstOrDefault();
                    }

                    var productVariation = driver.FindElements(By.CssSelector(".product-variation"));
                    var productDetails = dbContext.ProductDetails.Where(i => i.ProductID == productID).Select(i => i);
                    if (productDetails.Count() <= 0 || productDetails.Count() < productVariation.Count)
                    {
                        if (productVariation.Count > 0)
                        {
                            foreach (var j in driver.FindElements(By.CssSelector(".product-variation")))
                            {
                                j.Click();
                                Thread.Sleep(300);
                                string style = j.Text;
                                listBox1.Items.Add($"    {style}");
                                var IsStyle = dbContext.ProductDetails.Where(i => i.ProductID == productID && i.Style == style).FirstOrDefault();
                                if (IsStyle == null)
                                {
                                    int quantity = random.Next(10, 101);
                                    decimal unitPrice = Convert.ToDecimal(driver.FindElement(By.CssSelector("._2Shl1j")).Text.Trim(' ').TrimStart('$').Replace(",", "").Replace(" ", ""));
                                    var productDetailPicUrls = driver.FindElements(By.CssSelector("._3CXjs-._3DKwBj"));
                                    string productDetailPicUrl = "";
                                    if (productDetailPicUrls.Count > 0)
                                    {
                                        productDetailPicUrl = driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj")).GetAttribute("style").Split('"')[1];
                                    }
                                    byte[] photo = null;
                                    if (productDetailPicUrl != "")
                                    {
                                        HttpClient client = new HttpClient();
                                        photo = await client.GetByteArrayAsync(productDetailPicUrl);                                        
                                    }
                                    ProductDetail productDetail = new ProductDetail
                                    {
                                        ProductID = productID,
                                        Style = style,
                                        Quantity = quantity,
                                        UnitPrice = unitPrice,
                                        Pic = photo,
                                    };
                                    dbContext.ProductDetails.Add(productDetail);
                                }
                            }
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            string style = "樣式一";
                            listBox1.Items.Add($"    {style}");
                            var IsStyle = dbContext.ProductDetails.Where(i => i.ProductID == productID && i.Style == "樣式一").FirstOrDefault();
                            if (IsStyle == null)
                            {
                                int quantity = random.Next(10, 101);
                                decimal unitPrice = Convert.ToDecimal(driver.FindElement(By.CssSelector("._2Shl1j")).Text.Trim(' ').TrimStart('$').Replace(",", "").Replace(" ", ""));
                                var productDetailPicUrls = driver.FindElements(By.CssSelector("._3CXjs-._3DKwBj"));
                                string productDetailPicUrl = "";
                                if (productDetailPicUrls.Count > 0)
                                {
                                    productDetailPicUrl = driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj")).GetAttribute("style").Split('"')[1];
                                }
                                byte[] photo = null;
                                if (productDetailPicUrl != "")
                                {
                                    HttpClient client = new HttpClient();
                                    photo = await client.GetByteArrayAsync(productDetailPicUrl);
                                }
                                ProductDetail productDetail = new ProductDetail
                                {
                                    ProductID = productID,
                                    Style = style,
                                    Quantity = quantity,
                                    UnitPrice = unitPrice,
                                    Pic = photo,
                                };
                                dbContext.ProductDetails.Add(productDetail);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                    Thread.Sleep(300);
                    var pic = driver.FindElements(By.CssSelector("._2_49CO"));
                    if (pic.Count >0)
                    {
                        driver.FindElement(By.CssSelector("._2_49CO")).Click();
                    }
                    else
                    {
                        driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj")).Click();
                    }
                    Thread.Sleep(300);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    var productPicUrls = driver.FindElements(By.CssSelector("._1BkYjB._3DKwBj"));
                    if (productPicUrls.Count > 0)
                    {
                        List<string> productPicUrlList = new List<string>();
                        foreach (var j in productPicUrls)
                        {
                            string productPicUrl = j.GetAttribute("style").Split('"')[1];
                            if (productPicUrlList.Contains(productPicUrl))
                            {
                                continue;
                            }
                            else
                            {
                                productPicUrlList.Add(productPicUrl);
                            }
                        }
                        foreach (var j in productPicUrlList)
                        {
                            HttpClient client = new HttpClient();
                            byte[] photo = await client.GetByteArrayAsync(j);
                            var IsPic = dbContext.ProductPics.Where(i => i.ProductID == productID && i.Pic == photo).FirstOrDefault();
                            if (IsPic == null)
                            {
                                ProductPic productPic = new ProductPic
                                {
                                    ProductID = productID,
                                    Pic = photo,
                                };
                                dbContext.ProductPics.Add(productPic);
                            }
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
            MessageBox.Show("爬完了"); 
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            iSpanProjectEntities dbContext = new iSpanProjectEntities();
            var q = dbContext.Products.Where(i => i.ProductID == 102).FirstOrDefault();
            dbContext.Products.Remove(q);
            dbContext.SaveChanges();
            MessageBox.Show("已刪除");
            btnDelete.Enabled = false;
        }

        private async void btnGetAllItem_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            iSpanProjectEntities dbContext = new iSpanProjectEntities();
            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            EdgeDriver driver = new EdgeDriver(edgeOptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://shopee.tw/all_categories");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var bigTypeUrlTaq = driver.FindElements(By.CssSelector("a.category-grid"));
            List<string> bigTypeUrlList = new List<string>();
            foreach (var i in bigTypeUrlTaq)
            {
                bigTypeUrlList.Add(i.GetAttribute("href"));
            }
            foreach (var a in bigTypeUrlList)
            {
                driver.Navigate().GoToUrl(a);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driver.FindElement(By.CssSelector(".shopee-category-list__toggle-btn")).Click();
                Thread.Sleep(300);
                string bigTypeName = driver.FindElement(By.CssSelector(".shopee-category-list__main-category")).Text;
                var smallTypeUrlTaq = driver.FindElements(By.CssSelector("a.shopee-category-list__sub-category"));
                Dictionary<string, string> smallTypeDic = new Dictionary<string, string>();
                foreach (var b in smallTypeUrlTaq)
                {
                    smallTypeDic.Add(b.GetAttribute("htrf"), b.Text);
                }
                foreach (var b in smallTypeDic)
                {
                    var existedProductCount = dbContext.Products.Where(i => i.SmallType.SmallTypeName == b.Value && i.SmallType.BigType.BigTypeName == bigTypeName);
                    if (existedProductCount.Count() > 8)
                    {
                        continue;
                    }
                    driver.Navigate().GoToUrl(b.Key);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    var itemTaqs = driver.FindElements(By.CssSelector(".shopee-search-item-result__item>a"));
                    List<string> itemUrls = new List<string>();
                    foreach (var c in itemTaqs)
                    {
                        itemUrls.Add(c.GetAttribute("href"));
                    }
                    int itemCount = random.Next(5, 16);
                    for (int c = 0; c < itemCount; c++)
                    {
                        driver.Navigate().GoToUrl(itemUrls[c]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        string productName = driver.FindElement(By.CssSelector("._2rQP1z")).Text;
                        string description = driver.FindElement(By.CssSelector("._1MqcWX")).Text;
                        int smallTypeID = dbContext.SmallTypes.Where(i => i.SmallTypeName == b.Value && i.BigType.BigTypeName == bigTypeName).Select(i => i.SmallTypeID).FirstOrDefault();
                        var IsProduct = dbContext.Products.Where(i => i.ProductName == productName && i.Description == description && i.SmallTypeID == smallTypeID).FirstOrDefault();
                        int productID = 0;
                        if (IsProduct == null)
                        {
                            var memberIDList = dbContext.MemberAccounts.Where(i => i.MemberID != 11).Select(i => i.MemberID).ToList();
                            int memberIDIndex = random.Next(0, memberIDList.Count);
                            int memberID = memberIDList[memberIDIndex];
                            int regionID = dbContext.MemberAccounts.Where(i => i.MemberID == memberID).Select(i => i.RegionID).FirstOrDefault();
                            Product product = new Product
                            {
                                ProductName = productName,
                                SmallTypeID = smallTypeID,
                                MemberID = memberID,
                                RegionID = regionID,
                                Description = description,
                                ProductStatusID = 0,
                                EditTime = DateTime.Now,
                                CustomizedCategoryID = 1,
                            };
                            dbContext.Products.Add(product);
                            dbContext.SaveChanges();
                            productID = dbContext.Products.Where(i => i.ProductName == productName && i.SmallTypeID == smallTypeID).OrderByDescending(i => i.ProductID).Select(i => i.ProductID).FirstOrDefault();
                        }
                        else
                        {
                            productID = IsProduct.ProductID;
                        }
                        var styleTaqs = driver.FindElements(By.CssSelector(".product-variation"));
                        var productDetails = dbContext.ProductDetails.Where(i => i.ProductID == productID).Select(i => i);
                        if (productDetails.Count() <= 0 || productDetails.Count() < styleTaqs.Count)
                        {
                            if (styleTaqs.Count > 0)
                            {
                                foreach (var d in driver.FindElements(By.CssSelector(".product-variation")))
                                {
                                    d.Click();
                                    Thread.Sleep(300);
                                    string style = d.Text;
                                    var IsStyle = dbContext.ProductDetails.Where(i => i.Style == style && i.ProductID == productID).FirstOrDefault();
                                    if (IsStyle != null)
                                    {
                                        continue;
                                    }
                                    int quantity = random.Next(10, 101);
                                    var price = driver.FindElement(By.CssSelector("._2Shl1j"));
                                    if (price == null)
                                    {
                                        continue;
                                    }
                                    string modifiedPrice = price.Text.Trim(' ').TrimStart('$').Replace(",", "").Replace(" ", "");
                                    decimal unitPrice;
                                    decimal.TryParse(modifiedPrice, out unitPrice);
                                    if (unitPrice == 0)
                                    {
                                        modifiedPrice = price.Text.Split('-')[0].Trim(' ').TrimStart('$').Replace(",", "").Replace(" ", "");
                                        decimal.TryParse(modifiedPrice, out unitPrice);
                                    }
                                    if (unitPrice == 0)
                                    {
                                        continue;
                                    }
                                    var productDetailPicTaq = driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj"));
                                    string productDetailPicUrl = "";
                                    if (productDetailPicTaq != null)
                                    {
                                        productDetailPicUrl = productDetailPicTaq.GetAttribute("style").Split('"')[1];
                                    }
                                    byte[] photo = null;
                                    if (productDetailPicUrl != "")
                                    {
                                        HttpClient client = new HttpClient();
                                        photo = await client.GetByteArrayAsync(productDetailPicUrl);
                                    }
                                    ProductDetail productDetail = new ProductDetail
                                    {
                                        ProductID = productID,
                                        Style = style,
                                        Quantity = quantity,
                                        UnitPrice = unitPrice,
                                        Pic = photo,
                                    };
                                    dbContext.ProductDetails.Add(productDetail);
                                }
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                string style = "樣式一";
                                var IsStyle = dbContext.ProductDetails.Where(i => i.Style == "樣式一" && i.ProductID == productID).FirstOrDefault();
                                if (IsStyle == null)
                                {
                                    int quantity = random.Next(10, 101);
                                    decimal unitPrice = Convert.ToDecimal(driver.FindElement(By.CssSelector("._2Shl1j")).Text.Trim(' ').TrimStart('$').Replace(",", "").Replace(" ", ""));
                                    var productDetailPicUrls = driver.FindElements(By.CssSelector("._3CXjs-._3DKwBj"));
                                    string productDetailPicUrl = "";
                                    if (productDetailPicUrls.Count > 0)
                                    {
                                        productDetailPicUrl = driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj")).GetAttribute("style").Split('"')[1];
                                    }
                                    byte[] photo = null;
                                    if (productDetailPicUrl != "")
                                    {
                                        HttpClient client = new HttpClient();
                                        photo = await client.GetByteArrayAsync(productDetailPicUrl);
                                    }
                                    ProductDetail productDetail = new ProductDetail
                                    {
                                        ProductID = productID,
                                        Style = style,
                                        Quantity = quantity,
                                        UnitPrice = unitPrice,
                                        Pic = photo,
                                    };
                                    dbContext.ProductDetails.Add(productDetail);
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                        Thread.Sleep(300);
                        var pic = driver.FindElements(By.CssSelector("._2_49CO"));
                        if (pic.Count > 0)
                        {
                            driver.FindElement(By.CssSelector("._2_49CO")).Click();
                        }
                        else
                        {
                            driver.FindElement(By.CssSelector("._3CXjs-._3DKwBj")).Click();
                        }
                        Thread.Sleep(300);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        var productPicUrls = driver.FindElements(By.CssSelector("._1BkYjB._3DKwBj"));
                        if (productPicUrls.Count > 0)
                        {
                            List<string> productPicUrlList = new List<string>();
                            foreach (var j in productPicUrls)
                            {
                                string productPicUrl = j.GetAttribute("style").Split('"')[1];
                                if (productPicUrlList.Contains(productPicUrl))
                                {
                                    continue;
                                }
                                else
                                {
                                    productPicUrlList.Add(productPicUrl);
                                }
                            }
                            foreach (var j in productPicUrlList)
                            {
                                HttpClient client = new HttpClient();
                                byte[] photo = await client.GetByteArrayAsync(j);
                                var IsPic = dbContext.ProductPics.Where(i => i.ProductID == productID && i.Pic == photo).FirstOrDefault();
                                if (IsPic == null)
                                {
                                    ProductPic productPic = new ProductPic
                                    {
                                        ProductID = productID,
                                        Pic = photo,
                                    };
                                    dbContext.ProductPics.Add(productPic);
                                }
                            }
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            MessageBox.Show("爬完了");
        }
    }
}
