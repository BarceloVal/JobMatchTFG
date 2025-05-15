using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Job_Match.Data.Models;
using Job_Match.Maui.ApiService;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;
using Job_Match.Maui.Models;
using System.Runtime.CompilerServices;
using System.Globalization;
using Job_Match.Data.Models.Dtos;

namespace Job_Match.Maui.ViewModels
{
    public class ChangeProfileToClientViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor del ViewModel. Inicializa los tipos de documento y códigos postales desde la API.
        /// </summary>
        public ChangeProfileToClientViewModel()
        {
            LoadDocTypesAsync();
            LoadPostalCodesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Lanza el evento que notifica a la interfaz que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad (se completa automáticamente si se omite).</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ApiServiceClass _apiService = new();

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

        /// <summary>
        /// Valida el nombre. Es obligatorio y debe tener máximo 30 caracteres.
        /// </summary>
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

        /// <summary>
        /// Valida el apellido. Es obligatorio y debe tener máximo 30 caracteres.
        /// </summary>
        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                LastNameError = "Campo obligatorio.";
            else if (LastName.Length > 30)
                LastNameError = "Máximo 30 caracteres.";
            else
                LastNameError = string.Empty;
        }

        // --------------------------------
        // ------- Tipo Documento -------
        // --------------------------------
        public ObservableCollection<m_doc_type> DocTypes { get; set; } = new();

        private m_doc_type _selectedDocType;
        public m_doc_type SelectedDocType
        {
            get => _selectedDocType;
            set
            {
                _selectedDocType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Carga los tipos de documento disponibles desde la API y los asigna a la lista correspondiente.
        /// </summary>
        private async void LoadDocTypesAsync()
        {
            var result = await _apiService.GetDocTypesAsync();
            if (result != null)
            {
                DocTypes.Clear();

                foreach (var item in result)
                {
                    Console.WriteLine($"Cargando tipo: {item.id_mask} - {item.name}");
                    DocTypes.Add(item);
                }
            }
            else
            {
                Console.WriteLine("No se recibieron datos del servidor.");
            }
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

        /// <summary>
        /// Valida el documento según el tipo seleccionado (DNI, NIE o PAS).
        /// </summary>
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
            var tipo = SelectedDocType.id_mask.ToUpper();

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

                case "NIF":
                    if (!Regex.IsMatch(Document, @"^[A-Za-z0-9]{1,9}$"))
                    {
                        DocumentError = "NIF no válido. Debe tener hasta 9 caracteres alfanuméricos.";
                    }
                    else if (!Regex.IsMatch(Document, @"^[A-Z0-9]{1,9}$"))
                    {
                        DocumentError = "El NIF debe estar en mayúsculas.";
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

        /// <summary>
        /// Valida la dirección. Es obligatoria y debe tener máximo 50 caracteres.
        /// </summary>
        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                AddressError = "Campo obligatorio.";
            else if (Address.Length > 50)
                AddressError = "Máximo 50 caracteres.";
            else
                AddressError = string.Empty;
        }

        // ---------------------
        // --- Código postal ---
        // ---------------------
        public ObservableCollection<m_postal_code> PostalCodes { get; set; } = new();

        private m_postal_code _selectedPostalCode;
        public m_postal_code SelectedPostalCode
        {
            get => _selectedPostalCode;
            set
            {
                if (_selectedPostalCode != value)
                {
                    _selectedPostalCode = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Carga los códigos postales disponibles desde la API y los asigna a la lista correspondiente.
        /// </summary>
        private async void LoadPostalCodesAsync()
        {
            List<m_postal_code> result = await _apiService.GetPostalCodesAsync();
            if (result != null)
            {
                PostalCodes.Clear();
                foreach (var code in result)
                {
                    PostalCodes.Add(code);
                }
            }
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

        /// <summary>
        /// Valida el teléfono principal. Es obligatorio y debe cumplir el formato español (9 dígitos empezando en 6 o 7).
        /// </summary>
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

        /// <summary>
        /// Valida el segundo teléfono. Es opcional, pero si se especifica debe tener formato válido.
        /// </summary>
        private void ValidatePhone2()
        {
            if (string.IsNullOrWhiteSpace(Phone2))
                Phone2Error = string.Empty;
            else if (!Regex.IsMatch(Phone2, @"^[6-7]{1}[0-9]{8}$"))
                Phone2Error = "El formato del teléfono no es válido.";
            else
                Phone2Error = string.Empty;

            OnPropertyChanged(nameof(IsPhone2Valid));
        }

        /// <summary>
        /// Realiza el proceso de cambio de proveedor a cliente.
        /// Valida los campos, construye el DTO y llama al servicio correspondiente.
        /// </summary>
        /// <returns>True si el cambio fue exitoso, False en caso de error.</returns>
        public async Task<bool> ChangeToClientAsync()
        {
            var session = UserSessionService.Instance;
            var password = session.Password;

            ValidateFirstName();
            ValidateLastName();
            ValidateDocument();
            ValidateAddress();
            ValidatePhone1();
            ValidatePhone2();

            if (!IsFirstNameValid || !IsLastNameValid || !IsDocumentValid || !IsAddressValid || !IsPhone1Valid)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Completa todos los campos obligatorios.", "Aceptar");
                return false;
            }

            if (SelectedDocType == null || SelectedPostalCode == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar tipo de documento y población.", "Aceptar");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo recuperar la contraseña.", "Aceptar");
                return false;
            }

            var dto = new ChangeProviderToClientDto
            {
                IdUser = session.Id,
                FirstName = FirstName,
                LastName = LastName,
                Document = Document,
                IdDocType = SelectedDocType.id,
                Address = Address,
                IdPostalCode = SelectedPostalCode.id,
                Phone1 = Phone1,
                Phone2 = Phone2,
                Password = session.Password
            };

            var success = await _apiService.ChangeProviderToClientAsync(dto);
            return success;
        }
    }
}