using Common.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Common.DAL
{
    internal class EntitySource<Entity, Dto> where Entity : class, new()
    {
        protected readonly ILog _logger;
        protected readonly IDbContextFactory _dbContextFactory;
        protected readonly HashSet<string> _entitiesToInclude;
        protected readonly GenericConverter<Entity, Dto> _converter;

        public EntitySource(ILog logger, IDbContextFactory dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _entitiesToInclude = new HashSet<string>();
        }

        public EntitySource(ILog logger, IDbContextFactory dbContextFactory, GenericConverter<Entity, Dto> converter)
            : this(logger, dbContextFactory)
        {
            _converter = converter;
        }

        public IQueryable<Entity> Aggregate(IQueryable<Entity> resultSet, IEnumerable<string> toInclude)
        {
            if (toInclude == null || !toInclude.Any())
            {
                return resultSet;
            }

            return toInclude.Aggregate(resultSet, (current, entityToLoad) => current.Include(entityToLoad).AsNoTracking());
        }

        public string IncludeEntity<TProperty>(Expression<Func<Entity, TProperty>> propertySelector)
        {
            string body = DbSetExtensions.Include(propertySelector);
            _entitiesToInclude.Add(body);
            return body;
        }

        public string IncludeEntity<T1, T2>(Expression<Func<Entity, ICollection<T1>>> collectionPropertySelector,
                                            Expression<Func<T1, T2>> propertyFromCollectionInstanceSelector)
        {
            string collectionBody = DbSetExtensions.Include(collectionPropertySelector);
            string propertyBody = DbSetExtensions.Include(propertyFromCollectionInstanceSelector);

            string body = string.Join(".", collectionBody, propertyBody);
            _entitiesToInclude.Add(body);
            return body;
        }

        public string IncludeEntity<T1, T2, T3>(Expression<Func<Entity, ICollection<T1>>> firstCollectionSelector,
                                                Expression<Func<T1, ICollection<T2>>> secondCollectionSelector,
                                                Expression<Func<T2, T3>> propertySelector)
        {
            string firstCollectionBody = DbSetExtensions.Include(firstCollectionSelector);
            string secondCollectionBody = DbSetExtensions.Include(secondCollectionSelector);
            string propertyBody = DbSetExtensions.Include(propertySelector);

            string body = string.Join(".", firstCollectionBody, secondCollectionBody, propertyBody);
            _entitiesToInclude.Add(body);
            return body;
        }

        private IEnumerable<string> IncludeEntity<T>(Expression<Func<Entity, T>> propertySelector, IEnumerable<string> toInclude)
        {
            string relation = IncludeExtensions.Include(propertySelector);
            if (!string.IsNullOrWhiteSpace(relation))
            {
                toInclude = toInclude.Select(x => $"{relation}.{x}").ToList();
            }

            foreach (string inclusion in toInclude)
            {
                _entitiesToInclude.Add(inclusion);
            }
            return toInclude;
        }

        public virtual Entity LoadSingleEntity(params Expression<Func<Entity, bool>>[] filters)
        {
            IEnumerable<Entity> entities = LoadEntities(filters);
            return Single(entities, filters);
        }

        public virtual IEnumerable<Entity> LoadEntities(params Expression<Func<Entity, bool>>[] filters)
        {
            using DbContext db = _dbContextFactory.CreateNew();
            db.Database.SetCommandTimeout(3600);

            Expression<Func<Entity, bool>> combinedFilter = CombineFilters(filters);
            IQueryable<Entity> resultSet = db.Set<Entity>().AsNoTracking().Where(combinedFilter);
            IEnumerable<Entity> loadedEntities = Aggregate(resultSet, _entitiesToInclude).ToList();
            return loadedEntities;
        }

        public virtual IEnumerable<Entity> LoadAllEntities() => LoadEntities(x => true);

        public virtual IEnumerable<Entity> LoadEntities(string sqlQuery)
        {
            using DbContext db = _dbContextFactory.CreateNew();
            db.Database.SetCommandTimeout(3600);

            IEnumerable<Entity> resultSet = db.Set<Entity>().FromSqlRaw(sqlQuery);
            return resultSet.ToList();
        }

        public virtual Dto LoadSingle(params Expression<Func<Entity, bool>>[] filters)
        {
            if (_converter == null)
            {
                throw new InvalidOperationException($"No converter set for {typeof(Entity).Name} to {typeof(Dto).Name}");
            }

            IEnumerable<Dto> result = Load(filters);
            return Single(result, filters);
        }

        public virtual IEnumerable<Dto> Load(params Expression<Func<Entity, bool>>[] filters)
        {
            return Load(validate: false, filters: filters);
        }

        public virtual IEnumerable<Dto> Load(bool validate, params Expression<Func<Entity, bool>>[] filters)
        {
            if (_converter == null)
            {
                throw new InvalidOperationException($"No converter set for {typeof(Entity).Name} to {typeof(Dto).Name}");
            }

            IEnumerable<Entity> entities = LoadEntities(filters);
            IEnumerable<Dto> result = _converter.Convert(entities);
            return result;
        }

        public virtual IEnumerable<Dto> LoadAll() => Load(x => true);

        public virtual IEnumerable<Dto> Load(string sqlQuery)
        {
            if (_converter == null)
            {
                throw new InvalidOperationException($"No converter set for {typeof(Entity).Name} to {typeof(Dto).Name}");
            }

            using DbContext db = _dbContextFactory.CreateNew();
            db.Database.SetCommandTimeout(3600);

            IEnumerable<Entity> entities = db.Set<Entity>().FromSqlRaw(sqlQuery);
            IEnumerable<Dto> result = _converter.Convert(entities);
            return result;
        }

        private static K Single<K>(IEnumerable<K> objects, params Expression<Func<Entity, bool>>[] filters)
        {
            switch (objects.Count())
            {
                case 0:
                    return default;
                case 1:
                    return objects.Single();
                default:
                    Expression<Func<Entity, bool>> expr = CombineFilters(filters);
                    string expression = expr.Body.ToString().Replace(expr.Parameters[0].ToString(), objects.First().GetType().Name);
                    throw new InvalidOperationException($"Only expected one {nameof(K)}, found {objects.Count()}. Expression: {expression}");
            }
        }

        private static Expression<Func<Entity, bool>> CombineFilters(params Expression<Func<Entity, bool>>[] filters)
        {
            if (filters == null || filters.Length == 0)
            {
                return x => true;
            }

            // Transform the filters into a single filter
            Expression<Func<Entity, bool>> combinedFilter = filters.First();
            ParameterExpression parameter = Expression.Parameter(typeof(Entity));

            for (int i = 1; i < filters.Length; i++)
            {
                ReplaceExpressionVisitor leftVis = new ReplaceExpressionVisitor(combinedFilter.Parameters[0], parameter);
                Expression left = leftVis.Visit(combinedFilter.Body);

                ReplaceExpressionVisitor rightVis = new ReplaceExpressionVisitor(filters[i].Parameters[0], parameter);
                Expression right = rightVis.Visit(filters[i].Body);
                combinedFilter = Expression.Lambda<Func<Entity, bool>>(Expression.AndAlso(left, right), parameter);
            }
            return combinedFilter;
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression oldValue;
            private readonly Expression newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == oldValue ? newValue : base.Visit(node);
            }
        }

        public virtual void InsertEntity(Entity entity)
          => InsertEntities(entity.AsSingleEnumerable());

        public virtual void InsertEntities(IEnumerable<Entity> entities)
        {
            using DbContext db = _dbContextFactory.CreateNew();
            db.Database.SetCommandTimeout(300000);
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            db.Set<Entity>().AddRange(entities);
            db.SaveChanges();
        }

        public virtual void Insert(Dto dto)
          => Insert(dto.AsSingleEnumerable());

        public virtual void Insert(IEnumerable<Dto> dtos)
        {
            if (_converter == null)
            {
                throw new InvalidOperationException($"No converter set for {typeof(Dto).Name} to {typeof(Entity).Name}");
            }

            IEnumerable<Entity> entities = _converter.Convert(dtos);
            InsertEntities(entities);
        }

        public virtual void DeleteByEntityExpr(Expression<Func<Entity, bool>> filter)
        {
            DeleteByEntityExprReturningNumAffected(filter);
        }

        public virtual int DeleteByEntityExprReturningNumAffected(Expression<Func<Entity, bool>> filter)
        {
            using DbContext db = _dbContextFactory.CreateNew();
            db.Database.SetCommandTimeout(300000);
            IQueryable<Entity> toDelete = db.Set<Entity>().Where(filter);
            toDelete = Aggregate(toDelete, _entitiesToInclude);
            db.Set<Entity>().RemoveRange(toDelete);
            return db.SaveChanges();
        }

        public virtual IEnumerable<Entity> Update(
            Expression<Func<Entity, bool>> selector,
            Entity replacement,
            Action<Entity, Entity> updateAction,
            bool insertOnNoMatch = false,
            bool updateMultiple = false)
        {
            return Update(replacement.AsSingleEnumerable(), x => selector, updateAction, insertOnNoMatch, updateMultiple);
        }

        public virtual IEnumerable<Entity> Update(
            IEnumerable<Entity> replacements,
            Func<Entity, Expression<Func<Entity, bool>>> selectorFunc,
            Action<Entity, Entity> updateAction,
            bool insertOnNoMatch = false,
            bool updateMultiple = false,
            int saveChunkSize = 500)
        {
            using DbContext db = _dbContextFactory.CreateNew();
            db.ChangeTracker.AutoDetectChangesEnabled = true;

            int affectedCount = 0;
            int count = 0;
            foreach (Entity replacement in replacements)
            {
                count++;
                Expression<Func<Entity, bool>> selector = selectorFunc(replacement);
                IQueryable<Entity> matchingEntities = db.Set<Entity>().AsNoTracking().Where(selector);
                matchingEntities = Aggregate(matchingEntities, _entitiesToInclude);

                switch (matchingEntities.Count())
                {
                    case 0:
                        if (insertOnNoMatch)
                        {
                            db.Set<Entity>().Add(replacement);
                        }
                        break;
                    case 1:
                        Entity singleEntity = matchingEntities.Single();
                        updateAction(singleEntity, replacement);
                        db.Set<Entity>().Update(singleEntity);
                        break;
                    default:
                        if (updateMultiple)
                        {
                            foreach (Entity entity in matchingEntities)
                            {
                                updateAction(entity, replacement);
                            }
                        }
                        else
                        {
                            _logger?.Error($"Found {matchingEntities.Count()} entities matching {replacement} and updateMultiple is set to false - not updating any!");
                        }
                        break;
                }

                if (count >= saveChunkSize)
                {
                    affectedCount += db.SaveChanges();
                    count = 0;
                }
            }

            affectedCount += db.SaveChanges();
            return replacements;
        }

        public Dto Convert(Entity entity) => _converter.Convert(entity);

        public IEnumerable<Dto> Convert(IEnumerable<Entity> entities) => _converter.Convert(entities);

        public Entity Convert(Dto dto) => _converter.Convert(dto);

        public IEnumerable<Entity> Convert(IEnumerable<Dto> dtos) => _converter.Convert(dtos);
    }

    internal static class IncludeExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static string IncludeEntity<Entity, TProperty>(
            this Entity entity,
            Expression<Func<Entity, TProperty>> propertySelector)
        {
            return Include(propertySelector);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static string IncludeEntity<Entity, T1, T2>(
            this Entity entity,
            Expression<Func<Entity, ICollection<T1>>> collectionPropertySelector,
            Expression<Func<T1, T2>> propertyFromCollectionInstanceSelector)
        {
            string collectionBody = Include(collectionPropertySelector);
            string propertyBody = Include(propertyFromCollectionInstanceSelector);

            return string.Join(".", collectionBody, propertyBody);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static string IncludeEntity<Entity, T1, T2, T3>(
            this Entity entity,
            Expression<Func<Entity, ICollection<T1>>> firstCollectionSelector,
            Expression<Func<T1, ICollection<T2>>> secondCollectionSelector,
            Expression<Func<T2, T3>> propertySelector)
        {
            string firstCollectionBody = Include(firstCollectionSelector);
            string secondCollectionBody = Include(secondCollectionSelector);
            string propertyBody = Include(propertySelector);

            return string.Join(".", firstCollectionBody, secondCollectionBody, propertyBody);
        }

        internal static string Include<T1, T2>(Expression<Func<T1, T2>> propertySelector)
        {
			// Expected format: x.ABC[.DEF][.GHI][...] => required: ABC[.DEF][.GHI][...]
			string body = Regex.Match(propertySelector.Body.ToString(), "\\.(.*)").Value.Trim('.');
			return body;
        }
    }
}
