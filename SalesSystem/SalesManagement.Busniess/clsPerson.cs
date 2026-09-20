//using SalesManagement.DataAccess;
//using System;
//using System.Data;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;

//namespace SalesManagement.Busniess
//{

//    /// <summary>
//    /// This class represents person information.
//    /// </summary>
//    public class clsPerson
//    {

//        public enum enMode { AddNew = 0, Update = 1 };
//        public enMode Mode = enMode.AddNew;

//        public enum enGender { Male = 0, Female = 1 };
//        public enGender Gender { set; get; }

//        public int PersonID { set; get; }

//        public string NationalNo { set; get; }

//        public string FirstName { set; get; }

//        public string SecondName { set; get; }

//        public string? ThirdName { set; get; }

//        public string LastName { set; get; }
//        public string FullName
//        {
//            get
//            {
//                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
//            }
//        }

//        public DateTime DateOfBirth { set; get; }

//        public string Address { set; get; }

//        public string Phone { set; get; }

//        public string? Email { set; get; }

//        public int NationalityCountryID { set; get; }
//        public clsCountry CountryInfo;

//        private string _ImagePath;

//        public string ImagePath
//        {
//            get { return _ImagePath; }
//            set { _ImagePath = value; }
//        }


//        public clsPerson()

//        {
//            this.PersonID = -1;
//            this.NationalNo = "";
//            this.FirstName = "";
//            this.SecondName = "";
//            this.ThirdName = "";
//            this.LastName = "";
//            this.DateOfBirth = DateTime.Now;
//            this.Gender = clsPerson.enGender.Male;
//            this.Address = "";
//            this.Phone = "";
//            this.Email = "";
//            this.NationalityCountryID = -1;
//            this.ImagePath = "";

//            Mode = enMode.AddNew;

//        }

//        private clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName,
//            string? ThirdName, string LastName, DateTime DateOfBirth, enGender Gender, string Address,
//            string Phone, string? Email, int NationalityCountryID, string ImagePath)

//        {
//            this.PersonID = PersonID;
//            this.NationalNo = NationalNo;
//            this.FirstName = FirstName;
//            this.SecondName = SecondName;
//            this.ThirdName = ThirdName;
//            this.LastName = LastName;
//            this.DateOfBirth = DateOfBirth;
//            this.Gender = Gender;
//            this.Address = Address;
//            this.Phone = Phone;
//            this.Email = Email;
//            this.NationalityCountryID = NationalityCountryID;
//            this.CountryInfo = clsCountry.Find(NationalityCountryID);
//            this.ImagePath = ImagePath;

//            Mode = enMode.Update;

//        }

//        /// <summary>
//        /// Add new Person and returns the result.
//        /// </summary>
//        /// <returns>If person added successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        private bool _AddNewPerson()
//        {
//            //call DataAccess Layer 

//            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName,
//                this.ThirdName, this.LastName, this.DateOfBirth, (byte) this.Gender, this.Address, this.Phone,
//                this.Email, this.NationalityCountryID, this.ImagePath);

//            return (this.PersonID != -1);
//        }

//        /// <summary>
//        /// Update Person info and returns the result.
//        /// </summary>
//        /// <returns>If person updated successfully return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        private bool _UpdatePerson()
//        {
//            //call DataAccess Layer 

//            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
//                this.ThirdName, this.LastName, this.DateOfBirth, (byte) this.Gender, this.Address, this.Phone,
//                this.Email, this.NationalityCountryID, this.ImagePath);

//        }

//        /// <summary>
//        /// Find Person info by <paramref name="PersonID"/> and returns the result.
//        /// </summary>
//        /// <param name="PersonID">The person id for find person info.</param>
//        /// <returns>If person find successfully return Person <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
//        public static clsPerson Find(int PersonID)
//        {

//            string? ThirdName = "", Email = "", ImagePath = "";
//            string NationalNo = "", FirstName = "", SecondName = "", LastName = "", Phone = "", Address = "";
//            DateTime DateOfBirth = DateTime.Now;
//            int NationalityCountryID = -1;
//            byte Gender = (byte)clsPerson.enGender.Male;

//            if (clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName,
//                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email,
//                ref NationalityCountryID, ref ImagePath))

//                return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
//                    ThirdName, LastName, DateOfBirth, (clsPerson.enGender) Gender, Address, Phone, Email,
//                    NationalityCountryID, ImagePath);
//            else
//                return null!;

//        }

