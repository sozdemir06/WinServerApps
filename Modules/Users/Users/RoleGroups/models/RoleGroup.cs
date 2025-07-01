using Users.RoleGroups.DomainEvents;
using Users.RoleGroups.Dtos;
using Users.RoleGroups.Enums;

namespace Users.RoleGroups.models
{
    public class RoleGroup : Aggregate<Guid>
    {
        private readonly List<RoleGroupTranslatate> _roleGroupTranslatates = [];
        public IReadOnlyCollection<RoleGroupTranslatate> RoleGroupTranslatates => _roleGroupTranslatates.AsReadOnly();
        private readonly List<RoleGroupItem> _roleGroupItems = [];
        public IReadOnlyCollection<RoleGroupItem> RoleGroupItems => _roleGroupItems.AsReadOnly();
        public RoleGroupViewPermission ViewPermission { get; private set; }
        public bool IsActive { get; private set; }

        // Private constructor for EF Core
        private RoleGroup() { }

        // Aggregate Root'un oluşturulması için fabrika metodu
        public static async Task<RoleGroup> Create(IEnumerable<RoleGroupTranslationDto> translations)
        {
            var roleGroup = new RoleGroup
            {
                Id = Guid.CreateVersion7(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Sadece dolu olan (boş olmayan) çevirileri ekle
            var validTranslations = translations.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue);
            foreach (var transDto in validTranslations)
            {
                var newTranslate = RoleGroupTranslatate.Create(transDto.Name, transDto.Description, transDto.LanguageId, roleGroup.Id);
                roleGroup._roleGroupTranslatates.Add(newTranslate);
            }
            roleGroup.AddDomainEvent(new RoleGroupCreatedEvent(roleGroup));
            return await Task.FromResult(roleGroup);
        }


        
    

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("RoleGroup is already active.");

            IsActive = true;
            AddDomainEvent(new RoleGroupActivatedEvent(this));
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("RoleGroup is already inactive.");

            IsActive = false;
            IsDeleted = true;
            AddDomainEvent(new RoleGroupDeactivatedEvent(this));
        }
    }
}

