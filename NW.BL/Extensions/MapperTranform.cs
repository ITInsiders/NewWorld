using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace NW.BL.Extensions
{
    public class MapperTransform<TEntity, TModel> where TEntity : class where TModel : class
    {
        private static Mutex mutexLock = new Mutex();

        public static TEntity ToEntity(TModel model)
        {
            mutexLock.WaitOne();
            Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
            TEntity entity = Mapper.Map<TModel, TEntity>(model);
            mutexLock.ReleaseMutex();
            return entity;
        }

        public static TModel ToModel(TEntity entity)
        {
            mutexLock.WaitOne();
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
            TModel model = Mapper.Map<TEntity, TModel>(entity);
            mutexLock.ReleaseMutex();
            return model;
        }

        public static List<TEntity> ToEntityCollection(List<TModel> models)
        {
            mutexLock.WaitOne();
            Mapper.Initialize(cfg => cfg.CreateMap<TModel, TEntity>());
            List<TEntity> entitys = Mapper.Map<List<TModel>, List<TEntity>>(models);
            mutexLock.ReleaseMutex();
            return entitys;
        }

        public static List<TModel> ToModelCollection(List<TEntity> entities)
        {
            mutexLock.WaitOne();
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TModel>());
            List<TModel> models = Mapper.Map<List<TEntity>, List<TModel>>(entities);
            mutexLock.ReleaseMutex();
            return models;
        }
    }
}
