using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Job_Match.Maui.ViewModels
{
    public class CreateClientAccountViewModel : INotifyPropertyChanged
    {
        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para lanzar el evento PropertyChanged
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        // -----------------------
        // --- Nombre completo ---
        // -----------------------
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                    ValidateFullName();
                }
            }
        }

        private string _fullNameError;
        public string FullNameError
        {
            get => _fullNameError;
            set
            {
                _fullNameError = value;
                OnPropertyChanged(nameof(FullNameError));
                OnPropertyChanged(nameof(IsFullNameValid));
            }
        }

        public bool IsFullNameValid => string.IsNullOrEmpty(FullNameError);

        private void ValidateFullName()
        {
            if (string.IsNullOrWhiteSpace(FullName))
                FullNameError = "Campo obligatorio.";
            else if (FullName.Length > 100)
                FullNameError = "Máximo 100 caracteres.";
            else
                FullNameError = string.Empty;
        }


        // ---------------
        // --- Mail ---
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
            else
                MailError = string.Empty;

            OnPropertyChanged(nameof(IsMailValid));
        }


        // ---------------------
        // --- Contraseña ---
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
            PasswordError = string.IsNullOrWhiteSpace(Password) ? "Campo obligatorio." : string.Empty;
            OnPropertyChanged(nameof(IsPasswordValid));
        }


        // --------------------------------
        // --- Repetir contraseña ---
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
