namespace TablesSQLSignInOut.Models
{
    public class AuditEntity
    {
        public int ID { get; set; }

        public string MetaData { get; set; }

        public DateTime StartTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }

        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
