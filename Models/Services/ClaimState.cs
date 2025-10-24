
using Contract_Monthly_System.Models; // or wherever ClaimState is defined

namespace Contract_Monthly_System.Models.Services

{
    public enum ClaimState
    {
        Draft,
        Submitted,
        ApprovedByManager,
        RejectedByManager,
        ApprovedByProgrammeCoordinator,
        RejectedByProgrammeCoordinator,
        Pending,
        Finalized
    }
}
