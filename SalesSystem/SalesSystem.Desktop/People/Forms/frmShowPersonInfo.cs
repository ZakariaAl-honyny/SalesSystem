namespace SalesSystem.Desktop.People.Forms
{
    public partial class frmShowPersonInfo : Form
    {
        int _PersonID = -1;

        public frmShowPersonInfo()
        {
            InitializeComponent();
        }

        public void _LoadData(int personID)
        {
            _PersonID = personID;
            ctrlPersonCard1.LoadPersonInfo(_PersonID);
        }

        public void _LoadData(string nationalNo)
        {
            ctrlPersonCard1.LoadPersonInfoByNationalNo(nationalNo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
