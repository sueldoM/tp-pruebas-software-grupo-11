namespace TiendaApp;

public class Producto
{
    public virtual string Nombre { get; set; }
    public virtual decimal Precio { get; set; }
    public virtual string Categoria { get; set; }

    public Producto(string nombre, decimal precio, string categoria)
    {
        Nombre = nombre;
        Precio = precio;
        Categoria = categoria;
    }

    // ejersisio 2: cambia el precio y lanza excepción si el nuevo precio es negativo.
    public virtual void ActualizarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(nuevoPrecio),
                "El precio no puede ser negativo.");
        }

        Precio = nuevoPrecio;
    }
}
