using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract_Monthly_System.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contract_Monthly_System.Tests
{
    public class DatabaseWrapper : IDatabase
    {
        private readonly Database _realDb = new Database();

        public bool store_claim(LectureViewer claim)
        {
            return _realDb.store_claim(claim);
        }

        public List<LectureViewer> GetClaimsByLecturerId(int lecturerId)
        {
            return _realDb.GetClaimsByLecturerId(lecturerId);
        }
    }
}