//        /// <summary>
//        /// Find Person info by <paramref name="NationalNo"/> and returns the result.
//        /// </summary>
//        /// <param name="NationalNo">The National No for find person info.</param>
//        /// <returns>If person find successfully return Person <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
//        public static clsPerson Find(string NationalNo)
//        {

//            int PersonID = -1;
//            string? ThirdName = "", Email = "", ImagePath = "";
//            string FirstName = "", SecondName = "", LastName = "", Phone = "", Address = "";
//            DateTime DateOfBirth = DateTime.Now;
//            int NationalityCountryID = -1;
//            byte Gender = (byte)clsPerson.enGender.Male;

//            if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName,
//                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email,
//                ref NationalityCountryID, ref ImagePath))

//                return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
//                    ThirdName, LastName, DateOfBirth, (clsPerson.enGender) Gender, Address, Phone, Email,
//                    NationalityCountryID, ImagePath);
//            else
//                return null!;

//        }

//        /// <summary>
//        /// Find Person info by <paramref name="FullName"/> and returns the result.
//        /// </summary>
//        /// <param name="FullName">The National No for find person info.</param>
//        /// <returns>If person find successfully return Person <seealso cref="object"/>, otherwise <see langword="null"/>.</returns>
//        public static clsPerson FindByFullName(string FullName)
//        {

//            int PersonID = -1;
//            string? ThirdName = "", Email = "", ImagePath = "";
//            string NationalNo = "", FirstName = "", SecondName = "", LastName = "", Phone = "", Address = "";
//            DateTime DateOfBirth = DateTime.Now;
//            int NationalityCountryID = -1;
//            byte Gender = (byte)clsPerson.enGender.Male;

//            if (clsPersonData.GetPersonInfoByFullName(FullName, ref NationalNo, ref PersonID, ref FirstName, ref SecondName,
//                ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email,
//                ref NationalityCountryID, ref ImagePath))

//                return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
//                    ThirdName, LastName, DateOfBirth, (clsPerson.enGender)Gender, Address, Phone, Email,
//                    NationalityCountryID, ImagePath);
//            else
//                return null!;

//        }

//        /// <summary>
//        /// If Mode add new so will add new person and returns the result, 
//        /// if Mode update so will Save person changes and returns the result.
//        /// </summary>
//        /// <returns>If person save successfully into database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        public bool Save()
//        {


//            switch (Mode)
//            {
//                case enMode.AddNew:
//                    if (_AddNewPerson())
//                    {

//                        Mode = enMode.Update;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }

//                case enMode.Update:

//                    return _UpdatePerson();

//            }




//            return false;
//        }

//        /// <summary>
//        /// This function will get all persons from database. 
//        /// </summary>
//        /// <returns>All persons info into <seealso cref="DataView"/> class.</returns>
//        public static DataView GetAllPeople()
//        {
//            return clsPersonData.GetAllPeopleAsync();
//        }

//        /// <summary>
//        /// This function will delete person from database if <paramref name="PersonID"/> is exists and returns the result.
//        /// </summary>
//        /// <param name="PersonID">The Person ID for delete person info.</param>
//        /// <returns>If person deleted successfully from database return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        public static bool DeletePerson(int PersonID)
//        {
//            return clsPersonData.DeletePerson(PersonID);
//        }

//        /// <summary>
//        /// This function will check if person exists into database by <paramref name="PersonID"/> and returns the result, 
//        /// </summary>
//        /// <param name="PersonID">The Person ID for check person is exists.</param>
//        /// <returns>If person exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        public static bool isPersonExist(int PersonID)
//        {
//            return clsPersonData.IsPersonExist(PersonID);
//        }

//        /// <summary>
//        /// This function will check if person exists in database by <paramref name="NationalNo"/> and returns the result, 
//        /// </summary>
//        /// <param name="NationalNo">The National No for check person is exists.</param>
//        /// <returns>If person exists return <see langword="true"/>, otherwise <see langword="false"/>.</returns>
//        public static bool isPersonExist(string NationalNo)
//        {
//            return clsPersonData.IsPersonExist(NationalNo);
//        }

//    }
//}

using SalesManagement.DataAccess;
using System;
using System.Data;

