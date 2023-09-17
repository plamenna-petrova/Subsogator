using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Helpers
{
    public enum PageResultsEnum
    {
        [Display(Name = "3")]
        ThreeResults = 3,
        [Display(Name = "5")]
        FiveResults = 5,
        [Display(Name = "7")]
        SevenResults = 7,
        [Display(Name = "10")]
        TenResults = 10
    }
}
