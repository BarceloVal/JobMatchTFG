namespace Job_Match.API.Models.Dtos
{
    public class CreateClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Document { get; set; }
        public Guid DocTypeId { get; set; }
        public string Address { get; set; }
        public Guid PostalCodeId { get; set; }
        public string Phone1 { get; set; }
        public string? Phone2 { get; set; }
    }

}
