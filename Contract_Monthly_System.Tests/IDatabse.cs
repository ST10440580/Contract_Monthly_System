using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract_Monthly_System.Models;

namespace Contract_Monthly_System.Tests
{
    public interface IDatabase
    {
        bool store_claim(LectureViewer claim);
        List<LectureViewer> GetClaimsByLecturerId(int lecturerId);
    }
}
