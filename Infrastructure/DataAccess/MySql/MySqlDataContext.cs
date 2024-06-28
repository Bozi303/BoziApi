using MySql.Data.MySqlClient;

namespace Infrastructure.DataAccess.MySql
{
    public class MySqlDataContext
    {
        private readonly CustomerRepository _customerRepository;
        private readonly AdRepository _adRepository;
        private readonly CityRepository _cityRepository;
        private readonly ReportRepository _reportRepository;
        private readonly StoreRepository _storeRepository;

        public MySqlDataContext(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);  
            connection.Open();
            _customerRepository = new CustomerRepository(connection);
            _adRepository = new AdRepository(connection);
            _cityRepository = new CityRepository(connection);
            _reportRepository = new ReportRepository(connection);
            _storeRepository = new StoreRepository(connection);
        }

        public CustomerRepository CustomerRepository => _customerRepository;
        public AdRepository AdRepository => _adRepository;
        public CityRepository CityRepository => _cityRepository;
        public ReportRepository ReportRepository => _reportRepository;
        public StoreRepository StoreRepository => _storeRepository;
    }
}
