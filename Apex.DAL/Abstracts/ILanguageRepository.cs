using Apex.Core.Entities.LocaleE;

namespace Apex.DAL.Abstracts
{
    public interface ILanguageRepository : IRepository<Language>
    {
        long ActiveId();
        Language ActiveLanguage();
        Language FindByLocale(string name);
        void RemoveCategoryFromLanguage(long categoryId);
        void RemoveProductFromLanguage(long productId);
        void RemoveAdvertFromLanguage(long advertId);
        void RemoveMasterInfoFromLanguage(long userId);
        void RemoveThemeSettingFromLanguage(long userId);
    }
}
