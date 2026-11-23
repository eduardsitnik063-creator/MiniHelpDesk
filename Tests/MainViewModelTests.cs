using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniHelpDesk.Domain;
using System.Collections.Specialized;

namespace MiniHelpDesk.Tests;

[TestClass]
public sealed class MainViewModelTests
{
    [TestMethod]
    public void Dodanie_OdzwierciedlaZmianeKolekcji()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        int changes = 0;
        ((INotifyCollectionChanged)vm.Tickets).CollectionChanged += (_, _) => changes++;
        TestFactory.FillValid(vm);
        Assert.IsTrue(vm.TrySave());
        Assert.AreEqual(1, vm.Tickets.Count);
        Assert.AreEqual(1, changes);
        Assert.AreEqual(1, vm.TotalCount);
    }

    [TestMethod]
    public void NiepoprawneDane_BlokująZapis()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        vm.TitleInput = "a";
        vm.DescriptionInput = "za mało";
        Assert.IsFalse(vm.TrySave());
        Assert.AreEqual(0, vm.Tickets.Count);
        Assert.AreNotEqual("", vm.PriorityError);
    }

    [TestMethod]
    public void WalidacjaNaZywo_UkrywaBladPoPoprawie()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        vm.TitleInput = "a";
        Assert.AreNotEqual("", vm.TitleError);
        vm.TitleInput = "Drukarka";
        Assert.AreEqual("", vm.TitleError);
    }

    [TestMethod]
    public void Status_ObserverZmieniaLicznik()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        TestFactory.FillValid(vm);
        vm.TrySave();
        var ticket = vm.Tickets[0];
        var events = new List<string?>();
        ticket.PropertyChanged += (_, e) => events.Add(e.PropertyName);
        vm.AdvanceStatus(ticket);
        vm.AdvanceStatus(ticket);
        Assert.AreEqual(TicketOptions.Closed, ticket.Status);
        Assert.AreEqual(0, vm.ActiveCount);
        CollectionAssert.Contains(events, nameof(ticket.Status));
    }

    [TestMethod]
    public void Status_NiePodmieniaKolekcji()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        TestFactory.FillValid(vm);
        vm.TrySave();
        var tickets = vm.Tickets;
        vm.AdvanceStatus(vm.Tickets[0]);
        Assert.AreSame(tickets, vm.Tickets);
    }

    [TestMethod]
    public void Edycja_ZachowujeReferencje()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        TestFactory.FillValid(vm);
        vm.TrySave();
        var ticket = vm.Tickets[0];
        vm.StartEditing(ticket);
        Assert.AreEqual("Zapisz zmiany", vm.SaveButtonText);
        vm.TitleInput = "Zmiana nazwy";
        Assert.IsTrue(vm.TrySave());
        Assert.AreSame(ticket, vm.Tickets[0]);
        Assert.AreEqual("Zmiana nazwy", ticket.Title);
    }

    [TestMethod]
    public void BlednaEdycja_NieNadpisujeObiektu()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        TestFactory.FillValid(vm);
        vm.TrySave();
        var ticket = vm.Tickets[0];
        vm.StartEditing(ticket);
        vm.TitleInput = "a";
        Assert.IsFalse(vm.TrySave());
        Assert.AreEqual("Awaria komputera", ticket.Title);
    }

    [TestMethod]
    public void Usuniecie_AktualizujeWynikQuery()
    {
        using var sp = TestFactory.CreateProvider();
        var vm = TestFactory.CreateViewModel(sp);
        TestFactory.FillValid(vm);
        vm.TrySave();
        vm.Delete(vm.Tickets[0]);
        Assert.AreEqual(0, vm.TotalCount);
        Assert.AreEqual(0, vm.ActiveCount);
    }
}
