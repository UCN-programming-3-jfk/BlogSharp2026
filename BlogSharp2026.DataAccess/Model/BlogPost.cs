using System;
using System.Collections.Generic;
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
    public string PostTitle { get; set; }
    public string PostContent { get; set; }
    public DateTime CreationDate { get; set; }

}
