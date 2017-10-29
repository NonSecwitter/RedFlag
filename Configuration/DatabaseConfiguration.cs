using System.Data.SqlClient;
using Newtonsoft.Json;

namespace AppConfiguration
{
    public class DatabaseConfiguration
    {
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();

        public string DataSource
        {
            get { return stringBuilder.DataSource; }
            set { stringBuilder.DataSource = value; }
        }

        public bool PersistSecurityInfo
        {
            get { return stringBuilder.PersistSecurityInfo; }
            set { stringBuilder.PersistSecurityInfo = value; }
        }

        public string InitialCatalog
        {
            get { return stringBuilder.InitialCatalog; }
            set { stringBuilder.InitialCatalog = value; }
        }

        public string UserID
        {
            get { return stringBuilder.UserID; }
            set { stringBuilder.UserID = value; }
        }

        public string Password
        {
            get { return stringBuilder.Password; }
            set { stringBuilder.Password = value; }
        }

        public bool Encrypt
        {
            get { return stringBuilder.Encrypt; }
            set { stringBuilder.Encrypt = value; }
        }

        public bool TrustServerCertificate
        {
            get { return stringBuilder.TrustServerCertificate; }
            set { stringBuilder.TrustServerCertificate = value; }
        }

        public bool IntegratedSecurity
        {
            get { return stringBuilder.IntegratedSecurity; }
            set { stringBuilder.IntegratedSecurity = value; }
        }

        [JsonIgnore]
        public string ConnectionString
        {
            get { return stringBuilder.ConnectionString; }
        }
    }
}