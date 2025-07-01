

namespace Users.AppTenants.DomainEvents;

public record AppTenantUpdatedEvent(AppTenant AppTenant) : IDomainEvent
{
 
}