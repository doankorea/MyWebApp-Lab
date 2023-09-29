using Amazon.OpsWorks.Model;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Required = System.ComponentModel.DataAnnotations.RequiredAttribute;
namespace myWebApp.Models
{

    public class Student
    {
        [Display(Name = "Mã sinh viên")]
        public int Id { get; set; }//Mã sinh viên
        [Required(ErrorMessage = "Họ Tên bắt buộc phải được nhập")]
        [Display(Name = "Họ tên")]
        [Range(4, 100, ErrorMessage = "Tên phải có ít nhất 4 ký tự và tối đa 100 ký tự.")]
        public string? Name { get; set; } //Họ tên
        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}gmail.com", ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; } //Email
        [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
        [Display(Name = "Mật khẩu")]
        [RegularExpression("^(?=.{12,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@!%*?&,#]).+$", ErrorMessage = "Mật khẩu không hợp lệ")]

        public string? Password { get; set; }//Mật khẩu
        [Required(ErrorMessage = "Ngành học bắt buộc phải được nhập")]
        [Display(Name = "Ngành học")]
        
        public Branch? Branch { get; set; }//Ngành học

        [Display(Name = "Giới tính")]
        public Gender? Gender { get; set; }//Giới tính
                                           
        [Required(ErrorMessage = "Hệ bắt buộc phải được nhập")]
        [Display(Name = "Hệ: true-chính quy, false-phi chính quy")]
        public bool IsRegular { get; set; }//Hệ: true-chính quy, false-phi chính quy
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }//Địa chỉ
        [Required(ErrorMessage = "Yêu cầu nhập ngày")]
        [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "Ngày không hợp lệ")]
        [DataType(DataType.Date, ErrorMessage = "Vui lòng nhập một ngày hợp lệ.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "Ngày sinh")]
            
        public DateTime? DateOfBorth { get; set; }//Ngày sinh    
        [DataType(DataType.Currency, ErrorMessage ="Vui lòng nhập số thực")]
        [Required(ErrorMessage ="Vui lòng nhập điểm")]
        [Range(0.0,10.0, ErrorMessage = "Điểm chỉ được chứa một chữ số sau dấu chấm")]
        [Display(Name= "Điểm")]
        public float? Diem { get; set; }
        
       
    }
}
