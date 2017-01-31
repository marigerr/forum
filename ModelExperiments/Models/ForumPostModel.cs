using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ModelExperiments.Models
{
    public class ForumPost
    {
        [Key]
        public int PostId { get; set; }
        public int? ParentPostId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool? IsPrivate { get; set; }
        public string PmToUserName { get; set; }
        public bool? IsRead { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}