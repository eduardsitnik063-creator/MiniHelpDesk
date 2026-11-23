using System.Text;

namespace MiniHelpDesk.Tests;

public static class InAppTestRunner
{
    public static string RunAll()
    {
        var tests = new (string Name, Action Run)[]
        {
            ("Walidacja: brak nazwy", () => new TicketValidatorTests().BrakNazwy_JestBledem()),
            ("Walidacja: krótki opis", () => new TicketValidatorTests().OpisZaKrotki_JestBledem()),
            ("Walidacja: brak priorytetu", () => new TicketValidatorTests().BrakPriorytetu_JestBledem()),
            ("Walidacja: nieznany priorytet", () => new TicketValidatorTests().NieznanyPriorytet_JestBledem()),
            ("Walidacja: poprawne dane", () => new TicketValidatorTests().PoprawneDane_SaAkceptowane()),
            ("CQRS: dodawanie", () => new CommandBusTests().CommandBus_DodajePrzezHandler()),
            ("CQRS: walidacja w handlerze", () => new CommandBusTests().Handler_WalidujeRowniezBezWidoku()),
            ("CQRS: edycja", () => new CommandBusTests().CommandBus_EdytujeIstniejacyObiekt()),
            ("CQRS: błędna edycja", () => new CommandBusTests().BlednaEdycja_NieZmieniaDanych()),
            ("CQRS: usuwanie", () => new CommandBusTests().CommandBus_UsuwaZgloszenie()),
            ("CQRS: status i Query", () => new CommandBusTests().CommandBus_ZmieniaStatusICzytelneQueryLicznika()),
            ("CQRS: nieznany identyfikator", () => new CommandBusTests().NieznanyId_NieTworzyZgloszenia()),
            ("Observable: dodawanie", () => new MainViewModelTests().Dodanie_OdzwierciedlaZmianeKolekcji()),
            ("Formularz: błędne dane", () => new MainViewModelTests().NiepoprawneDane_BlokująZapis()),
            ("Formularz: walidacja na żywo", () => new MainViewModelTests().WalidacjaNaZywo_UkrywaBladPoPoprawie()),
            ("Observable: status i licznik", () => new MainViewModelTests().Status_ObserverZmieniaLicznik()),
            ("Observable: stabilna kolekcja", () => new MainViewModelTests().Status_NiePodmieniaKolekcji()),
            ("Formularz: edycja", () => new MainViewModelTests().Edycja_ZachowujeReferencje()),
            ("Formularz: błędna edycja", () => new MainViewModelTests().BlednaEdycja_NieNadpisujeObiektu()),
            ("Observable: usuwanie", () => new MainViewModelTests().Usuniecie_AktualizujeWynikQuery()),
        };

        int passed = 0;
        var failures = new StringBuilder();
        foreach (var test in tests)
        {
            try { test.Run(); passed++; }
            catch (Exception ex) { failures.AppendLine($"• {test.Name}: {ex.Message}"); }
        }

        var output = new StringBuilder();
        output.AppendLine($"Wynik: {passed}/{tests.Length} poprawnych testów.");
        output.AppendLine($"Niepowodzenia: {tests.Length - passed}.");
        if (passed != tests.Length) output.Append(failures);
        return output.ToString();
    }
}
