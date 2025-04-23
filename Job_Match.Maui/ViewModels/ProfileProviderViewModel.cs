using Job_Match.Data.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class ProfileProviderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TradeName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Guid IdDocType { get; set; }
        public string Document { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public Guid IdPostalCode { get; set; }
        public string PostalCodeName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Description { get; set; }
        public string MapCoordinates { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ProfileProviderViewModel()
        {
            var user = App.CurrentUser as ProfileProviderDto;
            if (user != null)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                TradeName = user.TradeName;
                Mail = user.Mail;
                IdDocType = user.IdDocType;
                Document = user.Document;
                Website = user.Website;
                Address = user.Address;
                IdPostalCode = user.IdPostalCode;
                PostalCodeName = user.PostalCodeName;
                Phone1 = user.Phone1;
                Phone2 = user.Phone2;
                Description = user.Description;
                MapCoordinates = user.MapCoordinates;
            }
        }
    }
}
