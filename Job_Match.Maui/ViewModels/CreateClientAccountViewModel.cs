using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Job_Match.Data.Models;
using Job_Match.Maui.ApiService;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;
using Job_Match.Maui.Models;
using System.Runtime.CompilerServices;

namespace Job_Match.Maui.ViewModels
{
    public class CreateClientAccountViewModel : INotifyPropertyChanged
    {
        public CreateClientAccountViewModel()
        {
            LoadDocTypesAsync();
            LoadPostalCodesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para lanzar el evento PropertyChanged
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

        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                LastNameError = "Campo obligatorio.";
            else if (LastName.Length > 30)
                LastNameError = "Máximo 30 caracteres.";
            else
                LastNameError = string.Empty;
        }


        // ---------------
        // ---- Mail ----
        // ---------------
        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                if (_mail != value)
                {
                    _mail = value;
                    OnPropertyChanged(nameof(Mail));
                    ValidateMail();
                }
            }
        }

        private string _mailError;
        public string MailError
        {
            get => _mailError;
            set
            {
                _mailError = value;
                OnPropertyChanged(nameof(MailError));
                OnPropertyChanged(nameof(IsMailValid));
            }
        }

        public bool IsMailValid => string.IsNullOrEmpty(MailError);

        private void ValidateMail()
        {
            if (string.IsNullOrWhiteSpace(Mail))
                MailError = "Campo obligatorio.";
            else if (!Regex.IsMatch(Mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                MailError = "El formato del correo no es válido.";
            else if (Mail.Length > 100)
                MailError = "Máximo 100 caracteres.";
            else
                MailError = string.Empty;

            OnPropertyChanged(nameof(IsMailValid));
        }


        // ---------------------
        // ---- Contraseña ----
        // ---------------------
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    ValidatePassword();
                    ValidateRepeatPassword(); // También se verifica coincidencia
                }
            }
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged(nameof(PasswordError));
                OnPropertyChanged(nameof(IsPasswordValid));
            }
        }

        public bool IsPasswordValid => string.IsNullOrEmpty(PasswordError);

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
                PasswordError = "Campo obligatorio.";
            else if (Password.Length > 100)
                PasswordError = "Máximo 100 caracteres.";
            else
                PasswordError = string.Empty;
            OnPropertyChanged(nameof(IsPasswordValid));
        }


        // --------------------------------
        // ----- Repetir contraseña -----
        // --------------------------------
        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    OnPropertyChanged(nameof(RepeatPassword));
                    ValidateRepeatPassword();
                }
            }
        }

        private string _repeatPasswordError;
        public string RepeatPasswordError
        {
            get => _repeatPasswordError;
            set
            {
                _repeatPasswordError = value;
                OnPropertyChanged(nameof(RepeatPasswordError));
                OnPropertyChanged(nameof(IsRepeatPasswordValid));
            }
        }

        public bool IsRepeatPasswordValid => string.IsNullOrEmpty(RepeatPasswordError);

        private void ValidateRepeatPassword()
        {
            RepeatPasswordError =
                string.IsNullOrWhiteSpace(RepeatPassword) ? "Campo obligatorio." :
                RepeatPassword != Password ? "Las contraseñas no coinciden." :
                string.Empty;

            OnPropertyChanged(nameof(IsRepeatPasswordValid));
        }


        // --------------------------------
        // ------- Tipo Documento -------
        // --------------------------------
        public ObservableCollection<DocType> DocTypes { get; set; } = new();

        private DocType _selectedDocType;
        public DocType SelectedDocType
        {
            get => _selectedDocType;
            set
            {
                _selectedDocType = value;
                OnPropertyChanged();
            }
        }

        private async void LoadDocTypesAsync()
        {
            var result = await _apiService.GetDocTypesAsync();
            if (result != null)
            {
                DocTypes.Clear();

                foreach (var item in result)
                {
                    Console.WriteLine($"Cargando tipo: {item.IdMask} - {item.Name}");
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
            var tipo = SelectedDocType.IdMask.ToUpper();

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
        // ---- Dirección ----
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

        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                AddressError = "Campo obligatorio.";
            else if (Address.Length > 50)
                AddressError = "Máximo 50 caracteres.";
            else
                AddressError = string.Empty;
        }

        // -------------------
        // -- Código postal --
        // -------------------
        public ObservableCollection<string> PostalCodes { get; set; } = new();

        private string _selectedPostalCode;
        public string SelectedPostalCode
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

        private async void LoadPostalCodesAsync()
        {
            var result = await _apiService.GetPostalCodesAsync(); // este método lo tenés en ApiService
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
        // ---- Teléfono 1 ----
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
        // ---- Teléfono 2 ----
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
    }
}
