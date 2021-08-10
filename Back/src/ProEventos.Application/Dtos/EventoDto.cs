using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Local { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string DataEvento { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigatório"),
          StringLength(50, MinimumLength = 3, ErrorMessage = "{0} deve ter entre 3 e 50 caracteres")]
        public string Tema { get; set; }

        [Range(1, 1500, ErrorMessage = "{0} inválida, deve estar entre 1 e 1500")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Imagem inválida")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
        Phone(ErrorMessage = "Número de telefone inválido")]
        public string Telefone { get; set; }

        [Display(Name = "e-mail"),
        Required(ErrorMessage = "O campo {0} é obrigatório"),
        EmailAddress(ErrorMessage = "e-mail inválido")]
        public string Email { get; set; }
        public IEnumerable<LoteDto> Lote { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> PalestrantesDto { get; set; }
    }
}
