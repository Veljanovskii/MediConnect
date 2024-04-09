using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}