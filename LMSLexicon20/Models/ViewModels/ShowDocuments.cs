using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class ShowDocuments
    {
        public int Id { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
