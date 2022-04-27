using System.ComponentModel.DataAnnotations;

namespace EagleEatsFinal.Data
{
    public class Item
    {
        [Key]
        public int Item_Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public string? Description { get; set; }

    }
}