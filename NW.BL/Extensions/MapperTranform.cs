using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace NW.BL.Extensions
{
    public class MapperTransform<TEntity, TModel> where TEntity : class where TModel : class
    {
        private static Object Lock = new Object();

        public static TEntity ToEntity(TModel model)
        {
            lock (Lock)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
                return Mapper.Map<TModel, TEntity>(model);
            }
        }

        public static TModel ToModel(TEntity entity)
        {
            lock (Lock)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
                return Mapper.Map<TEntity, TModel>(entity);
            }
        }

        public static List<TEntity> ToEntityCollection(List<TModel> models)
        {
            lock (Lock)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
                return Mapper.Map<List<TModel>, List<TEntity>>(models);
            }
        }

        public static List<TModel> ToModelCollection(List<TEntity> entities)
        {
            lock (Lock)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
                return Mapper.Map<List<TEntity>, List<TModel>>(entities);
            }
        }
    }
}
