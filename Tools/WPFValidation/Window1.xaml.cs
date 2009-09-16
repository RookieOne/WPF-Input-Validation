using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ValidationQuickStart.BusinessEntities;
using Microsoft.Practices.EnterpriseLibrary.Validation;


namespace WPFValidation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {
        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { customer = value; }
        }
	
        public Window1()
        {
            InitializeComponent();

            customer = new Customer();
            customer.Address = new Address();
            epCustomer.DataContext = customer;
        }

        private void OnSubmit(object sender, RoutedEventArgs args)
        {
            //check if error providers is valid and set focus to first invalid control
            bool valid = epCustomer.Validate();
            if (!valid)
                epCustomer.GetFirstInvalidElement().Focus();

            StringBuilder errorSummary = new StringBuilder();
            errorSummary.AppendLine(valid ? "Data is valid." : "Data is not valid.");
            foreach (string error in epCustomer.ErrorMessages)
                errorSummary.AppendLine(error);
            
            MessageBox.Show(errorSummary.ToString());
        }
    }
}