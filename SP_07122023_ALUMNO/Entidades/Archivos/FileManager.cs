using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path;

        static FileManager() 
        {
            FileManager.path = "C:\\Users\\Rojas\\OneDrive\\Documentos\\" + "\\SP_07122023_ALUMNO\\";
            FileManager.ValidaExistenciaDeDirectorio();
        }

        private static void ValidaExistenciaDeDirectorio()
        {
            if (!Directory.Exists(FileManager.path))
            {
                try
                {
                    Directory.CreateDirectory(FileManager.path);
                }
                catch(Exception ex)
                {
                    throw new FileManagerException("Error al crear el direcctorio", ex);
                }
            }
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            string archivo = Path.Combine(FileManager.path, nombreArchivo);
            using (StreamWriter writeFile = new StreamWriter(archivo, append))
            {
                writeFile.Write(data);
            }
        }

        public static bool Serializar<T>(T elemento, string nombreArchivo) where T : class
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(elemento, options);
            if (json != null)
            {
                FileManager.Guardar(json, nombreArchivo, false);
                return true;
            }
            return false;
        }
    }
}
