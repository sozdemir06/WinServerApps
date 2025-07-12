using Catalog.Categories.Models;
using Shared.DDD;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

public record AdminCategoryDeactivatedEvent(AdminCategory Category) : IDomainEvent;