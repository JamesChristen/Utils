namespace Common.DAL
{
    internal abstract class GenericConverter<Entity, Dto>
    {
        public virtual Dto Convert(Entity from) 
            => throw new InvalidOperationException($"Cannot conver to entity {typeof(Entity).Name}");

        public virtual IEnumerable<Dto> Convert(IEnumerable<Entity> from)
        {
            foreach (Entity f in from)
            {
                yield return Convert(f);
            }
        }

        public virtual Entity Convert(Dto from)
            => throw new InvalidOperationException($"{typeof(Entity).Name} updates should be done in SQL");

        public virtual IEnumerable<Entity> Convert(IEnumerable<Dto> from)
        {
            foreach (Dto f in from)
            {
                yield return Convert(f);
            }
        }
    }
}
