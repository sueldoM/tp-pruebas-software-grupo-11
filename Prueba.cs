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