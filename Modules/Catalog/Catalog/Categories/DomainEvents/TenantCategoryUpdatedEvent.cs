using Catalog.Categories.Models;
using Shared.DDD;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

public record TenantCategoryUpdatedEvent(TenantCategory Category) : IDomainEvent;