using System.ComponentModel.DataAnnotations;

namespace TicTatToe.Data.Models;

public class User
{
    
    [Length(6, 200)]
    public required string UserName { get; init; } = null!;
    
    [MaxLength(256)]
    public string PasswordHashed { get; set; } = null!;
}