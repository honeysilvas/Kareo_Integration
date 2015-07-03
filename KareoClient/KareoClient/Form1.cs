using System;
using System.Windows.Forms;
using KareoClient.KareoAPI;

namespace KareoClient
{
    public partial class Form1 : Form
    {

        // ClientVersion: an optional field that identifies the name and version of your application.
        const string CLIENTVERSION = "enter_your_client_version_here";

        // CustomerKey: the customer key associated with your Kareo account 
        // Please see Section 2.1 of Kareo Integration API manual under Getting Your Customer Key for more info.
        const string CUSTOMERKEY = "enter_your_customer_key_here";

        // User: the login for an authorized user account in Kareo.
        const string USER = "enter_your_user_here";

        // Password: the password for an authorized user account in Kareo. 
        const string PASSWORD = "enter_your_password_here";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CallKareoService();
        }

        private void CallKareoService()
        {
            KareoServicesClient service = new KareoServicesClient();

            // Create the request header that is required for every call to the Kareo Service 
            RequestHeader requestHeader = new RequestHeader();
            //requestHeader.ClientVersion = CLIENTVERSION; 
            requestHeader.CustomerKey = CUSTOMERKEY;
            requestHeader.User = USER;
            requestHeader.Password = PASSWORD;

            // Create the filter for the GetPatients call 
            PatientFilter filter = new PatientFilter();
            filter.PracticeName = "enter_name_of_practice_here";
            //filter.FullName = "Doe, John";

            // Set the fields you want populated in the return by marking them as true 
            PatientFieldsToReturn fields = new PatientFieldsToReturn();
            fields.ID = true;
            fields.PracticeName = true;
            fields.PatientFullName = true;

            // Create the GetPatientsRequest object with the request information 
            GetPatientsReq request = new GetPatientsReq();
            request.RequestHeader = requestHeader;
            request.Filter = filter;
            request.Fields = fields;

            // This actually calls the Kareo Service passing in the parameters for the request 
            GetPatientsResp response = service.GetPatients(request);

            // Check the response for an error 
            if (response.ErrorResponse.IsError)
            {
                System.Diagnostics.Trace.WriteLine(response.ErrorResponse.ErrorMessage);
            }
            else if (!response.SecurityResponse.SecurityResultSuccess)
            {
                System.Diagnostics.Trace.WriteLine(response.SecurityResponse.SecurityResult);
            }
            else
            {
                // There was no error print out the data 
                foreach (PatientData p in response.Patients)
                {
                    System.Diagnostics.Trace.WriteLine(p.PatientFullName);
                }
            }
        }
    }
}