using Users.Managers.Models;
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Branches.DomainEvents;

public record BranchManagerRemovedEvent(Branch Branch, Manager Manager) : IDomainEvent;