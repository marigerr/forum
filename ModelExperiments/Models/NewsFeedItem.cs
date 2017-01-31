using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelExperiments.Models
{
    public class NewsFeedItem
    {
        [Key]
        public int ItemId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string ItemContent { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string JSEventDateTime { get; set; }
        public string Organizer { get; set; }
        public string OrganizerContactEmail { get; set; }
        public bool? IsEvent { get; set; }
    }
}