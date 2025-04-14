using Job_Match.Maui.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class ProfileClientViewModel
    {
        public string FullName { get; set; }

        // Estas son las propiedades que el XAML usará con Binding
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Documento { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }

        public ProfileClientViewModel(_UserDto user)
        {
            FullName = $"{user.first_name} {user.last_name}";
            Nombre = user.first_name;
            Apellido = user.last_name;
            Mail = user.mail;
            Documento = "Y1234567J"; // temporal
            Direccion = "Calle 123"; // temporal
            CodigoPostal = "46220";  // temporal
            Telefono1 = "622 58 55 11"; // temporal
            Telefono2 = ""; // por ahora vacío
        }
    }
}
