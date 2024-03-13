namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Athlete
    {
        public Athlete()
        {
            Dissusions = new HashSet<Dissusion>();
            RengAthleteInPeriods = new HashSet<RengAthleteInPeriod>();
            Archives = new HashSet<Archive>();
            PhysicalExamHistories = new HashSet<PhysicalExamHistory>();
            DocTypeAthelets = new HashSet<DocTypeAthelet>();
            IndividualHistorys = new HashSet<IndividualHistory>();
            MRecords = new HashSet<MRecord>();
            BMIS = new HashSet<BMI>();
        }
        public DateTime birthDateMiladi
        {
            get
            {
                return Birthdate.persianToMiladi();
            }
        }
        public int AthleteId { get; set; }
        public bool? Flag { get; set; }
        public bool? IsArchive { get; set; }
        public int? EmTypeId { get; set; }
        public string UserInsert { get; set; }
        [Required(ErrorMessage = "نام را وارد کنید")]
        [StringLength(50)]
        public string Fnam { get; set; }
        public int? CityId { get; set; }
        public int? Athid { get; set; }
        [StringLength(50)]
        public string AddressH { get; set; }
        [StringLength(50)]
        public string Expertise { get; set; }
        [StringLength(50)]
        public string AddressW { get; set; }
        [StringLength(5)]
        public string GoorohKhoni { get; set; }
        [Required(ErrorMessage = "نام خانوادگی را وارد کنید.")]
        [StringLength(50)]
        public string Lname { get; set; }
        public string LastEditDate { get; set; }
        public string LastEditeUser { get; set; }
        [StringLength(50)]
        public string RegisterDate { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "نام پدر را وارد کنید")]
        public string FatherName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "شماره شناسنامه را وارد کنید")]
        public string Shenasnameh { get; set; }
        [Required(ErrorMessage = "کد ملی را واردکنید.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        public string Melicode { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "محل صدور را وارد کنید")]

        public string Sadereh { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "شماره بیمه را وارد کنید")]

        public string bimeh { get; set; }
        
        [StringLength(10)]
        public string PersonalCode { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = " تاریخ باید 10 رقم باشد")]
        public string EnterDate { get; set; }

        public int? MaritalStatusId { get; set; }

        [UIHint("PersianDatePicker")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = " تاریخ باید 10 رقم باشد")]
        [Required(ErrorMessage = "تاریخ تولدرا وارد کنید")]

        public string Birthdate { get; set; }
        public int SematId { get; set; }
        public string Picture { get; set; }
        public string NoteAthlete { get; set; }
        public int DegreeId { get; set; }

        [StringLength(250)]
        public string Reshteh { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = " تاریخ باید 10 رقم باشد")]

        [UIHint("PersianDatePicker")]
        public string DateEnter { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = " تاریخ باید 10 رقم باشد")]
        [Required(ErrorMessage = "تاریخ ورود به سازمان را وارد کنید")]

        public string PointEnter { get; set; }
        [StringLength(11, MinimumLength =11, ErrorMessage = " موبایل باید 11 رقم باشد")]

        [Required(ErrorMessage = "موبایل را وارد کنید")]
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public int EmployeeStatusId { get; set; }
        public int? ShiftWorkId { get; set; }
        public int MRecordId { get; set; }
        [Required(ErrorMessage = "پست سازمانی را وارد کنید")]
        public int PostSazmaniId { get; set; }
        [Required(ErrorMessage = " سهمیه را وارد کنید")]
        public int ShareId { get; set; }
        [DisplayName("نام و خانوادگی")]
        [ScaffoldColumn(false)]
        public string FullName // مشتق
        {
            get
            {
                return string.Format("{0} {1}", Fnam, Lname);
            }
        }
        public int? InfoDocTitleTypeId { get; set; }
        public virtual ICollection<Dissusion> Dissusions { get; set; }

        public int? PhyEduId { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<BMI> BMIS { get; set; }
        public virtual EmployeeStatu EmployeeStatu { get; set; }
        public virtual EmployeType EmployeType { get; set; }
        public virtual MaritalStatu MaritalStatu { get; set; }
        public virtual PostSazmani PostSazmani { get; set; }
        public virtual Semat Semat { get; set; }
        public virtual Share Share { get; set; }
        public virtual ShiftWork ShiftWork { get; set; }
        public virtual ICollection<DocTypeAthelet> DocTypeAthelets { get; set; }
        public virtual ICollection<PhysicalExamHistory> PhysicalExamHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RengAthleteInPeriod> RengAthleteInPeriods { get; set; }
        public virtual ICollection<Archive> Archives { get; set; }

        public virtual ICollection<IndividualHistory> IndividualHistorys { get; set; }
        public virtual ICollection<Medical> Medicals { get; set; }

        public virtual ICollection<MRecord> MRecords { get; set; }

        internal static object ToPagedList(int p, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
