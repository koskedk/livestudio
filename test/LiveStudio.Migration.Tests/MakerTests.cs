using LiveStudio.Migration.Schema;

namespace LiveStudio.Migration.Tests;

[TestFixture]
public class MakerTests
{
    private  IMaker? _composer;
    private Chamber? _chamber;

    [SetUp]
    public void SetUp()
    {
        _composer = new Maker();
        
        _chamber = new Chamber("Kit");
        _chamber.Add("Name",FieldType.String);
        _chamber.Add("Expiry",FieldType.Date);
    }
    [Test]
    public void should_Make()
    {
        var def =new Composer().ComposeDefinition(_chamber);

        var made = _composer.Make(def,_chamber.Name,TestContext.CurrentContext.TestDirectory);
        Assert.That(made.Type,Is.Not.Null);
        Console.WriteLine(made.Type);
        Console.WriteLine(made.File);
    }

    [TestCase(100000)]
    [TestCase(640000)]
    [TestCase(750000)]
    public void shoul_CalPAYE(int annualIncome)
    {
        decimal taxableIncome = CalculateTaxableIncome(annualIncome);
        decimal taxLiability = CalculateTaxLiability(taxableIncome);

        // Display results
        Console.WriteLine($"Annual Income: {annualIncome:C}");
        Console.WriteLine($"Taxable Income: {taxableIncome:C}");
        Console.WriteLine($"Tax Liability: {taxLiability:C}");
        Console.WriteLine($"Take Home: {(taxableIncome-taxLiability):C}");
        Assert.Pass();
    }

    private decimal CalculateTaxableIncome(decimal annualIncome)
    {
        // For simplicity, assuming no deductions in this example
        return annualIncome;
    }

    private decimal CalculateTaxLiability(decimal taxableIncome)
    {
        // Tax rates (as of 2022)
        decimal[] taxRates = { 0.1m, 0.15m, 0.2m, 0.25m, 0.3m };
        decimal[] incomeBrackets = { 24000, 16000, 16000, 16000, decimal.MaxValue };

        decimal taxLiability = 0;

        for (int i = 0; i < taxRates.Length; i++)
        {
            if (taxableIncome <= 0)
                break;

            decimal taxableInThisBracket = Math.Min(taxableIncome, incomeBrackets[i]);
            taxLiability += taxableInThisBracket * taxRates[i];
            taxableIncome -= taxableInThisBracket;
        }

        return taxLiability;
    }
}