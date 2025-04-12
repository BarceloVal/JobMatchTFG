using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace Job_Match.Maui.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        // Lanza el evento cuando una propiedad cambia
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            {
                MailError = "Campo obligatorio.";
            }
            else if (!Regex.IsMatch(Mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MailError = "El formato del mail no es válido.";
            }
            else
            {
                MailError = string.Empty;
            }
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
            {
                PasswordError = "Campo obligatorio.";
            }
            else
            {
                PasswordError = string.Empty;
            }
        }
    }
}
