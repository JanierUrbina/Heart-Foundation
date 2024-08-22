namespace Heart_Foundation.Helper.Archivos
{
    public class Archivo: IArchivo
    {
        public string GuardarArchivo(IFormFile file, string filePath)
        {
            string ruta = "";
           
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);//Si no existe, crea la ruta
            }

            ruta = Path.Combine(filePath, file.FileName);

            // Usar el bloque `using` para asegurar que el archivo se cierra correctamente
            using (var fileStream = new FileStream(ruta, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }

            return ruta;

        }

        public void EliminarArchivo(string ruta)
        {
            if(File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }
    }
}
