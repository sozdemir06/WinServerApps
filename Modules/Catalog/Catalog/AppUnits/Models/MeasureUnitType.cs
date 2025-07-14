namespace Catalog.AppUnits.Models
{
    public enum MeasureUnitType
    {
        /// <summary>
        /// Hiçbir sınıfa ait olmayan veya tanımsız birimler için kullanılır.
        /// Genellikle varsayılan değerdir ve bir hata durumunu gösterebilir.
        /// </summary>
        None = 0,

        /// <summary>
        /// Ağırlığı ifade eden birimler (kg, g, lbs).
        /// Veritabanı ID: 1
        /// </summary>
        Weight = 1,

        /// <summary>
        /// Uzunluk, genişlik, yükseklik gibi boyutları ifade eden birimler (cm, m, in).
        /// Veritabanı ID: 2
        /// </summary>
        Length = 2,

        /// <summary>
        /// Sıvı veya dökme ürünlerin hacmini ifade eden birimler (L, ml).
        /// Veritabanı ID: 3
        /// </summary>
        Volume = 3,

        /// <summary>
        /// Sayılabilir, tekil ürünleri ifade eder (adet, çift, set).
        /// Veritabanı ID: 4
        /// </summary>
        Quantity = 4
    }
}