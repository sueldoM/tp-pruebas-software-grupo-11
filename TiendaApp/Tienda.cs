namespace TiendaApp;

public class Tienda
{
    public List<Producto> Inventario { get; } = new();

    // ejersicio 1
    public void AgregarProducto(Producto producto)
    {
        Inventario.Add(producto);
    }

    // ejercicio 2: ya no devuelve null; lanza excepción si no existe.
    public Producto BuscarProducto(string nombre)
    {
        foreach (Producto producto in Inventario)
        {
            if (producto.Nombre == nombre)
            {
                return producto;
            }
        }

        throw new KeyNotFoundException(
            $"No se encontró el producto '{nombre}'.");
    }

    // ejercicio 2: lanza excepción si el producto no existe (la propaga BuscarProducto).
    public void EliminarProducto(string nombre)
    {
        Producto producto = BuscarProducto(nombre);
        Inventario.Remove(producto);
    }

    // ejercicio 3: aplica un descuento (en %) delegando el cambio de precio en Producto.ActualizarPrecio.
    public void AplicarDescuento(string nombre, decimal porcentaje)
    {
        if (porcentaje < 0 || porcentaje > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(porcentaje),
                "El porcentaje debe estar entre 0 y 100.");
        }

        Producto producto = BuscarProducto(nombre);
        decimal nuevoPrecio = producto.Precio * (1 - porcentaje / 100m);
        producto.ActualizarPrecio(nuevoPrecio);
    }

    // ejercicio 5: suma el precio de cada producto del carrito. Un nombre repetido cuenta una vez por cada aparición.
    public decimal CalcularTotalCarrito(IEnumerable<string> carrito)
    {
        decimal total = 0m;

        foreach (string nombre in carrito)
        {
            total += BuscarProducto(nombre).Precio;
        }

        return total;
    }
}
