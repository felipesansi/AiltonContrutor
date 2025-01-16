using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AiltonContrutor.TagHelper
{
    [HtmlTargetElement("email")]
    public class TagHelperEmail : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        public string Endereco { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Define o nome da tag HTML para "a"
            output.TagName = "a";

            // Constrói o atributo "href" com o prefixo "mailto:"
            var mailtoLink = $"mailto:{Endereco}";
            output.Attributes.SetAttribute("href", mailtoLink);

            // Define o conteúdo da tag "a"
            output.Content.SetContent(Conteudo);

            // Adiciona uma classe para estilização
            output.Attributes.Add("class", "email-link");
        }
    }
}
