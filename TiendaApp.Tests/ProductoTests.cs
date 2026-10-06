namespace TiendaApp.Tests;

[TestClass]
public class ProductoTests
{
    [TestMethod]
    public void Constructor_DeberiaAsignarNombrePrecioYCategoria()
    {
        Producto producto = new Producto("Remera", 15000m, "Ropa");

        Assert.AreEqual("Remera", producto.Nombre);
        Assert.AreEqual(15000m, producto.Precio);
        Assert.AreEqual("Ropa", producto.Categoria);
    }

    [TestMethod]
    public void ActualizarPrecio_ConPrecioValido_DeberiaCambiarElPrecio()
    {
        Producto producto = new Producto("Remera", 15000m, "Ropa");

        producto.ActualizarPrecio(12000m);

        Assert.AreEqual(12000m, producto.Precio);
    }

    [TestMethod]
    public void ActualizarPrecio_ConPrecioCero_DeberiaPermitirlo()
    {
        Producto producto = new Producto("Remera", 15000m, "Ropa");

        producto.ActualizarPrecio(0m);

        Assert.AreEqual(0m, producto.Precio);
    }

    [TestMethod]
    public void ActualizarPrecio_ConPrecioNegativo_DeberiaLanzarExcepcion()
    {
        Producto producto = new Producto("Remera", 15000m, "Ropa");

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => producto.ActualizarPrecio(-1m));
    }

    [TestMethod]
    public void ActualizarPrecio_ConPrecioNegativo_NoDeberiaModificarElPrecio()
    {
        Producto producto = new Producto("Remera", 15000m, "Ropa");

        try
        {
            producto.ActualizarPrecio(-1m);
        }
        catch (ArgumentOutOfRangeException)
        {
            // esperado
        }

        Assert.AreEqual(15000m, producto.Precio);
    }
}
