using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jzm.Rent.WebApp45.Models
{
    public class Borrow
    {
        public int id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? BorrowDateOnUtc { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDateOnUtc { get; set; }
        public int BorrowDays { get; set; }
        public int UserInfo_id { get; set; }
        public string DeviceUID { get; set; }
        public int DeviceInfo_id { get; set; }
        public string Note { get; set; }
        public string UserLastName { get; set; }
        public string UserFirstName { get; set; }
        public string DeviceName { get; set; }
        public bool IsReturned { get; set; }
        public int BorrowQuantity { get; set; }
        public int ReturnQuantity { get; set; }

    }
}