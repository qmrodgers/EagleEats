using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EagleEatsFinal.Data
{
    public class Item
    {
        [Key]
        public int Item_Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [Description("Leave a note for this item.")]
        [DisplayName("Note")]
        public string? Note { get; set; }
        [Description("Describe information such as the business that sells the item, etc.")]
        [DisplayName("Full Item Description")]
        public string? Description { get; set; }

    }
}