﻿using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using System;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPageService : IBaseService<NccPage>
    {
        private readonly NccPageRepository _entityRepository;

        public NccPageService(NccPageRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccPage Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccPage Save(NccPage entity)
        {
            using (var txn = _entityRepository.BeginTransaction())
            {
                try
                {
                    _entityRepository.Add(entity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }

                return entity;
            }
        }

        public NccPage Update(NccPage entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if(oldEntity != null)
            {
                oldEntity.ModificationDate = DateTime.Now;
                oldEntity.ModifyBy = oldEntity.GetCurrentUserId();
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(entity, oldEntity);
                    _entityRepository.Eidt(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }
            
            return entity;
        }
        
        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Eidt(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccPage> GetAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccPage> GetAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccPage> GetAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccPage> GetAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        public NccPage CopyNewData(NccPage copyFrom, NccPage copyTo)
        {                
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;            
            copyTo.Status = copyFrom.Status;
            copyTo.AddToNavigationMenu = copyFrom.AddToNavigationMenu;
            copyTo.Content = copyFrom.Content;
            copyTo.MetaDescription = copyFrom.MetaDescription;
            copyTo.MetaKeyword = copyFrom.MetaKeyword;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.PageStatus = copyFrom.PageStatus;
            copyTo.PageType = copyFrom.PageType;
            copyTo.Parent = copyFrom.Parent;
            copyTo.PublishDate = copyFrom.PublishDate;
            copyTo.Slug = copyFrom.Slug;
            copyTo.Title = copyFrom.Title;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            return copyTo;
        }
        
    }
}