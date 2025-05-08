using RetailInventorySystem.Domain.Entity;
using RetailInventorySystem.Domain.ValueObject;

namespace DomainModeling.Tests;

[TestClass]
public class InventoryTests
{
    [TestMethod]
    public void ProductName_ShouldCreateValidProductName()
    {
        var name = new ProductName("Laptop");
        Assert.AreEqual("Laptop", name.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProductName_ShouldThrowException_ForEmptyName()
    {
        new ProductName("");
    }

    [TestMethod]
    public void ProductCategory_ShouldCreateValidCategory()
    {
        var category = new ProductCategory("Electronics");
        Assert.AreEqual("Electronics", category.Category);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProductCategory_ShouldThrowException_ForInvalidCategory()
    {
        new ProductCategory("Toys");
    }

    [TestMethod]
    public void Price_ShouldCreateValidPrice()
    {
        var price = new Price(1500.00m);
        Assert.AreEqual(1500.00m, price.Amount);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Price_ShouldThrowException_ForNegativeOrZeroPrice()
    {
        new Price(-100.00m);
    }

    [TestMethod]
    public void StockQuantity_ShouldCreateValidStockQuantity()
    {
        var stock = new StockQuantity(50);
        Assert.AreEqual(50, stock.Quantity);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void StockQuantity_ShouldThrowException_ForNegativeStock()
    {
        new StockQuantity(-10);
    }

    [TestMethod]
    public void Discount_ShouldCreateValidDiscount()
    {
        var discount = new Discount(20);
        Assert.AreEqual(20, discount.Percentage);
        Assert.AreEqual(800.00m, discount.Apply(1000.00m));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Discount_ShouldThrowException_ForInvalidPercentage()
    {
        new Discount(120);
    }

    [TestMethod]
    public void Product_ShouldCreateValidProduct()
    {
        var product = new Product(new ProductName("Laptop"), new ProductCategory("Electronics"), new Price(2000.00m), new StockQuantity(50));
        Assert.AreEqual("Laptop", product.Name.Name);
        Assert.AreEqual("Electronics", product.Category.Category);
        Assert.AreEqual(2000.00m, product.Price.Amount);
        Assert.AreEqual(50, product.Stock.Quantity);
    }

    [TestMethod]
    public void Product_ShouldApplyDiscountCorrectly()
    {
        var product = new Product(new ProductName("Laptop"), new ProductCategory("Electronics"), new Price(2000.00m), new StockQuantity(50));
        product.SetDiscount(new Discount(10));
        Assert.AreEqual(1800.00m, product.GetFinalPrice());
    }

    [TestMethod]
    public void Product_ShouldManageStockCorrectly()
    {
        var product = new Product(new ProductName("Laptop"), new ProductCategory("Electronics"), new Price(2000.00m), new StockQuantity(50));
        product.AddStock(20);
        Assert.AreEqual(70, product.Stock.Quantity);
        product.ReduceStock(30);
        Assert.AreEqual(40, product.Stock.Quantity);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Product_ShouldThrowException_WhenReducingStockBelowZero()
    {
        var product = new Product(new ProductName("Laptop"), new ProductCategory("Electronics"), new Price(2000.00m), new StockQuantity(10));
        product.ReduceStock(20);
    }

    [TestMethod]
    public void ProductBundle_ShouldCreateValidBundle()
    {
        var laptop = new Product(new ProductName("Laptop"), new ProductCategory("Electronics"), new Price(2000.00m), new StockQuantity(10));
        var mouse = new Product(new ProductName("Mouse"), new ProductCategory("Electronics"), new Price(50.00m), new StockQuantity(50));
        var bundle = new ProductBundle("Laptop + Mouse Bundle");
        bundle.AddProduct(laptop);
        bundle.AddProduct(mouse);
        bundle.SetBundlePrice(1900.00m);
        Assert.AreEqual("Laptop + Mouse Bundle - Price: $1,900.00, Items: 2", bundle.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProductBundle_ShouldThrowException_ForEmptyBundleName()
    {
        new ProductBundle("");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProductBundle_ShouldThrowException_ForInvalidBundlePrice()
    {
        var bundle = new ProductBundle("Test Bundle");
        bundle.SetBundlePrice(-100.00m);
    }
}
