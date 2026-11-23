using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniHelpDesk.Application.Validation;

namespace MiniHelpDesk.Tests;

[TestClass]
public sealed class TicketValidatorTests
{
    [TestMethod]
    public void BrakNazwy_JestBledem()
    {
        var result = TicketValidator.Validate("  ", "Drukarka nie działa", "Niski");
        Assert.IsFalse(result.IsValid);
        Assert.AreNotEqual("", result.TitleError);
    }

    [TestMethod]
    public void OpisZaKrotki_JestBledem()
    {
        var result = TicketValidator.Validate("Drukarka", "Krótki", "Średni");
        Assert.IsFalse(result.IsValid);
        Assert.AreNotEqual("", result.DescriptionError);
    }

    [TestMethod]
    public void BrakPriorytetu_JestBledem()
    {
        var result = TicketValidator.Validate("Drukarka", "Drukarka nie działa", null);
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void NieznanyPriorytet_JestBledem()
    {
        var result = TicketValidator.Validate("Drukarka", "Drukarka nie działa", "Krytyczny");
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void PoprawneDane_SaAkceptowane()
    {
        var result = TicketValidator.Validate(" Drukarka ", " Opis jest wystarczająco długi ", "Wysoki");
        Assert.IsTrue(result.IsValid);
    }
}
