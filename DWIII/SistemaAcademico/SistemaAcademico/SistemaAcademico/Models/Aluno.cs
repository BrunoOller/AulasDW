using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaAcademico.Models
{
    public class Aluno
    {
        public int AlunoId { get; set; }
        [Display(Name = "RA")]
        [Required(ErrorMessage = "O RA é Obrigatório!")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "O RA deve ter entre 4 e 10 caracteres!")]
        public string? Ra { get; set; }
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
