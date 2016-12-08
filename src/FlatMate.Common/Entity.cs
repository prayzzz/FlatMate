namespace FlatMate.Common
{
    public abstract class Entity
    {
        protected Entity(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public bool IsSaved => Id != 0;
    }
}