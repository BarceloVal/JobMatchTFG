using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Job_Match.Maui.ViewModels
{
    public class EditProfileClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Llama al evento PropertyChanged para actualizar la interfaz de usuario cuando una propiedad cambia.
        /// </summary>
        /// <param name="propName">Nombre de la propiedad que cambió. Se asigna automáticamente si no se especifica.</param>
        private void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private readonly ApiService.ApiService _apiService = new();

        private ProfileClientDto _profileDto;

        public Guid Id { get; set; }
        public Guid IdUser { get; set; }

        // -----------------------
        // ------- Nombre -------
        // -----------------------
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    ValidateFirstName();
                }
            }
        }

        private string _firstNameError;
        public string FirstNameError
        {
            get => _firstNameError;
            set
            {
                _firstNameError = value;
                OnPropertyChanged(nameof(FirstNameError));
                OnPropertyChanged(nameof(IsFirstNameValid));
            }
        }

        public bool IsFirstNameValid => string.IsNullOrEmpty(FirstNameError);

        /// <summary>Valida el campo Nombre. Máximo 30 caracteres, obligatorio.</summary>
        private void ValidateFirstName()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                FirstNameError = "Campo obligatorio.";
            else if (FirstName.Length > 30)
                FirstNameError = "Máximo 30 caracteres.";
            else
                FirstNameError = string.Empty;
        }


        // -----------------------
        // ------ Apellido ------
        // -----------------------
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    ValidateLastName();
                }
            }
        }

        private string _lastNameError;
        public string LastNameError
        {
            get => _lastNameError;
            set
            {
                _lastNameError = value;
                OnPropertyChanged(nameof(LastNameError));
                OnPropertyChanged(nameof(IsLastNameValid));
            }
        }

        public bool IsLastNameValid => string.IsNullOrEmpty(LastNameError);

        /// <summary>Valida el campo Apellido. Máximo 30 caracteres, obligatorio.</summary>
        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                LastNameError = "Campo obligatorio.";
            else if (LastName.Length > 30)
                LastNameError = "Máximo 30 caracteres.";
            else
                LastNameError = string.Empty;
        }

        // -------------------
        // ---- Documento ----
        // -------------------
        private string _document;
        public string Document
        {
            get => _document;
            set
            {
                if (_document != value)
                {
                    _document = value;
                    OnPropertyChanged(nameof(Document));
                    ValidateDocument();
                }
            }
        }

        private string _documentError;
        public string DocumentError
        {
            get => _documentError;
            set
            {
                _documentError = value;
                OnPropertyChanged(nameof(DocumentError));
                OnPropertyChanged(nameof(IsDocumentValid));
            }
        }

        public bool IsDocumentValid => string.IsNullOrEmpty(DocumentError);

        /// <summary>Valida el Documento según el tipo (DNI, NIE, PAS).</summary>
        private void ValidateDocument()
        {
            if (string.IsNullOrWhiteSpace(Document))
            {
                DocumentError = "Campo obligatorio.";
                return;
            }

            if (SelectedDocType == null)
            {
                DocumentError = "Selecciona un tipo de documento.";
                return;
            }

            var doc = Document.ToUpper().Trim();
            var tipo = SelectedDocType?.id_mask?.ToUpper();

            switch (tipo)
            {
                case "DNI":
                    if (!Regex.IsMatch(Document, @"^\d{8}[A-Za-z]$"))
                    {
                        DocumentError = "DNI no válido. Debe tener 8 números y 1 letra.";
                    }
                    else if (!Regex.IsMatch(Document, @"^\d{8}[A-Z]$"))
                    {
                        DocumentError = "La letra del DNI debe estar en mayúscula.";
                    }
                    else
                    {
                        DocumentError = "";
                    }
                    break;

                case "NIE":
                    if (!Regex.IsMatch(Document, @"^[XYZxyz]\d{7}[A-Za-z]$"))
                    {
                        DocumentError = "NIE no válido. Formato: X1234567T.";
                    }
                    else if (!Regex.IsMatch(Document, @"^[XYZ]\d{7}[A-Z]$"))
                    {
                        DocumentError = "Las letras del NIE deben estar en mayúscula.";
                    }
                    else
                    {
                        DocumentError = "";
                    }
                    break;

                case "PAS":
                    DocumentError = doc.Length >= 5 ? "" : "Pasaporte demasiado corto.";
                    break;

                default:
                    DocumentError = "";
                    break;
            }

        }

        // -------------------
        // --- Dirección ---
        // -------------------
        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                    ValidateAddress();
                }
            }
        }

        private string _addressError;
        public string AddressError
        {
            get => _addressError;
            set
            {
                _addressError = value;
                OnPropertyChanged(nameof(AddressError));
                OnPropertyChanged(nameof(IsAddressValid));
            }
        }

        public bool IsAddressValid => string.IsNullOrEmpty(AddressError);

        /// <summary>Valida el campo Dirección. Máximo 50 caracteres, obligatorio.</summary>
        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                AddressError = "Campo obligatorio.";
            else if (Address.Length > 50)
                AddressError = "Máximo 50 caracteres.";
            else
                AddressError = string.Empty;
        }

        // --------------------
        // --- Teléfono 1 ---
        // --------------------
        private string _phone1;
        public string Phone1
        {
            get => _phone1;
            set
            {
                if (_phone1 != value)
                {
                    _phone1 = value;
                    OnPropertyChanged(nameof(Phone1));
                    ValidatePhone1();
                }
            }
        }

        private string _phone1Error;
        public string Phone1Error
        {
            get => _phone1Error;
            set
            {
                _phone1Error = value;
                OnPropertyChanged(nameof(Phone1Error));
                OnPropertyChanged(nameof(IsPhone1Valid));
            }
        }

        public bool IsPhone1Valid => string.IsNullOrEmpty(Phone1Error);

        /// <summary>Valida el campo Teléfono 1. Obligatorio, formato español.</summary>
        private void ValidatePhone1()
        {
            if (string.IsNullOrWhiteSpace(Phone1))
                Phone1Error = "Campo obligatorio.";
            else if (!Regex.IsMatch(Phone1, @"^[6-7]{1}[0-9]{8}$"))
                Phone1Error = "El formato del teléfono no es válido.";
            else
                Phone1Error = string.Empty;

            OnPropertyChanged(nameof(IsPhone1Valid));
        }


        // --------------------
        // --- Teléfono 2 ---
        // --------------------
        private string _phone2;
        public string Phone2
        {
            get => _phone2;
            set
            {
                if (_phone2 != value)
                {
                    _phone2 = value;
                    OnPropertyChanged(nameof(Phone2));
                    ValidatePhone2();
                }
            }
        }

        private string _phone2Error;
        public string Phone2Error
        {
            get => _phone2Error;
            set
            {
                _phone2Error = value;
                OnPropertyChanged(nameof(Phone2Error));
                OnPropertyChanged(nameof(IsPhone2Valid));
            }
        }

        public bool IsPhone2Valid => string.IsNullOrEmpty(Phone2Error);

        /// <summary>Valida el campo Teléfono 2. Opcional, pero debe tener formato si se especifica.</summary>
        private void ValidatePhone2()
        {
            if (string.IsNullOrWhiteSpace(Phone2))
                Phone2Error = string.Empty; // Es válido si está vacío (opcional)
            else if (!Regex.IsMatch(Phone2, @"^[6-7]{1}[0-9]{8}$"))
                Phone2Error = "El formato del teléfono no es válido.";
            else
                Phone2Error = string.Empty;

            OnPropertyChanged(nameof(IsPhone2Valid));
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

        /// <summary>
        /// Inicializa los datos necesarios para la vista de edición, como los tipos de documento y códigos postales.
        /// Además, selecciona el tipo de documento y código postal correspondientes al DTO original.
        /// </summary>
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

        /// <summary>
        /// Construye un objeto DTO (`UpdateProfileClientDto`) con los datos actuales del ViewModel,
        /// preparado para ser enviado a la API para actualizar el perfil del cliente.
        /// </summary>
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
    }
}