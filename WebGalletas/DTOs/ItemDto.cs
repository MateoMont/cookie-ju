namespace WebGalletas.DTOs
{
    public class ItemDto
    {
        public int GalletaId { get; set; }  // Id de la galleta seleccionada
        public int Cantidad { get; set; }   // Cantidad de galletas de ese tipo
        public bool Integral { get; set; }  // Si es integral o no
        public bool TieneSal { get; set; }  // Si tiene sal o no
    }
}
