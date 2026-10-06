using Microsoft.VisualStudio.TestTools.UnitTesting;
using TiendaApp;

namespace TiendaApp.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
    }
}

[TestClass]
public class TiendaTests
{
    [TestMethod]
    public void AgregarProducto_DeberiaAgregarProductoAlInventario()
    {
        // Arrange
        Tienda tienda = new Tienda();
        Producto producto =
            new Producto("Remera", 15000m, "Ropa");

        // Act
        tienda.AgregarProducto(producto);

        // Assert
        CollectionAssert.Contains(
            tienda.Inventario,
            producto
        );
    }
}