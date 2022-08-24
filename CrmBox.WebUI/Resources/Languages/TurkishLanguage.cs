namespace CrmBox.WebUI.Resources.Languages;
internal class TurkishLanguage
{
    public const string Culture = "tr";

    public static string GetTranslation(string key) => key switch
    {
        "EmailValidator" => "'{PropertyName}'  geçerli bir e-posta adresi değil.",
        "GreaterThanOrEqualValidator" => "'{PropertyName}' değeri '{ComparisonValue}' değerinden büyük veya eşit olmalı.",
        "GreaterThanValidator" => "'{PropertyName}' değeri '{ComparisonValue}' değerinden büyük olmalı.",
        "LengthValidator" => "'{PropertyName}', {MinLength} ve {MaxLength} arasında karakter uzunluğunda olmalı . Toplam {TotalLength} adet karakter girdiniz.",
        "MinimumLengthValidator" => "'{PropertyName}', {MinLength} karakterden büyük veya eşit olmalıdır. {TotalLength} karakter girdiniz.",
        "MaximumLengthValidator" => "'{PropertyName}', {MaxLength} karakterden küçük veya eşit olmalıdır. {TotalLength} karakter girdiniz.",
        "LessThanOrEqualValidator" => "'{PropertyName}', '{ComparisonValue}' değerinden küçük veya eşit olmalı.",
        "LessThanValidator" => "'{PropertyName}', '{ComparisonValue}' değerinden küçük olmalı.",
        "NotEmptyValidator" => "'{PropertyName}' boş olmamalı.",
        "NotEqualValidator" => "'{PropertyName}', '{ComparisonValue}' değerine eşit olmamalı.",
        "NotNullValidator" => "'{PropertyName}' boş olamaz.",
        "PredicateValidator" => "Belirtilen durum '{PropertyName}' için geçerli değil.",
        "AsyncPredicateValidator" => "Belirtilen durum '{PropertyName}' için geçerli değil.",
        "RegularExpressionValidator" => "'{PropertyName}' değerinin formatı doğru değil.",
        "EqualValidator" => "'{PropertyName}', '{ComparisonValue}' değerine eşit olmalı.",
        "ExactLengthValidator" => "'{PropertyName}', {MaxLength} karakter uzunluğunda olmalı. {TotalLength} adet karakter girdiniz.",
        "InclusiveBetweenValidator" => "'{PropertyName}', {From} ve {To} arasında olmalı. {PropertyValue} değerini girdiniz.",
        "ExclusiveBetweenValidator" => "'{PropertyName}', {From} ve {To} (dahil değil) arasında olmalı. {PropertyValue} değerini girdiniz.",
        "CreditCardValidator" => "'{PropertyName}' geçerli bir kredi kartı numarası değil.",
        "ScalePrecisionValidator" => "'{PropertyName}', {ExpectedScale} ondalıkları için toplamda {ExpectedPrecision} rakamdan fazla olamaz. {Digits} basamak ve {ActualScale} basamak bulundu.",
        "EmptyValidator" => "'{PropertyName}' boş olmalıdır.",
        "NullValidator" => "'{PropertyName}' boş olmalıdır.",
        "EnumValidator" => "'{PropertyName}', '{PropertyValue}' içermeyen bir değer aralığı içeriyor.",
        // Additional fallback messages used by clientside validation integration.
        "Length_Simple" => "'{PropertyName}', {MinLength} ve {MaxLength} arasında karakter uzunluğunda olmalı.",
        "MinimumLength_Simple" => "'{PropertyName}', {MinLength} karakterden büyük veya eşit olmalıdır.",
        "MaximumLength_Simple" => "'{PropertyName}', {MaxLength} karakterden küçük veya eşit olmalıdır.",
        "ExactLength_Simple" => "'{PropertyName}', {MaxLength} karakter uzunluğunda olmalı.",
        "InclusiveBetween_Simple" => "'{PropertyName}', {From} ve {To} arasında olmalı.",
        _ => null,
    };
}