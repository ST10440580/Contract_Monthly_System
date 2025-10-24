using System.Collections.Generic;
using System.Linq;
using Contract_Monthly_System.Models;

namespace Contract_Monthly_System.Models.Services
{
    public class Claim_Memory
    {
        private static List<Claim> claims = new List<Claim>();

        public List<Claim> GetAll() => claims;

        public Claim Get(int id) => claims.FirstOrDefault(c => c.ClaimId == id);

        public void Add(Claim claim)
        {
            claim.ClaimId = claims.Count > 0 ? claims.Max(c => c.ClaimId) + 1 : 1;
            claims.Add(claim);
        }

        public void Update(Claim updatedClaim)
        {
            var index = claims.FindIndex(c => c.ClaimId == updatedClaim.ClaimId);
            if (index != -1)
                claims[index] = updatedClaim;
        }

        public void Delete(int id)
        {
            var claim = Get(id);
            if (claim != null)
                claims.Remove(claim);
        }
    }
}
