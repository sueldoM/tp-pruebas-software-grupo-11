namespace TiendaApp;

public class Tienda
{
    public List<Producto> Inventario { get; } = new();

    public void AgregarProducto(Producto producto)
    {
        Inventario.Add(producto);
    }

    public Producto? BuscarProducto(string nombre)
    {
        foreach (Producto producto in Inventario)
        {
            if (producto.Nombre == nombre)
            {
                return producto;
            }
        }

        return null;
    }

    public bool EliminarProducto(string nombre)
    {
        Producto? producto = BuscarProducto(nombre);

        if (producto == null)
        {
            return false;
        }

        Inventario.Remove(producto);

        return true;
    }
}