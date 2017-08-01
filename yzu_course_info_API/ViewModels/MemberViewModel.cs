using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Security.Cryptography;
using yzu_course_info_API.Models;
using System.Text;


namespace yzu_course_info_API.ViewModels
{
    public class MemberViewModel : IValidatableObject
    {

        private Entities db = new Entities();

        /// <summary>
        /// 帳號
        /// </summary>
        public string mId { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼")]
        public string mPwd { get; set; }

        /// <summary>
        /// 密碼確認
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼確認")]
        [Compare("mPwd", ErrorMessage = "輸入的密碼與上列不同")]
        public string ConfirmPwd { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "請輸入暱稱")]
        [DisplayName("暱稱")]
        public string mNickname { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //到DB抓使用者資料 看有沒有重複的使用者帳號
            Member member = db.Member.Find(mId);
            if (member == null)
                yield return new ValidationResult("抓不到你是誰呢....", new string[] { "Account" });

            //將使用者輸入的字串加密
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string cryptedPwd = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(mPwd)));

            /*db.Entry(new Member
            {
                mId = mId,
                mPwd = cryptedPwd,
                mNickname = mNickname
            }).State = EntityState.Modified;
            db.SaveChanges();*/

            db.Member.AddOrUpdate(new Member
            {
                mId = mId,
                mPwd = cryptedPwd,
                mNickname = mNickname
            });
            db.SaveChanges();
        }

        // 最安全請用 MembershipProvider

        // https://stackoverflow.com/questions/18493683/mvc-insert-post-data-into-database
    }
}