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

        public bool IsOwned { get;  }

        public bool IsEditable { get; }

        public bool IsDeletable { get; }
    }
}