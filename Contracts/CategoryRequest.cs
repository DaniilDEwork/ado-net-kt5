using System.ComponentModel.DataAnnotations;

namespace Kt5RepositoryApi.Contracts;

public class CategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
