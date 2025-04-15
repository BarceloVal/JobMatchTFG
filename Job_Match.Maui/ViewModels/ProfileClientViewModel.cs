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
        // Estas son las propiedades que el XAML usará con Binding
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string DocType { get; set; }
        public string Doc { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public ProfileClientViewModel(_UserDto user)
        {
            FirstName = user.first_name;
            LastName = user.last_name;
            Mail = user.mail;
            //Password = user.password;
            //Doc = user.
            //Direccion = "Calle 123"; 
            //CodigoPostal = "46220";  
            //Telefono1 = "622 58 55 11"; 
            //Telefono2 = ""; 
        }
    }
}
