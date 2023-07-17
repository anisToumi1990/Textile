
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Textile.DAL;

namespace Textile.BLL
{
    public class GenericBLL<TEntity> where TEntity : class
    {
        private static string _errorMessage = string.Empty;
        public static TEntity GetById(object id)
        {
            TextileEntities context = new TextileEntities();
            return context.Set<TEntity>().Find(id);
        }
        public static List<TEntity> GetAll()
        {
            TextileEntities context = new TextileEntities();
            return context.Set<TEntity>().ToList();
        }
        public static void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                TextileEntities context = new TextileEntities();
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public static void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                TextileEntities context = new TextileEntities();
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(_errorMessage, dbEx);
            }
        }

        public static void Delete(object id)
        {
            try
            {
                TextileEntities context = new TextileEntities();
                var entity = context.Set<TEntity>().Find(id);
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }
    }

}
