﻿using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikeRobbins.SitecoreDataImporter.DataAccess;
using MikeRobbins.SitecoreDataImporter.Entities;
using MikeRobbins.SitecoreDataImporter.Interfaces;
using Sitecore.Diagnostics.PerformanceCounters;
using StructureMap;
using MikeRobbins.SitecoreDataImporter.IoC;

namespace MikeRobbins.SitecoreDataImporter.Repositories
{
    public class ItemRespository : Sitecore.Services.Core.IRepository<DataItem>
    {
        private Container _container;

        public ItemRespository()
        {
            _container = new Container(new IoCRegistry());
        }

        public IQueryable<DataItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public DataItem FindById(string id)
        {
            throw new NotImplementedException();
        }

        public void Add(DataItem entity)
        {
            IItemCreator itemCreator = _container.GetInstance<IItemCreator>();
            IMediaReader mediaReader = _container.GetInstance<IMediaReader>();
            IFieldReader fieldReader = _container.GetInstance<IFieldReader>();
            IItemReader itemReader = _container.GetInstance<IItemReader>();

            var mediaItem = mediaReader.GetMediaItem(entity.MediaItemId);

            var importItems = fieldReader.GetFieldsFromMediaItem(mediaItem);

            foreach (var importItem in importItems)
            {
                itemCreator.ParentItem =itemReader.GetItem(entity.ParentId.ToString());
                itemCreator.Template = itemReader.GetTemplateItem(entity.TemplateId.ToString()); ;

                itemCreator.CreateItem(importItem);
            }
        }

        public bool Exists(DataItem entity)
        {
            return false;
        }

        public void Update(DataItem entity)
        {
            IItemUpdater itemUpdater = _container.GetInstance<IItemUpdater>();

            itemUpdater.UpdateItem(entity);

        }

        public void Delete(DataItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
