
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Branches.DomainEvents;

public record BranchUpdatedEvent(Branch Branch) : IDomainEvent;