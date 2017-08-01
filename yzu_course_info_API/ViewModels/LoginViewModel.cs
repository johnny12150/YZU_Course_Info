using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using yzu_course_info_API.Models;

namespace yzu_course_info_API.ViewModels
{
    public class LoginViewModel : IValidatableObject
    {

        private Entities db = new Entities();


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "請輸入帳號")]
        [DisplayName("帳號")]
        public string mId { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼")]
        public string mPwd { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string mNickname { get; set; } = "teset";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //到DB抓使用者資料
            Member member = db.Member.Find(mId);
            //將使用者輸入的字串加密
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string cryptedPwd = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(mPwd)));
            
            //假如抓不到系統使用者資料
            if (!(member != null && cryptedPwd == member.mPwd))
            {
                yield return new ValidationResult("帳號或密碼錯誤", new string[] { "Account" });
            }

            try { mNickname = member.mNickname; }
            catch { }
           
        }
        
    }
}