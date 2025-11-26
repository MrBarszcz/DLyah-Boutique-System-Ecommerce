using System.Globalization;

namespace DLyah_Boutique_System.Extensions;

public static class StringExtensions {
    // Converte uma string para o formato "Title Case" (Primeira Letra Maiúscula).
    // Exemplo: "CAMISAS" -> "Camisas", "tênis de corrida" -> "Tênis De Corrida".
    public static string ToTitleCase( this string source ) {
        // Se a string for nula ou vazia, retorna ela mesma para evitar erros
        if ( string.IsNullOrWhiteSpace( source ) ) {
            return source;
        }

        TextInfo textInfo = new CultureInfo( "pt-BR", false ).TextInfo;

        return textInfo.ToTitleCase( source.ToLower() );
    }
}