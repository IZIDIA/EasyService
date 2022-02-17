using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequest_App
{
    public class Request
    {
        public int Id { get; set; }
        public string Number { get; set; } // Номер заявки
        public string Date { get; set; } // Дата создания заявки

        public string UserName { get; set; }
        public string Location { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string WithoutMe { get; set; }
        public bool ProblemWithMyPc { get; set; }
        public string WorkTime { get; set; }

        public string TextOfRequest { get; set; }

        //JSON
       public string[] Coments { get; set; }

        public string Status { get; set; }

        public string ImagePath { get; set; } // путь к изображению
    }
}
