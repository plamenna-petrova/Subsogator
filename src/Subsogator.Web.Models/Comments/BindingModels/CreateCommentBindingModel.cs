using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Models.Comments.BindingModels
{
    public class CreateCommentBindingModel
    {
        [Required]
        [StringLength(300, MinimumLength = 5,
            ErrorMessage = "The comment must be between 5 and 300 symbols in length")]
        public string Content { get; set; }
    }
}
