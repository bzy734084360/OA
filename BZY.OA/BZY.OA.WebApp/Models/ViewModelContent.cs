using BZY.OA.Model.EnumType;

namespace BZY.OA.WebApp.Models
{
    public class ViewModelContent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public LuceneTypeEnum LuceneTypeEnum { get; set; }
    }
}