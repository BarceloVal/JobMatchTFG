using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job_Match.Maui.ApiService;

namespace Job_Match.Maui.ViewModels
{
    public class ProfileClientViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Guid IdDocType { get; set; }
        public string Document { get; set; }
        public string Address { get; set; }
        public Guid IdPostalCode { get; set; }
        public string PostalCodeName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ProfileClientViewModel(ProfileClientDto user)
        {
            user = App.CurrentUser as ProfileClientDto;
            if (user != null)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                Mail = user.Mail;
                IdDocType = user.IdDocType;
                Document = user.Document;
                Address = user.Address;
                IdPostalCode = user.IdPostalCode;
                PostalCodeName = user.PostalCodeName;
                Phone1 = user.Phone1;
                Phone2 = user.Phone2;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        private readonly ApiService.ApiService _apiService = new();
        public ObservableCollection<TaskScoreDisplayDto> ReceivedScores { get; set; } = new();

        public async Task LoadReceivedScoresAsync(Guid userId)
        {
            var scores = await _apiService.GetScoresReceivedByUserAsync(userId);
            ReceivedScores.Clear();
            foreach (var score in scores)
                ReceivedScores.Add(score);
        }

    }
}
