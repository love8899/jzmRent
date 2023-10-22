using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Jzm.Rent.WebApp45.Models
{
    public class DeviceInfo
    {
        public int id { get; set; }
        [Required(ErrorMessage = "* deviceUID can not be empty")]
       //[Remote(" "DeviceList", ErrorMessage = "DeviceUID already exists!")]
       [Remote("CheckDeviceUid", "DeviceList",ErrorMessage = "DeviceUID already exists!!")]
        public string DeviceUID { get; set; }
        public string DeviceName { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOnUtc { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ? UpdateOnUtc { get; set; }
        public int Quantity { get; set; }




    }
}
