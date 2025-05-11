using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class EditProfileClientViewModel : INotifyPropertyChanged
    {
        private readonly ApiService.ApiService _apiService = new();

        private ProfileClientDto _profileDto;

        public Guid Id { get; set; }
        public Guid IdUser { get; set; }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _document;
        public string Document
        {
            get => _document;
            set { _document = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        private string _phone1;
        public string Phone1
        {
            get => _phone1;
            set { _phone1 = value; OnPropertyChanged(); }
        }

        private string _phone2;
        public string Phone2
        {
            get => _phone2;
            set { _phone2 = value; OnPropertyChanged(); }
        }

        public ObservableCollection<m_doc_type> DocTypes { get; } = new();
        public ObservableCollection<m_postal_code> PostalCodes { get; } = new();

        private m_doc_type _selectedDocType;
        public m_doc_type SelectedDocType
        {
            get => _selectedDocType;
            set { _selectedDocType = value; OnPropertyChanged(); }
        }

        private m_postal_code _selectedPostalCode;
        public m_postal_code SelectedPostalCode
        {
            get => _selectedPostalCode;
            set { _selectedPostalCode = value; OnPropertyChanged(); }
        }

        public EditProfileClientViewModel(ProfileClientDto dto)
        {
            _profileDto = dto;

            Id = dto.Id;
            IdUser = dto.IdUser;

            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Document = dto.Document;
            Address = dto.Address;
            Phone1 = dto.Phone1;
            Phone2 = dto.Phone2;
        }

        public async Task InitAsync()
        {
            var docTypes = await _apiService.GetDocTypesAsync();
            DocTypes.Clear();
            foreach (var dt in docTypes)
            {
                DocTypes.Add(dt);
                if (dt.id == _profileDto.IdDocType)
                    SelectedDocType = dt;
            }

            var postalCodes = await _apiService.GetPostalCodesAsync();
            PostalCodes.Clear();
            foreach (var pc in postalCodes)
            {
                PostalCodes.Add(pc);
                if ($"{pc.cod_auto_name}/{pc.cod_pro_name}/{pc.cod_loc_name}" == _profileDto.PostalCodeName)
                    SelectedPostalCode = pc;
            }
        }

        public UpdateProfileClientDto BuildUpdateDto() => new()
        {
            Id = Id,
            IdUser = IdUser,
            FirstName = FirstName,
            LastName = LastName,
            Document = Document,
            Address = Address,
            IdDocType = SelectedDocType.id,
            IdPostalCode = SelectedPostalCode.id,
            Phone1 = Phone1,
            Phone2 = Phone2
        };

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}