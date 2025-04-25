namespace freelanceNew.Models.Enums
{
    public enum UserRole
    {
        Admin,
        Freelancer,
        Client
    }

    public enum JobStatus
    {
        Open,
        InProgress,
        Completed,
        Cancelled
    }

    public enum ProposalStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public enum ContractStatus
    {
        Active,
        Completed,
        Terminated
    }
}
