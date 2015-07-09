﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikeRobbins.SitecoreDataImporter.Entities;
using MikeRobbins.SitecoreDataImporter.Interfaces;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace MikeRobbins.SitecoreDataImporter.DataAccess
{
    public class AuditReader : IAuditReader
    {
        public ImportAudit GetLatestAudit()
        {
            var importAudit = new ImportAudit();

            var auditFolder = Sitecore.Data.Database.GetDatabase("master").GetItem(new ID("{1251023A-F7E0-4559-BCDF-04340C731EBE}"));

            if (auditFolder != null)
            {
                var lastestAudit = auditFolder.Children.OrderByDescending(x => x.Statistics.Created).FirstOrDefault();

                if (lastestAudit != null)
                {
                    importAudit.ImportedItems = GetTitles(lastestAudit, "Imported Items");
                }
            }

            return importAudit;
        }

        private List<string> GetTitles(Item auditItem, string fieldName)
        {
            var titles = new List<string>();

            var importedItemsField = (Sitecore.Data.Fields.MultilistField)auditItem.Fields[fieldName];

            if (importedItemsField!=null)
            {
                foreach (var item in importedItemsField.GetItems())
                {
                    titles.Add(item.Name);
                }
            }

            return titles;
        }
    }
}
