//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TV.MeanChords.Data.Db.Context.DiscosChowell
{
    using System;
    using System.Collections.Generic;
    
    public partial class DiscTag
    {
        public int DiscTagId { get; set; }
        public int DiscId { get; set; }
        public int TagId { get; set; }
    
        public virtual Disc Disc { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
