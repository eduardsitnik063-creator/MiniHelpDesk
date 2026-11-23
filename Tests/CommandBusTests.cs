using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Application.Queries;
using MiniHelpDesk.Application.Validation;
using MiniHelpDesk.Domain;

namespace MiniHelpDesk.Tests;

[TestClass]
public sealed class CommandBusTests
{
    private const string Description = "Drukarka nie drukuje dokumentów.";

    [TestMethod]
    public void CommandBus_DodajePrzezHandler()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        bus.Send(new AddTicketCommand("Drukarka", Description, "Niski"));
        Assert.AreEqual(1, queries.GetCounts().All);
        Assert.AreEqual("Drukarka", queries.GetAll()[0].Title);
    }

    [TestMethod]
    public void Handler_WalidujeRowniezBezWidoku()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        Assert.ThrowsExactly<TicketValidationException>(
            () => bus.Send(new AddTicketCommand("a", Description, "Niski")));
        Assert.AreEqual(0, queries.GetCounts().All);
    }

    [TestMethod]
    public void CommandBus_EdytujeIstniejacyObiekt()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        bus.Send(new AddTicketCommand("Drukarka", Description, "Niski"));
        var ticket = queries.GetAll()[0];
        bus.Send(new EditTicketCommand(ticket.Id, "Nowy tytuł", "Nowy opis problemu", "Wysoki"));
        Assert.AreSame(ticket, queries.GetAll()[0]);
        Assert.AreEqual("Wysoki", ticket.Priority);
    }

    [TestMethod]
    public void BlednaEdycja_NieZmieniaDanych()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        bus.Send(new AddTicketCommand("Drukarka", Description, "Niski"));
        var ticket = queries.GetAll()[0];
        Assert.ThrowsExactly<TicketValidationException>(
            () => bus.Send(new EditTicketCommand(ticket.Id, "x", "Krótki", "Wysoki")));
        Assert.AreEqual("Drukarka", ticket.Title);
    }

    [TestMethod]
    public void CommandBus_UsuwaZgloszenie()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        bus.Send(new AddTicketCommand("Drukarka", Description, "Niski"));
        bus.Send(new DeleteTicketCommand(queries.GetAll()[0].Id));
        Assert.AreEqual(0, queries.GetCounts().All);
    }

    [TestMethod]
    public void CommandBus_ZmieniaStatusICzytelneQueryLicznika()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        var queries = sp.GetRequiredService<ITicketQueries>();
        bus.Send(new AddTicketCommand("Drukarka", Description, "Niski"));
        var ticket = queries.GetAll()[0];
        bus.Send(new AdvanceTicketStatusCommand(ticket.Id));
        Assert.AreEqual(TicketOptions.InProgress, ticket.Status);
        bus.Send(new AdvanceTicketStatusCommand(ticket.Id));
        Assert.AreEqual(TicketOptions.Closed, ticket.Status);
        Assert.AreEqual(0, queries.GetCounts().Active);
    }

    [TestMethod]
    public void NieznanyId_NieTworzyZgloszenia()
    {
        using var sp = TestFactory.CreateProvider();
        var bus = sp.GetRequiredService<ICommandBus>();
        Assert.ThrowsExactly<KeyNotFoundException>(
            () => bus.Send(new AdvanceTicketStatusCommand(Guid.NewGuid())));
    }
}
