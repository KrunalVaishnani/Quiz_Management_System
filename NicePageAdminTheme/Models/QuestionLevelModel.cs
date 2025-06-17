using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class QuestionLevelModel
{
    [Key]
    public int? QuestionLevelID { get; set; }

    [Required]
    [StringLength(100)]
    public string QuestionLevel { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public DateTime Modified { get; set; }
}

public class UserDropDownModel
{
    public int UserID { get; set; }
    public string UserName { get; set; }
}