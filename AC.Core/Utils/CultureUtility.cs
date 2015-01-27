using System.Globalization;

namespace AC.Core.Utils
{
    public class CultureUtility
    {
        public virtual string GetCultureCode()
        {
            return CultureInfo.CurrentCulture.Name.ToLower();
        }
    }
}
