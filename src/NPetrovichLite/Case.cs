namespace NPetrovichLite
{
    public enum Case : int
    {
        /// <summary>
        /// именительный
        /// </summary>
        Nominative  = 0,

        /// <summary>
        ///родительный
        /// </summary>
        Genitive    = 1,

        /// <summary>
        /// дательный
        /// </summary>
        Dative      = 2,

        /// <summary>
        ///винительный
        /// </summary>
        Accusative  = 3,

        /// <summary>
        ///творительный
        /// </summary>
        Instrumental    = 4,

        /// <summary>
        ///предложный
        /// </summary>
        Prepositional   = 5
    }
}