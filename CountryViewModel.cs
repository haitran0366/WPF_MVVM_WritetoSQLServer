using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_MVVM_WritetoSQLServer
{
    public class CountryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string name ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string countryName;
        public string CountryName 
        {
            get { return countryName; }
            set
            {
                countryName = value;
                NotifyPropertyChanged("CountryName");
            }
        }

        private string greeting;
        public string Greeting
        {
            get { return greeting; }
            set
            {
                greeting = value;
                NotifyPropertyChanged("Greeting");
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        public ICommand cmdSave { get; private set; }
        public bool CanExectute 
        {
            get { return !string.IsNullOrEmpty(CountryName) & !string.IsNullOrEmpty(Greeting); }
        }

        private string CnnStr = Properties.Settings.Default.WPF_Connection;
        public CountryViewModel()
        {
            cmdSave = new RelayCommand(Save, () => CanExectute);
        }

        private void Save()
        {
            SqlConnection con = new SqlConnection(CnnStr);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Insert tblCountry(CountryName,Greeting) Values(@CountryName,@Greeting)";
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            cmd.Parameters.AddWithValue("@Greeting", Greeting);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {

            }
            ErrorMessage = "Data saved successfully!";
        }
    }
}
