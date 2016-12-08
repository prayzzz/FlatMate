namespace FlatMate.Common
{
    public class ModelPrivileges
    {
        public ModelPrivileges(bool isOwned, bool isEditable, bool isDeletable)
        {
            IsOwned = isOwned;
            IsEditable = isEditable;
            IsDeletable = isDeletable;
        }

        public bool IsDeletable { get; }

        public bool IsEditable { get; }

        public bool IsOwned { get; }
    }
}