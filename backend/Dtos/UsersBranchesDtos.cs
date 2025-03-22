using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UserBranchDto
{
    public Guid UserBranchId { get; set; }
    public Guid UserId { get; set; }
    public Guid BranchId { get; set; }
}

public class UsersAndBranchesDtos
{
    public required List<UserAndBranchIdDto> UserAndBranchIdDtos { get; set; }
}

public class UserAndBranchIdDto
{
    [GuidNotEmpty]
    public Guid UserId { get; set; }
    
    [GuidNotEmpty]
    public Guid BranchId { get; set; }
}

public class UserBranchResponseStatusDto : ResponseStatusDto
{
    public required UserAndBranchIdDto UserAndBranchId { get; set; }
}