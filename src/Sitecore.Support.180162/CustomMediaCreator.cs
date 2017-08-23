using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Pipelines.GetMediaCreatorOptions;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using System;
using System.IO;

namespace Sitecore.Support
{
    public class CustomMediaCreator : MediaCreator
    {
        protected override Item CreateFolder(string itemPath, MediaCreatorOptions options)
        {
            Assert.ArgumentNotNullOrEmpty(itemPath, "itemPath");
            Assert.ArgumentNotNull(options, "options");
            Item item2;
            using (new SecurityDisabler())
            {
                TemplateItem folderTemplate = this.GetFolderTemplate(options);
                Database database = this.GetDatabase(options);
                Item item = database.GetItem(itemPath, options.Language);
                if (item != null)
                {
                    return item;
                }
                item2 = database.CreateItemPath(itemPath, folderTemplate, folderTemplate);
                Assert.IsNotNull(item2, typeof(Item), "Could not create media folder: '{0}'.", new object[]
                {
                    itemPath
                });
            }
            return item2;
        }

        private TemplateItem GetFolderTemplate(MediaCreatorOptions options)
        {
            Assert.ArgumentNotNull(options, "options");
            Database database = this.GetDatabase(options);
            var language = options.Language ?? Context.Language;
            TemplateItem templateItem = database.Templates[TemplateIDs.MediaFolder, language];
            Assert.IsNotNull(templateItem, typeof(TemplateItem), "Could not find folder template for media. Template: '{0}'", new object[]
            {
                TemplateIDs.MediaFolder
            });
            return templateItem;
        }

        private Database GetDatabase(MediaCreatorOptions options)
        {
            Assert.ArgumentNotNull(options, "options");
            Database resultDatabase;
            if ((resultDatabase = options.Database) == null)
            {
                resultDatabase = (Context.ContentDatabase ?? Context.Database);
            }
            return Assert.ResultNotNull<Database>(resultDatabase);
        }
    }
}