using System.Linq;
using Apex.Core;
using Apex.DAL.Abstracts;
using Apex.DAL.EF;
using Apex.Core.Entities.LocaleE;
using Apex.DAL.Helpers;

namespace Apex.DAL.Concretes
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApexContext context, IUnitOfWork unitOfWork, RequestInfo info) : base(context, unitOfWork, info)
        {
            //Debug.WriteLine($" contextId: {context.GetHashCode()}");
        }

        public long ActiveId()
        {
            var cc = CultureHelper.Get();
            return DbSet.Where(x => x.Locale.ToLower() == cc.ToLower()).Select(x => x.Id).FirstOrDefault();
        }

        public Language ActiveLanguage()
        {
            var cc = CultureHelper.Get();
            return DbSet.FirstOrDefault(x => x.Locale.ToLower() == cc.ToLower()) ?? new Language();
        }

        public Language FindByLocale(string locale)
        {
            return DbSet.FirstOrDefault(x => x.Locale.ToLower() == locale.ToLower());
        }


        public void RemoveCategoryFromLanguage(long categoryId)
        {
            var activeId = ActiveId();
            var translations = Context.CategoryTranslations.Where(m => m.LanguageId == activeId && m.CategoryId == categoryId);
            Context.CategoryTranslations.RemoveRange(translations);
        }

        public void RemoveProductFromLanguage(long productId)
        {
            var activeId = ActiveId();
            var translations = Context.ProductTranslations.Where(m => m.LanguageId == activeId && m.ProductId == productId);
            Context.ProductTranslations.RemoveRange(translations);
        }

        public void RemoveAdvertFromLanguage(long advertId)
        {
            var activeId = ActiveId();
            var translations = Context.AdvertTranslations.Where(m => m.LanguageId == activeId && m.AdvertId == advertId);
            Context.AdvertTranslations.RemoveRange(translations);
        }

        public void RemoveMasterInfoFromLanguage(long userId)
        {
            var activeId = ActiveId();
            var translations = Context.MasterInfoTranslations.Where(m => m.LanguageId == activeId && m.MasterInfoUserId == userId);
            Context.MasterInfoTranslations.RemoveRange(translations);
        }
        public void RemoveThemeSettingFromLanguage(long userId)
        {
            var activeId = ActiveId();
            var translations = Context.ThemeSettingTranslations.Where(m => m.LanguageId == activeId && m.ThemeSettingUserId == userId);
            Context.ThemeSettingTranslations.RemoveRange(translations);
        }
    }
}
