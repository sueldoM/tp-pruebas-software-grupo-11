using Moq;

namespace TiendaApp.Tests;

[TestClass]
public class TiendaTests
{
    // ---------- Ejercicio 4: FIXTURE ----------
    // Se crea una tienda nueva con productos de ejemplo antes de CADA test
    // (Setup) y se limpia después de cada uno (Teardown).
    private Tienda _tienda = null!;

    [TestInitialize]
    public void Setup()
    {
        _tienda = new Tienda();
        _tienda.AgregarProducto(new Producto("Remera", 15000m, "Ropa"));
        _tienda.AgregarProducto(new Producto("Pantalon", 30000m, "Ropa"));
        _tienda.AgregarProducto(new Producto("Zapatillas", 50000m, "Calzado"));
    }

    [TestCleanup]
    public void Teardown()
    {
        _tienda.Inventario.Clear();
    }

    // ---------- Ejercicio 1: pruebas básicas ----------

    [TestMethod]
    public void AgregarProducto_DeberiaAgregarProductoAlInventario()
    {
        Producto producto = new Producto("Campera", 80000m, "Ropa");

        _tienda.AgregarProducto(producto);

        CollectionAssert.Contains(_tienda.Inventario, producto);
        Assert.AreEqual(4, _tienda.Inventario.Count);
    }

    [TestMethod]
    public void BuscarProducto_Existente_DeberiaDevolverElProducto()
    {
        Producto encontrado = _tienda.BuscarProducto("Remera");

        Assert.AreEqual("Remera", encontrado.Nombre);
        Assert.AreEqual(15000m, encontrado.Precio);
    }

    [TestMethod]
    public void EliminarProducto_Existente_DeberiaQuitarloDelInventario()
    {
        Producto remera = _tienda.BuscarProducto("Remera");

        _tienda.EliminarProducto("Remera");

        CollectionAssert.DoesNotContain(_tienda.Inventario, remera);
        Assert.AreEqual(2, _tienda.Inventario.Count);
    }

    // ---------- Ejercicio 2: excepciones ----------

    [TestMethod]
    public void BuscarProducto_Inexistente_DeberiaLanzarKeyNotFoundException()
    {
        Assert.ThrowsExactly<KeyNotFoundException>(
            () => _tienda.BuscarProducto("Inexistente"));
    }

    [TestMethod]
    public void EliminarProducto_Inexistente_DeberiaLanzarKeyNotFoundException()
    {
        Assert.ThrowsExactly<KeyNotFoundException>(
            () => _tienda.EliminarProducto("Inexistente"));
    }

    [TestMethod]
    public void EliminarProducto_Inexistente_NoDeberiaModificarElInventario()
    {
        try
        {
            _tienda.EliminarProducto("Inexistente");
        }
        catch (KeyNotFoundException)
        {
            // esperado
        }

        Assert.AreEqual(3, _tienda.Inventario.Count);
    }

    // ---------- Ejercicio 3: dobles de prueba (mocks con Moq) ----------

    [TestMethod]
    public void AplicarDescuento_DeberiaCalcularNuevoPrecioYLlamarActualizarPrecio()
    {
        // Arrange: producto simulado (no se usa un Producto real)
        Mock<Producto> productoMock = new Mock<Producto>("Mochila", 200m, "Accesorios");
        productoMock.Setup(p => p.Nombre).Returns("Mochila");
        productoMock.Setup(p => p.Precio).Returns(200m);

        Tienda tienda = new Tienda();
        tienda.AgregarProducto(productoMock.Object);

        // Act: 25% de descuento sobre 200 => 150
        tienda.AplicarDescuento("Mochila", 25m);

        // Assert: se verificó la interacción con el colaborador
        productoMock.Verify(p => p.ActualizarPrecio(150m), Times.Once());
    }

    [TestMethod]
    public void AplicarDescuento_ProductoInexistente_NoDeberiaLlamarActualizarPrecio()
    {
        Mock<Producto> productoMock = new Mock<Producto>("Mochila", 200m, "Accesorios");
        productoMock.Setup(p => p.Nombre).Returns("Mochila");
        productoMock.Setup(p => p.Precio).Returns(200m);

        Tienda tienda = new Tienda();
        tienda.AgregarProducto(productoMock.Object);

        Assert.ThrowsExactly<KeyNotFoundException>(
            () => tienda.AplicarDescuento("Inexistente", 10m));

        productoMock.Verify(
            p => p.ActualizarPrecio(It.IsAny<decimal>()), Times.Never());
    }

    [TestMethod]
    public void AplicarDescuento_PorcentajeInvalido_DeberiaLanzarExcepcion()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => _tienda.AplicarDescuento("Remera", 150m));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => _tienda.AplicarDescuento("Remera", -5m));
    }

    // ---------- Ejercicio 5: integración (objetos reales + fixture) ----------

    [TestMethod]
    public void CalcularTotalCarrito_SinDescuentos_DeberiaSumarLosPrecios()
    {
        decimal total = _tienda.CalcularTotalCarrito(new[] { "Remera", "Pantalon" });

        Assert.AreEqual(45000m, total);
    }

    [TestMethod]
    public void CalcularTotalCarrito_CarritoVacio_DeberiaDevolverCero()
    {
        Assert.AreEqual(0m, _tienda.CalcularTotalCarrito(Array.Empty<string>()));
    }

    [TestMethod]
    public void CalcularTotalCarrito_ProductoInexistente_DeberiaLanzarExcepcion()
    {
        Assert.ThrowsExactly<KeyNotFoundException>(
            () => _tienda.CalcularTotalCarrito(new[] { "Remera", "Inexistente" }));
    }

    [TestMethod]
    public void FlujoCompleto_ConDescuentos_DeberiaCalcularElTotalCorrecto()
    {
        // Remera 15000 - 10% = 13500 ; Zapatillas 50000 - 50% = 25000 ; Pantalon sin descuento = 30000
        _tienda.AplicarDescuento("Remera", 10m);
        _tienda.AplicarDescuento("Zapatillas", 50m);

        decimal total = _tienda.CalcularTotalCarrito(
            new[] { "Remera", "Pantalon", "Zapatillas" });

        Assert.AreEqual(68500m, total);
    }

    [TestMethod]
    public void FlujoCompleto_AgregarDescontarYCalcular_DeberiaIntegrarTodasLasFunciones()
    {
        _tienda.AgregarProducto(new Producto("Gorra", 10000m, "Accesorios"));
        _tienda.AplicarDescuento("Gorra", 20m);              // 8000

        decimal total = _tienda.CalcularTotalCarrito(new[] { "Gorra", "Gorra" });

        Assert.AreEqual(16000m, total);
    }
}
