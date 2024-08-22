namespace Heart_Foundation.Helper.Archivos
{
    public interface IArchivo
    {
        string GuardarArchivo(IFormFile file, string ruta);
        void EliminarArchivo(string ruta);
    }
}
