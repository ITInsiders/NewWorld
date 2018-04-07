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
        public static TEntity ToEntity(TModel model)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
            return Mapper.Map<TModel, TEntity>(model);
        }

        public static TModel ToModel(TEntity entity)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
            return Mapper.Map<TEntity, TModel>(entity);
        }

        public static List<TEntity> ToEntityCollection(List<TModel> models)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
            return Mapper.Map<List<TModel>, List<TEntity>>(models);
        }

        public static List<TModel> ToModelCollection(List<TEntity> entities)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
            return Mapper.Map<List<TEntity>, List<TModel>>(entities);
        }
    }
}
