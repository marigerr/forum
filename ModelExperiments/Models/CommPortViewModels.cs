using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelExperiments.Models
{
    public class ThreadReplyViewModel
    {
        public ForumPost forumPost = new ForumPost();
        public string LoggedInUser { get; set; }
        public string LoggedInUserStatus { get; set; }
        public bool IsAdminOrMod { get; set; }
        public List<ForumPostDTO> ForumPostReplies { get; set; }
    }

    public class ForumListViewModel
    {
        public string ProfileUser { get; set; }
        public DateTime ProfileUserDateJoined { get; set; }
        public string LoggedInUser { get; set; }
        public string LoggedInUserStatus { get; set; }
        public bool IsAdminOrMod { get; set; }
        public List<ForumPostDTO> ForumPosts { get; set; }

    }

    public class ForumPostDTO
    {
        [Key]
        public int PostId { get; set; }
        public int? ParentPostId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool? IsRead { get; set; }

        public string UserName { get; set; }
        public DateTime DateJoined { get; set; }
        public string UserId { get; set; }
    }

    public class NewsFeedViewModel
    {
        public bool IsAdminOrMod { get; set; }
        public List<NewsItemDTO> NewsItems { get; set; }

    }

    public class NewsItemDTO
    {
        [Key]
        public int ItemId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string ItemContent { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? EventDateTime { get; set; }
        //public DateTime? EventTime { get; set; }
        public string Organizer { get; set; }
        public string OrganizerContactEmail { get; set; }
        public bool? IsEvent { get; set; }
    }

    public class UserRoleViewModel
    {
        [Key]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime DateJoined { get; set; }
        public IEnumerable<string> RoleList { get; set; }
    }
}