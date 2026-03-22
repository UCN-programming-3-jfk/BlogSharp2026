using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSharp2026.DataAccess.Model;

//[Id] INT NOT NULL,
//	[FK_Author_Id] INT NOT NULL,
//	[PostTitle] NVARCHAR(100) NOT NULL,
//    [PostContent] NVARCHAR(MAX) NOT NULL,
//    [CreationDate] DATETIME2(7) NOT NULL,
public class BlogPost
{
    public int Id { get; set; }
    public int FK_Author_Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string PostTitle { get; set; }
    [Required]
    [MinLength(10)]
    public string PostContent { get; set; }
    public DateTime CreationDate { get; set; }

    public override string ToString()
    {
        return $"BlogPost {{ Id = {Id}, FK_Author_Id = {FK_Author_Id}, PostTitle = '{PostTitle}', PostContent = '{PostContent}', CreationDate = {CreationDate:O} }}";
    }
}