namespace SalesManagement.Busniess
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enum enGender { Male = 0, Female = 1 }

        public enMode Mode = enMode.AddNew;
        public enGender Gender { get; set; }

        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        public clsCountry CountryInfo;

        public string FullName =>
            $"{FirstName} {SecondName} {ThirdName} {LastName}".Trim();

        // Default constructor — used when adding a new person
        public clsPerson()
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = enGender.Male;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";
            Mode = enMode.AddNew;
        }

        // Private constructor — used internally by Find() methods to build
        // a fully populated object from database data (sets Mode to Update)
        private clsPerson(int personID, string nationalNo, string firstName, string secondName,
            string? thirdName, string lastName, DateTime dateOfBirth, enGender gender,
            string address, string phone, string? email, int nationalityCountryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryID;
            CountryInfo = clsCountry.Find(nationalityCountryID);
            ImagePath = imagePath;
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            // Calls the data access layer to insert a new record
            // Returns the new PersonID, or -1 if insert failed
            PersonID = clsPersonData.AddNewPerson(
                NationalNo, FirstName, SecondName, ThirdName,
                LastName, DateOfBirth, (byte)Gender,
                Address, Phone, Email, NationalityCountryID, ImagePath);

            return PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                PersonID, NationalNo, FirstName, SecondName, ThirdName,
                LastName, DateOfBirth, (byte)Gender,
                Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        /// <summary>
        /// Saves the person — inserts if AddNew, updates if Update mode.
        /// Automatically switches Mode to Update after a successful insert.
        /// </summary>
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdatePerson();

                default:
                    return false;
            }
        }

        /// <summary>
        /// Finds a person by ID.
        /// Uses ref parameters to receive multiple values from the data layer in one call,
        /// since C# methods can only return one value directly.
        /// </summary>
        public static clsPerson Find(int personID)
        {
            string nationalNo = "", firstName = "", secondName = "",
                    lastName = "", phone = "", address = "";
            string? thirdName = "", email = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            int nationalityCountryID = -1;
            byte gender = (byte)enGender.Male;

            if (!clsPersonData.GetPersonInfoByID(personID,
                    ref nationalNo, ref firstName, ref secondName,
                    ref thirdName, ref lastName, ref dateOfBirth,
                    ref gender, ref address, ref phone,
                    ref email, ref nationalityCountryID, ref imagePath))
                return null!;

            return new clsPerson(personID, nationalNo, firstName, secondName,
                thirdName, lastName, dateOfBirth, (enGender)gender,
                address, phone, email, nationalityCountryID, imagePath);
        }

        /// <summary>
        /// Finds a person by National Number.
        /// Uses ref parameters — same reason as Find(int).
        /// </summary>
        public static clsPerson Find(string nationalNo)
        {
            int personID = -1;
            string firstName = "", secondName = "", lastName = "", phone = "", address = "";
            string? thirdName = "", email = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            int nationalityCountryID = -1;
            byte gender = (byte)enGender.Male;

            if (!clsPersonData.GetPersonInfoByNationalNo(nationalNo,
                    ref personID, ref firstName, ref secondName,
                    ref thirdName, ref lastName, ref dateOfBirth,
                    ref gender, ref address, ref phone,
                    ref email, ref nationalityCountryID, ref imagePath))
                return null!;

            return new clsPerson(personID, nationalNo, firstName, secondName,
                thirdName, lastName, dateOfBirth, (enGender)gender,
                address, phone, email, nationalityCountryID, imagePath);
        }

        /// <summary>
        /// Finds a person by Full Name.
        /// Uses ref parameters — same reason as Find(int).
        /// </summary>
        public static clsPerson FindByFullName(string fullName)
        {
            int personID = -1;
            string nationalNo = "", firstName = "", secondName = "",
                     lastName = "", phone = "", address = "";
            string? thirdName = "", email = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            int nationalityCountryID = -1;
            byte gender = (byte)enGender.Male;

            if (!clsPersonData.GetPersonInfoByFullName(fullName,
                    ref nationalNo, ref personID, ref firstName,
                    ref secondName, ref thirdName, ref lastName,
                    ref dateOfBirth, ref gender, ref address,
                    ref phone, ref email, ref nationalityCountryID,
                    ref imagePath))
                return null!;

            return new clsPerson(personID, nationalNo, firstName, secondName,
                thirdName, lastName, dateOfBirth, (enGender)gender,
                address, phone, email, nationalityCountryID, imagePath);
        }

        public static DataView GetAllPeople() => clsPersonData.GetAllPeopleAsync();
        public static bool DeletePerson(int id) => clsPersonData.DeletePerson(id);
        public static bool isPersonExist(int id) => clsPersonData.IsPersonExist(id);
        public static bool isPersonExist(string nationalNo) => clsPersonData.IsPersonExist(nationalNo);
    }
}
