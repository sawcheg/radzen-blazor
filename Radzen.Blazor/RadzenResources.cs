using System.Reflection;

namespace Radzen.Blazor
{
    /// <summary>
    /// Const params
    /// </summary>
    public static class RadzenResources
    {
        /// <summary>
        /// Used package name
        /// </summary>
        public static readonly string PackageName = typeof(RadzenResources).Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        /// <summary>
        /// Full path to main js file for Radzen
        /// </summary>
        public static readonly string JsContent = $"_content/{PackageName}/Radzen.Blazor.js";
        /// <summary>
        /// Full path to material theme css
        /// </summary>
        public static readonly string CssMaterialBase = GetCssTheme("material");
        /// <summary>
        /// Full path to standart theme css
        /// </summary>
        public static readonly string CssStandartBase = GetCssTheme("standart");
        /// <summary>
        /// Full path to default theme css
        /// </summary>
        public static readonly string CssDefaultBase = GetCssTheme("default");
        /// <summary>
        /// Full path to standart theme css
        /// </summary>
        public static readonly string CssDarkBase = GetCssTheme("dark");
        /// <summary>
        /// Full path to standart theme css
        /// </summary>
        public static readonly string CssSoftwareBase = GetCssTheme("software");
        /// <summary>
        /// Full path to standart theme css
        /// </summary>
        public static readonly string CssHumanisticBase = GetCssTheme("humanistic");

        static string GetCssTheme(string themeName)
            => $"_content/{PackageName}/css/{themeName}-base.css";
    }
}