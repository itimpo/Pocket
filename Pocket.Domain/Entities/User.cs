using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pocket.Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    [StringLength(150)]
    public string PasswordHash { get; set; } = string.Empty;
}
