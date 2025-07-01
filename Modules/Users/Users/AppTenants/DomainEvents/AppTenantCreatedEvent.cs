namespace Users.AppTenants.DomainEvents;

public record AppTenantCreatedEvent(AppTenant AppTenant) : IDomainEvent
{
 
}